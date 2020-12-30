using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.FSharp.Core;
using Models;
using WebApp.Common;
using static Models.DtoTypes;
using static Models.ServiceTypes;

#nullable enable

namespace WebApp.Data
{
    public class CosmosConnector : ICosmosConnector, IDisposable
    {
        private CosmosClient _client;
        private string _databaseId = "db";
        private readonly string _containerId = "items";
        private bool _isDisposed;
        private const string _partitionKeyPath = "/CustomerId";
        private const string _customerId = "justin@magaram.com";

#if DEBUG
        private bool _delay = false;
#else
        private bool _delay = false;
#endif
        private int _delaySeconds = 3;

        public CosmosConnector(string connectionString)
        {
            _client = new CosmosClient(connectionString);
        }

        public CosmosConnector(string endpointUri, string primaryKey, string applicationName, string databaseId = "db")
        {
            _databaseId = databaseId;
            _client = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = applicationName });
        }

        private async Task ArtificialDelay()
        {
            if (_delay)
            {
                await Task.Delay(_delaySeconds * 1000);
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public async Task CreateDatabaseAsync()
        {
            Database db = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
            Container ct = await db.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath);
        }

        public async Task DeleteDatabaseAsync()
        {
            Database db = _client.GetDatabase(_databaseId);
            await db.DeleteAsync();
        }

        public async Task<Changes> PushAsync(Changes c, CancellationToken cancel)
        {
            using var cancelIfException = new CancellationTokenSource();
            using var cancelRoot = CancellationTokenSource.CreateLinkedTokenSource(cancel, cancelIfException.Token);
            try
            {
                var items = await GetItems(cancelRoot.Token, c.Items);
                var categories = await GetItems(cancelRoot.Token, c.Categories);
                var purchases = await GetItems(cancelRoot.Token, c.Purchases);
                var stores = await GetItems(cancelRoot.Token, c.Stores);
                var notSoldItems = await GetItems(cancelRoot.Token, c.NotSoldItems);
                return new Changes(items, categories, stores, notSoldItems, purchases);
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                cancelIfException.Cancel();
                throw;
            }

            async Task<Document<T>[]> GetItems<T>(CancellationToken cancel, Document<T>[] x)
            {
                return ServiceModule.pushed(await PushBulkCoreAsync(x.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag, cancel));
            }
        }

        public Task<PushResult<T>[]> PushBulkCoreAsync<T>(
            IEnumerable<T> docs,
            Func<T, string> etag,
            CancellationToken cancel)
        {
            var upserts = docs.Select(i => PushCoreAsync(i, etag, cancel));
            return Task.WhenAll(upserts);
        }

        public async Task<PushResult<T>> PushCoreAsync<T>(T doc, Func<T, string> etag, CancellationToken cancel)
        {
            var container = _client.GetContainer(_databaseId, _containerId);
            try
            {
                var options = new ItemRequestOptions { IfMatchEtag = etag(doc) };
                var task = container.UpsertItemAsync(
                    item: doc,
                    requestOptions: options,
                    cancellationToken: cancel);
                var result = await task;
                return PushResult<T>.NewPushed(doc, result.Resource);
            }
            catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
            {
                return PushResult<T>.NewConcurrencyConflict(doc);
            }
        }

        public async Task<Changes> PullSinceAsync(int lastSync, CancellationToken token) => await PullCore(lastSync, token);

        public async Task<Changes> PullEverythingAsync(CancellationToken token) => await PullCore(null, token);

        private async Task<Changes> PullCore(int? lastSync, CancellationToken cancel)
        {
            var items = await PullByKindCore<Item>(_customerId, lastSync, DocumentKind.Item, cancel);
            var stores = await PullByKindCore<Store>(_customerId, lastSync, DocumentKind.Store, cancel);
            var categories = await PullByKindCore<Category>(_customerId, lastSync, DocumentKind.Category, cancel);
            var notSoldItems = await PullByKindCore<Unit>(_customerId, lastSync, DocumentKind.NotSoldItem, cancel);
            var purchases = await PullByKindCore<Unit>(_customerId, lastSync, DocumentKind.Purchase, cancel);
            var import = new Changes(items, categories, stores, notSoldItems, purchases);
            await ArtificialDelay();
            return import;
        }

        private async Task<Document<T>[]> PullByKindCore<T>(string customerId, int? timestamp, DocumentKind kind, CancellationToken cancel)
        {
            var container = _client.GetContainer(_databaseId, _containerId);
            var query = container.GetItemLinqQueryable<Document<T>>()
                .Where(i => i.Timestamp > (timestamp ?? int.MinValue))
                .Where(i => i.CustomerId == customerId)
                .Where(i => i.DocumentKind == kind);
            var docs = new List<Document<T>>();
            using (var iterator = query.ToFeedIterator())
            {
                while (iterator.HasMoreResults)
                {
                    foreach (var item in await iterator.ReadNextAsync(cancel))
                    {
                        docs.Add(item);
                    }
                }
            }
            return docs.ToArray();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _client?.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
