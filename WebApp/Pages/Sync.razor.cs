using Microsoft.AspNetCore.Components;
using Microsoft.Azure.Cosmos;
using Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApp.Pages {
    public partial class Sync : ComponentBase, IDisposable {
        private static readonly string EndpointUri = "https://localhost:8081";
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private CosmosClient _client;
        private Database _database;
        private Container _container;
        private const string _applicationName = "CosmosDBDotnetQuickstart";
        private readonly string _databaseId = "db";
        private readonly string _containerId = "items";
        private string _userId = "justin@magaram.com"; // Varies by customer; the partition key
        private const string _partitionKeyPath = "/UserId";

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        protected override async Task OnInitializedAsync() {
            IsReady = false;
            _client = CreateClient();
            await CreateDatabaseIfNotExistsAsync();
            await CreateContainerIfNotExistsAsync();
            IsReady = true;
        }

        private async Task ResetToEmpty() {
            IsReady = false;
            await CreateDatabaseIfNotExistsAsync();
            await DeleteDatabaseAsync();
            await CreateDatabaseIfNotExistsAsync();
            await CreateContainerIfNotExistsAsync();
            IsReady = true;
        }

        private static CosmosClient CreateClient() =>
            new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = _applicationName });

        private async Task CreateDatabaseIfNotExistsAsync() =>
            _database = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);

        private async Task CreateContainerIfNotExistsAsync() =>
            _container = await _database.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath, 400);

        protected async Task PushAsync() {
            var changes = Models.Dto.pushChanges(_userId, StateService.Current);
            var partitionKey = new PartitionKey(_userId);
            foreach (var i in changes.Items) {
                var doc = await _container.UpsertItemAsync(i, partitionKey);
            }
            foreach (var i in changes.Categories) {
                var doc = await _container.UpsertItemAsync(i, partitionKey);
            }
            foreach (var i in changes.Stores) {
                var doc = await _container.UpsertItemAsync(i, partitionKey);
            }
            foreach (var i in changes.NotSoldItems) {
                var doc = await _container.UpsertItemAsync(i, partitionKey);
            }
            StateService.Update(StateTypes.StateMessage.AcceptAllChanges);
        }

        protected async Task DeleteDatabaseAsync() {
            DatabaseResponse response = await _database.DeleteAsync();
        }

        private async Task AddItemsOfType<T>(PartitionKey partitionKey, IEnumerable<T> items, Func<T, string> id) {
            foreach (var i in items) {
                try {
                    ItemResponse<T> doc = await _container.ReadItemAsync<T>(id(i), partitionKey);
                }
                catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound) {
                    ItemResponse<T> doc = await _container.CreateItemAsync(i, partitionKey);
                }
            }
        }

        private async Task ReplaceItemAsync(string itemId) {
            var partitionKey = new PartitionKey(_userId);
            ItemResponse<DtoTypes.Item> item = await _container.ReadItemAsync<DtoTypes.Item>(itemId.ToString(), partitionKey);
            var itemBody = item.Resource;
            itemBody.Note = itemBody.Note + $" updated {DateTime.Now}";
            item = await _container.ReplaceItemAsync(itemBody, itemBody.ItemId.ToString(), partitionKey);
        }

        // store type of document in the schema so know how to deserialize it?
        private async Task QueryItemsAsync() => await QueryItemsAsyncOfType<Models.DtoTypes.Item>();

        private async Task QueryItemsAsyncOfType<T>() {
            var sqlQueryText = "SELECT * FROM c WHERE c.ItemName <> \"aaa\"";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            var queryResultSetIterator = _container.GetItemQueryIterator<T>(queryDefinition);
            var docs = new List<T>();
            while (queryResultSetIterator.HasMoreResults) {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var doc in currentResultSet) {
                    docs.Add(doc);
                }
            }
        }

        //    var queryable = container
        //.GetItemLinqQueryable<IDictionary<string, object>>();
        //    var oneDay = DateTime.UtcNow.AddDays(-1);
        //    var query = queryable
        //        .OrderByDescending(s => s["timestamp"])
        //        .Where(s => (DateTime)s["timestamp"] > oneDay);
        //    var iterator = query.ToFeedIterator();

        //private async Task QueryItemsAsync() {
        //    var sqlQueryText = "SELECT * FROM c";
        //    QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

        //    FeedIterator<DtoTypes.GroceryDocument> queryResultSetIterator = _container.GetItemQueryIterator<DtoTypes.GroceryDocument>(queryDefinition);
        //    List<DtoTypes.GroceryDocument> docs = new List<DtoTypes.GroceryDocument>();
        //    while (queryResultSetIterator.HasMoreResults) {
        //        FeedResponse<DtoTypes.GroceryDocument> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        //        foreach (DtoTypes.GroceryDocument doc in currentResultSet) {
        //            docs.Add(doc); // does not work; returns just base class
        //        }
        //    }
        //}

        // Querying a document AND mapping to specific type
        ////https://www.annytab.com/safe-update-in-cosmos-db-with-etag-asp-net-core/

        // https://github.com/Azure/azure-cosmos-dotnet-v3/blob/master/Microsoft.Azure.Cosmos.Samples/Usage/Queries/Program.cs#L154-L186


        protected bool IsReady { get; set; }

        public void Dispose() => _client.Dispose();

    }
}
