using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private TimeSpan _timeout = TimeSpan.FromSeconds(6);

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
            using CancellationTokenSource source = new(_timeout);
            try {
                var items = GetItems(c.Items);
                var categories = GetItems(c.Categories);
                var purchases = GetItems(c.Purchases);
                var stores = GetItems(c.Stores);
                var notSoldItems = GetItems(c.NotSoldItems);
                await Task.WhenAll(items, categories, purchases, stores, notSoldItems);
                return new Changes(items.Result, categories.Result, stores.Result, notSoldItems.Result, purchases.Result);
            }
            catch (OperationCanceledException) {
                return Dto.emptyChanges;
            }
            catch (CosmosException e) when (e.StatusCode is HttpStatusCode.ServiceUnavailable or HttpStatusCode.InternalServerError or HttpStatusCode.TooManyRequests) {
                source.Cancel();
                return Dto.emptyChanges;
            }
            async Task<Document<T>[]> GetItems<T>(Document<T>[] x) {
                var pushBulk = PushBulkCoreAsync(
                    x.Select(i => Dto.withCustomerId(_customerId, i)),
                    i => i.Etag,
                    source.Token);
                return
                    (await pushBulk)
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
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.PreconditionFailed) {
                return new ConcurrencyConflict<T>(doc);
            }
        }

        public async Task<Changes> PullIncrementalAsync(int after, int? before) => await PullCoreAsync(after, before);

        public async Task<Changes> PullEverythingAsync() => await PullCoreAsync(null, null);

        // Would be better for the return value to be a discriminated union
        // indicating success or one of the expected error conditions; that
        // would make it possible for the caller to implement smart logic for
        // retry and going online and offline. Detailed list of exceptions at
        // https://docs.microsoft.com/en-us/azure/cosmos-db/troubleshoot-dot-net-sdk
        private async Task<Changes> PullCoreAsync(int? after, int? before) {
            using CancellationTokenSource source = new(_timeout);
            try {
                var items = Pull<Item>(DocumentKind.Item);
                var stores = Pull<Store>(DocumentKind.Store);
                var categories = Pull<Category>(DocumentKind.Category);
                var notSoldItems = Pull<Unit>(DocumentKind.NotSoldItem);
                var purchases = Pull<Unit>(DocumentKind.Purchase);
                await Task.WhenAll(items, stores, categories, notSoldItems, purchases);
                var import = new Changes(items.Result, categories.Result, stores.Result, notSoldItems.Result, purchases.Result);
                return import;
            }
            catch (OperationCanceledException) {
                return Dto.emptyChanges;
            }
            catch (CosmosException e) when (e.StatusCode is HttpStatusCode.ServiceUnavailable or HttpStatusCode.InternalServerError or HttpStatusCode.TooManyRequests) {
                source.Cancel();
                return Dto.emptyChanges;
            }
            Task<Document<T>[]> Pull<T>(DocumentKind kind) => PullByKindCore<T>(_customerId, after, before, kind, source.Token);
        }

        private async Task<Document<T>[]> PullByKindCore<T>(string customerId, int? after, int? before, DocumentKind kind, CancellationToken cancel) {
            var container = _client.GetContainer(_databaseId, _containerId);
            var query = container.GetItemLinqQueryable<Document<T>>()
                .Where(i => i.Timestamp > (after ?? int.MinValue))
                .Where(i => i.Timestamp < (before ?? int.MaxValue))
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
