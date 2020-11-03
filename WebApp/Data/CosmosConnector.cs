using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.FSharp.Core;
using Models;
using WebApp.Common;
using static Models.DtoTypes;
using static Models.ServiceTypes;

namespace WebApp.Data {
    public class CosmosConnector : ICosmosConnector, IDisposable {
        private CosmosClient _client;
        private readonly string _databaseId = "db";
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

        public CosmosConnector(string connectionString) {
            _client = new CosmosClient(connectionString);
        }

        public CosmosConnector(string endpointUri, string primaryKey, string applicationName) {
            _client = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = applicationName });
        }

        private async Task ArtificialDelay() {
            if (_delay) {
                await Task.Delay(_delaySeconds * 1000);
            }
            else {
                await Task.CompletedTask;
            }
        }

        public async Task CreateDatabaseAsync() {
            Database db = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
            Container ct = await db.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath);
        }

        public async Task DeleteDatabaseAsync() {
            Database db = _client.GetDatabase(_databaseId);
            await db.DeleteAsync();
        }

        public async Task PushAsync(Changes c, CancellationToken cancel) {
            await PushCore(c.Items.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag, cancel);
            await PushCore(c.Categories.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag, cancel);
            await PushCore(c.Stores.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag, cancel);
            await PushCore(c.NotSoldItems.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag, cancel);
            await ArtificialDelay();
        }

        private async Task PushCore<T>(IEnumerable<T> items, Func<T, string> etag, CancellationToken cancel) {
            var ct = _client.GetContainer(_databaseId, _containerId);
            foreach (var i in items) {
                try {
                    await ct.UpsertItemAsync(item: i, requestOptions: new ItemRequestOptions { IfMatchEtag = etag(i) }, cancellationToken: cancel);
                }
                catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.PreconditionFailed) {
                }
            }
        }

        public async Task<Changes> PullSinceAsync(int lastSync, CancellationToken token) => await PullCore(lastSync, token);

        public async Task<Changes> PullEverythingAsync(CancellationToken token) => await PullCore(null, token);

        private async Task<Changes> PullCore(int? lastSync, CancellationToken cancel) {
            var items = await PullByKindCore<Item>(_customerId, lastSync, DocumentKind.Item, cancel);
            var stores = await PullByKindCore<Store>(_customerId, lastSync, DocumentKind.Store, cancel);
            var categories = await PullByKindCore<Category>(_customerId, lastSync, DocumentKind.Category, cancel);
            var notSoldItems = await PullByKindCore<Unit>(_customerId, lastSync, DocumentKind.NotSoldItem, cancel);
            var import = new Changes(items, categories, stores, notSoldItems);
            await ArtificialDelay();
            return import;
        }

        private async Task<Document<T>[]> PullByKindCore<T>(string customerId, int? timestamp, DocumentKind kind, CancellationToken cancel) {
            var container = _client.GetContainer(_databaseId, _containerId);
            var query = container.GetItemLinqQueryable<Document<T>>()
                .Where(i => i.Timestamp > (timestamp ?? int.MinValue))
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
