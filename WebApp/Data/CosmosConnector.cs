using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Models;
using WebApp.Common;
using static Models.DtoTypes;

namespace WebApp.Data {
    public class CosmosConnector : IDisposable {
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private readonly string _databaseId = "db";
        private readonly string _containerId = "items";
        private bool _isDisposed;
        private const string _partitionKeyPath = "/CustomerId";
        private const string _customerId = "justin@magaram.com";

        public CosmosConnector(string connectionString) {
            _client = new CosmosClient(connectionString);
        }

        public CosmosConnector(string endpointUri, string primaryKey, string applicationName) {
            _client = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = applicationName });
        }

        public async Task CreateDatabase() {
            if (_database == null || _container == null) {
                _database = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
                _container = await _database.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath);
            }
        }

        public async Task DeleteDatabase() {
            var db = _client.GetDatabase(_databaseId);
            await db.DeleteAsync();
            _database = null;
            _container = null;

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

        private async Task Push(Changes c) {
            await PushCore(c.Items.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
            await PushCore(c.Categories.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
            await PushCore(c.Stores.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
            await PushCore(c.NotSoldItems.Select(i => Dto.withCustomerId(_customerId, i)), i => i.Etag);
        }

        public async Task Push(StateTypes.State s) => await Dto.pushRequest(s).DoAsync(c => Push(c));

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

        public async Task<StateTypes.ImportChanges> Pull(int? lastSyncTimestamp, StateTypes.State s) {
            var items = await PullCore<Item>(_customerId, lastSyncTimestamp, DocumentKind.Item);
            var stores = await PullCore<Store>(_customerId, lastSyncTimestamp, DocumentKind.Store);
            var categories = await PullCore<Category>(_customerId, lastSyncTimestamp, DocumentKind.Category);
            var notSoldItems = await PullCore<Microsoft.FSharp.Core.Unit>(_customerId, lastSyncTimestamp, DocumentKind.NotSoldItem);
            var import = Dto.pullResponse(items, categories, stores, notSoldItems);
            return import;
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
