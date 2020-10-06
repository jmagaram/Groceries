using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Models.DtoTypes;

namespace WebApp.Common {
    public class CosmosConnector : IDisposable {
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private readonly string _databaseId = "db";
        private readonly string _containerId = "items";
        private readonly int? _throughput;
        private bool _isDisposed;
        private const string _partitionKeyPath = "/CustomerId";
        private const string _customerId = "justin@magaram.com";
        private readonly PartitionKey _partitionKey;

        public CosmosConnector(string endpointUri, string primaryKey, string applicationName, int? throughput = 400) {
            _client = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = applicationName });
            _partitionKey = new PartitionKey(_partitionKeyPath);
            _throughput = throughput;
        }

        public async Task CreateDatabase() {
            _database = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath, _throughput);
        }

        public async Task DeleteDatabase() {
            var db = _client.GetDatabase(_databaseId);
            await db.DeleteAsync();
        }

        public async Task<ItemResponse<T>> Upsert<T>(T item, string etag) {
            return await _container.UpsertItemAsync(item, requestOptions: new ItemRequestOptions { IfMatchEtag = etag });
        }

        private async Task PushCore<T>(IEnumerable<T> items, Func<T, string> etag) {
            foreach (var i in items) {
                try {
                    await _container.UpsertItemAsync(item: i, requestOptions: new ItemRequestOptions { IfMatchEtag = etag(i) });
                }
                catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.PreconditionFailed) {
                }
            }
        }

        public async Task Push(StateTypes.State s) {
            var changes = Models.Dto.changes(s);
            await PushCore(changes.Items.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
            await PushCore(changes.Categories.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
            await PushCore(changes.Stores.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
            await PushCore(changes.NotSoldItems.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
        }

        private async Task<List<Document<T>>> PullCore<T>(string customerId, int? timestamp, DocumentKind documentKind) {
            var query = _container.GetItemLinqQueryable<Document<T>>()
                .Where(i => i.Timestamp > (timestamp ?? int.MinValue))
                .Where(i => i.CustomerId == customerId)
                .Where(i => i.DocumentKind == documentKind);
            var docs = new List<Document<T>>();
            using (var iterator = query.ToFeedIterator()) {
                while (iterator.HasMoreResults) {
                    foreach (var item in await iterator.ReadNextAsync()) {
                        docs.Add(item);
                    }
                }
            }
            return docs;
        }

        public async Task<(StateTypes.State, int?)> Pull(int? lastSyncTimestamp, StateTypes.State s) {
            var items = await PullCore<Item>(_customerId, lastSyncTimestamp, DocumentKind.Item);
            var stores = await PullCore<Store>(_customerId, lastSyncTimestamp, DocumentKind.Store);
            var categories = await PullCore<Category>(_customerId, lastSyncTimestamp, DocumentKind.Category);
            var notSoldItems = await PullCore<Microsoft.FSharp.Core.Unit>(_customerId, lastSyncTimestamp, DocumentKind.NotSoldItem);
            if (items.Any() || stores.Any() || categories.Any() || notSoldItems.Any()) {
                lastSyncTimestamp =
                    items.Select(i => i.Timestamp)
                    .Concat(categories.Select(i => i.Timestamp))
                    .Concat(stores.Select(i => i.Timestamp))
                    .Concat(notSoldItems.Select(i => i.Timestamp))
                    .Max();
            }
            var itemChanges = items.Select(i => Dto.deserializeItem(i)).ToList();
            var storeChanges = stores.Select(i => Dto.deserializeStore(i)).ToList();
            var categoryChanges = Dto.deserializeCategories(categories).ToList();
            var notSoldItemChanges = notSoldItems.Select(i => Dto.deserializeNotSoldItem(i));
            var state = Dto.processPull(itemChanges, categoryChanges, storeChanges, notSoldItemChanges, s);
            return (state, lastSyncTimestamp);
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
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
