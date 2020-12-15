using System.Threading;
using System.Threading.Tasks;
using Models;
using WebApp.Common;
using WebApp.Data;
using Xunit;

namespace WebAppTest {
    public class CosmosConnectorTests {
        private static readonly string _localEndpointUri = "https://localhost:8081";
        private static readonly string _localPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string _applicationName = "CosmosDBDotnetQuickstart";

        [Fact]
        public async Task CanCreateDatabase() {
            using var c = TestConnector();
            await c.CreateDatabaseAsync();
        }

        [Fact]
        public async Task CanPushThenPullSameNumberOfItems() {
            using var c = TestConnector();
            await c.DeleteDatabaseAsync();
            await c.CreateDatabaseAsync();
            var x = StateModule.createSampleData (UserIdModule.anonymous);
            await Dto.pushRequest(x).DoAsync(changes => c.PushAsync(changes, CancellationToken.None));
            var y = StateModule.createDefault (UserIdModule.anonymous);
            var changes = await c.PullEverythingAsync(CancellationToken.None);
            var import = Dto.changesAsImport(changes);
            var z = StateModule.importChanges(import.Value, y);
            Assert.Equal(x.Items.Item.Count, z.Items.Item.Count);
            Assert.Equal(x.Categories.Item.Count, z.Categories.Item.Count);
            Assert.Equal(x.Stores.Item.Count, z.Stores.Item.Count);
            Assert.Equal(x.NotSoldItems.Item.Count, z.NotSoldItems.Item.Count);
        }

        private CosmosConnector TestConnector() => new CosmosConnector(_localEndpointUri, _localPrimaryKey, _applicationName);
    }
}

