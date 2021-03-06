﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Cosmos.Fluent;
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
        private const string _applicationName = "groceries";
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(6);

        public CosmosConnector(string connectionString) {
            _client = new CosmosClientBuilder(connectionString).WithApplicationName(_applicationName).Build();
        }

        public CosmosConnector(string endpointUri, string primaryKey, string databaseId = "db") {
            _databaseId = databaseId;
            _client = new CosmosClientBuilder(endpointUri, primaryKey).WithApplicationName(_applicationName).Build();
        }

        public async Task CreateDatabaseAsync() {
            Database db = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);
            _ = await db.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath);
        }

        public async Task DeleteDatabaseAsync() {
            Database db = _client.GetDatabase(_databaseId);
            await db.DeleteAsync();
        }

        public async Task<Changes> PushAsync(string familyId, Changes c) {
            using CancellationTokenSource source = new(_timeout);
            c = Dto.affixFamilyId(familyId, c);
            try {
                var items = GetPushResponse(c.Items);
                var categories = GetPushResponse(c.Categories);
                var purchases = GetPushResponse(c.Purchases);
                var stores = GetPushResponse(c.Stores);
                var notSoldItems = GetPushResponse(c.NotSoldItems);
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
            async Task<Document<T>[]> GetPushResponse<T>(Document<T>[] docs) {
                var pushBulk = PushBulkCoreAsync(
                    docs: docs,
                    properties: i => new CosmosDocumentProperties(Id: i.Id, Etag: i.Etag, PartitionKey: i.CustomerId),
                    cancel: source.Token);
                var results = (await pushBulk);
                return
                    results
                    .Select(i => i.ServerVersion())
                    .ToArray();
            }
        }

        public Task<PushResult<T>[]> PushBulkCoreAsync<T>(
            IEnumerable<T> docs,
             Func<T, CosmosDocumentProperties> properties,
            CancellationToken cancel) {
            var upserts = docs.Select(i => PushCoreAsync(i, properties, cancel));
            return Task.WhenAll(upserts);
        }

        public async Task<PushResult<T>> PushCoreAsync<T>(T doc, Func<T, CosmosDocumentProperties> properties, CancellationToken cancel) {
            var props = properties(doc);
            var container = _client.GetContainer(_databaseId, _containerId);
            var options = new ItemRequestOptions { IfMatchEtag = props.Etag };
            var key = new PartitionKey(props.PartitionKey);
            try {
                var task = container.UpsertItemAsync(
                    item: doc,
                    partitionKey: key,
                    requestOptions: options,
                    cancellationToken: cancel);
                var result = await task;
                return new PushSuccess<T>(doc, result.Resource);
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.PreconditionFailed) {
                var serverDoc = await container.ReadItemAsync<T>(id: props.Id, partitionKey: key, cancellationToken: cancel);
                return new ConcurrencyConflict<T>(doc, serverDoc.Resource);
            }
        }

        public async Task<Changes> PullIncrementalAsync(string familyId, int after, int? before)
            => await PullCoreAsync(familyId, after, before);

        public async Task<Changes> PullEverythingAsync(string familyId)
            => await PullCoreAsync(familyId, null, null);

        // Would be better for the return value to be a discriminated union
        // indicating success or one of the expected error conditions; that
        // would make it possible for the caller to implement smart logic for
        // retry and going online and offline. Detailed list of exceptions at
        // https://docs.microsoft.com/en-us/azure/cosmos-db/troubleshoot-dot-net-sdk
        private async Task<Changes> PullCoreAsync(string familyId, int? after, int? before) {
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
            Task<Document<T>[]> Pull<T>(DocumentKind kind) => PullByKindCore<T>(familyId, after, before, kind, source.Token);
        }

        private async Task<Document<T>[]> PullByKindCore<T>(string familyId, int? after, int? before, DocumentKind kind, CancellationToken cancel) {
            var container = _client.GetContainer(_databaseId, _containerId);
            var requestOptions = new QueryRequestOptions { PartitionKey = new PartitionKey(familyId) };
            var query = container.GetItemLinqQueryable<Document<T>>(requestOptions: requestOptions)
                .Where(i => i.Timestamp > (after ?? int.MinValue))
                .Where(i => i.Timestamp < (before ?? int.MaxValue))
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
