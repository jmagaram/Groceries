using Microsoft.AspNetCore.Components;
using Microsoft.Azure.Cosmos;
using Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebApp.Pages {
    public partial class Sync : ComponentBase, IDisposable {
        //<appSettings>
        //  <add key = "EndpointUri" value="https://localhost:8081" />
        //  <add key = "PrimaryKey" value="C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==" />
        //</appSettings>
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

        protected override void OnInitialized() => _client = CreateClient();

        private static CosmosClient CreateClient() =>
            new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = _applicationName });

        public void Dispose() => _client.Dispose();

        private async Task CreateDatabaseIfNotExistsAsync() =>
            _database = await _client.CreateDatabaseIfNotExistsAsync(_databaseId);

        private async Task CreateContainerIfNotExistsAsync() =>
            _container = await _database.CreateContainerIfNotExistsAsync(_containerId, _partitionKeyPath, 400);

        private async Task ResetToEmpty() {
            await CreateDatabaseIfNotExistsAsync();
            await DeleteDatabaseAsync();
            await CreateDatabaseIfNotExistsAsync();
            await CreateContainerIfNotExistsAsync();
        }

        protected async Task DeleteDatabaseAsync() {
            DatabaseResponse response = await _database.DeleteAsync();
        }

        static string _someId = "";

        private async Task AddItemsToContainerAsync() {
            var partitionKey = new PartitionKey(_userId);
            await AddItemsOfType(partitionKey, CosmosExperiment.items(_userId, StateService.Current), i => IdModule.serialize(i.ItemId));
            await AddItemsOfType(partitionKey, CosmosExperiment.categories(_userId, StateService.Current), i => IdModule.serialize(i.CategoryId));
            await AddItemsOfType(partitionKey, CosmosExperiment.stores(_userId, StateService.Current), i => IdModule.serialize(i.StoreId));
            await AddItemsOfType(partitionKey, CosmosExperiment.notSoldItems(_userId, StateService.Current), i => i.Id);
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

        private async Task DeleteItemAsync(Guid itemId) {
            var partitionKey = new PartitionKey(_userId);
            ItemResponse<StateTypes.Item> r = await _container.DeleteItemAsync<StateTypes.Item>(_someId.ToString(), partitionKey);
        }

        // <QueryItemsAsync>
        /// <summary>
        /// Run a query (using Azure Cosmos DB SQL syntax) against the container
        /// Including the partition key value of lastName in the WHERE filter results in a more efficient query
        /// </summary>
        //private async Task QueryItemsAsync() {
        //    var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

        //    Console.WriteLine("Running query: {0}\n", sqlQueryText);

        //    QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
        //    FeedIterator<Family> queryResultSetIterator = this.container.GetItemQueryIterator<Family>(queryDefinition);

        //    List<Family> families = new List<Family>();

        //    while (queryResultSetIterator.HasMoreResults) {
        //        FeedResponse<Family> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        //        foreach (Family family in currentResultSet) {
        //            families.Add(family);
        //            Console.WriteLine("\tRead {0}\n", family);
        //        }
        //    }
        //}

        //public async Task GetStartedDemoAsync() {
        //    // Create a new instance of the Cosmos Client
        //    this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
        //    await this.CreateDatabaseAsync();
        //    await this.CreateContainerAsync();
        //    await this.ScaleContainerAsync();
        //    await this.AddItemsToContainerAsync();
        //    await this.QueryItemsAsync();
        //    await this.ReplaceFamilyItemAsync();
        //    await this.DeleteFamilyItemAsync();
        //    await this.DeleteDatabaseAndCleanupAsync();
        //}
    }
}
