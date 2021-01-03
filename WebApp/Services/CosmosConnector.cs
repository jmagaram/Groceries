using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.FSharp.Core;
using Models;
using static Models.DtoTypes;

#nullable enable

namespace WebApp.Services {
    public class CosmosConnector : ICosmosConnector, IDisposable {
        private readonly CosmosClient _client;
        private readonly string _databaseId = "db";
        private readonly string _containerId = "items";
        private bool _isDisposed;
        private const string _partitionKeyPath = "/CustomerId";
        private const string _customerId = "justin@magaram.com";
        private const int timoutMs = 5000;

        public CosmosConnector(string connectionString) {
            _client = new CosmosClient(connectionString);
        }

        public CosmosConnector(string endpointUri, string primaryKey, string applicationName, string databaseId = "db") {
            _databaseId = databaseId;
            _client = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = applicationName });
        }

        public async Task CreateDatabaseAsync() {
            Database db = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
            _ = await db.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath);
        }

        public async Task DeleteDatabaseAsync() {
            Database db = _client.GetDatabase(_databaseId);
            await db.DeleteAsync();
        }

        public async Task<Changes> PushAsync(Changes c) {
            // Might be able to push this up into the service; no need to handling it here
            // Also not tested at all to see how it handles connection issues
            using CancellationTokenSource cancel = new(timoutMs);
            try {
                var items = await GetItems(cancel.Token, c.Items);
                var categories = await GetItems(cancel.Token, c.Categories);
                var purchases = await GetItems(cancel.Token, c.Purchases);
                var stores = await GetItems(cancel.Token, c.Stores);
                var notSoldItems = await GetItems(cancel.Token, c.NotSoldItems);
                return new Changes(items, categories, stores, notSoldItems, purchases);
            }
            catch (Exception e) when (e is not OperationCanceledException) {
                cancel.Cancel();
                throw;
            }

            async Task<Document<T>[]> GetItems<T>(CancellationToken cancel, Document<T>[] x) {
                return
                    (await PushBulkCoreAsync(x.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag, cancel))
                    .OfType<PushSuccess<Document<T>>>()
                    .Select(i => i.Pull)
                    .ToArray();
            }
        }

        public Task<PushResult<T>[]> PushBulkCoreAsync<T>(
            IEnumerable<T> docs,
            Func<T, string> etag,
            CancellationToken cancel) {
            var upserts = docs.Select(i => PushCoreAsync(i, etag, cancel));
            return Task.WhenAll(upserts);
        }

        public async Task<PushResult<T>> PushCoreAsync<T>(T doc, Func<T, string> etag, CancellationToken cancel) {
            var container = _client.GetContainer(_databaseId, _containerId);
            try {
                var options = new ItemRequestOptions { IfMatchEtag = etag(doc) };
                var task = container.UpsertItemAsync(
                    item: doc,
                    requestOptions: options,
                    cancellationToken: cancel);
                var result = await task;
                return new PushSuccess<T>(doc, result.Resource);
            }
            catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.PreconditionFailed) {
                return new ConcurrencyConflict<T>(doc);
            }
        }

        public async Task<Changes> PullIncrementalAsync(int after, int? before) => await PullCore(after, before);

        public async Task<Changes> PullEverythingAsync() => await PullCore(null, new int?());

        private async Task<Changes> PullCore(int? after, int? before) {
            using CancellationTokenSource cancel = new(timoutMs);
            var items = await PullByKindCore<Item>(_customerId, after, before, DocumentKind.Item, cancel.Token);
            var stores = await PullByKindCore<Store>(_customerId, after, before, DocumentKind.Store, cancel.Token);
            var categories = await PullByKindCore<Category>(_customerId, after, before, DocumentKind.Category, cancel.Token);
            var notSoldItems = await PullByKindCore<Unit>(_customerId, after, before, DocumentKind.NotSoldItem, cancel.Token);
            var purchases = await PullByKindCore<Unit>(_customerId, after, before, DocumentKind.Purchase, cancel.Token);
            var import = new Changes(items, categories, stores, notSoldItems, purchases);
            return import;
        }

        private async Task<Document<T>[]> PullByKindCore<T>(string customerId, int? timestamp, int? earlierThan, DocumentKind kind, CancellationToken cancel) {
            var container = _client.GetContainer(_databaseId, _containerId);
            var query = container.GetItemLinqQueryable<Document<T>>()
                .Where(i => i.Timestamp > (timestamp ?? int.MinValue))
                .Where(i => i.Timestamp < (earlierThan ?? int.MaxValue))
                .Where(i => i.CustomerId == customerId)
                .Where(i => i.DocumentKind == kind);

            var docs = new List<Document<T>>();
            using (var iterator = query.ToFeedIterator()) {
                while (iterator.HasMoreResults) {
                    foreach (var item in await iterator.ReadNextAsync(cancel)) {
                        docs.Add(item);
                    }
                }
            }
            return docs.ToArray();
        }

        protected virtual void Dispose(bool disposing) {
            if (!_isDisposed) {
                if (disposing) {
                    _client?.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _isDisposed = true;
            }
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
