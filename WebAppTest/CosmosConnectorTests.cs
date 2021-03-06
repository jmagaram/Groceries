using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;
using WebApp.Common;
using WebApp.Services;
using Xunit;

namespace WebAppTest {
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "CosmosDb requires a case-sensitive property name.")]
    public record Document(string id, string Title, string _eTag, string CustomerId = "justin") {
        public static Document Random() => new($"ID {Guid.NewGuid()}", $"TITLE {Guid.NewGuid()}", "");
        public Document ClearEtag() => this with { _eTag = "" };
        public Document WithRandomEtag() => this with { _eTag = Guid.NewGuid().ToString() };
        public bool HasAnEtag => !string.IsNullOrWhiteSpace(this._eTag);
        public CosmosDocumentProperties Properties => new(Id: id, Etag: _eTag, PartitionKey: CustomerId);
    }

    public class CosmosConnectorTests {
        private static readonly string _localEndpointUri = "https://localhost:8081";
        private static readonly string _localPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string _databaseId = "unitTestDb";
        private const string _familyId = "testfamily";

        [Fact]
        public async Task CanCreateDatabase() {
            using var c = TestConnector();
            await c.CreateDatabaseAsync();
        }

        [Fact]
        public async Task PushCoreAsync_WhenInsert_ReturnWithEtag() {
            using var c = await CreateTargetAsync();
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var doc = Document.Random();
            var result = await c.PushCoreAsync(doc, i => i.Properties, source.Token);
            Assert.Equal(result.UpdatedDocument().ClearEtag(), doc.ClearEtag());
            Assert.True(result.UpdatedDocument().HasAnEtag);
        }

        [Fact]
        public async Task PushCoreAsync_WhenUpdate_ReturnWithEtag() {
            using var c = await CreateTargetAsync();
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var doc1 = Document.Random();
            var doc2 = (await c.PushCoreAsync(doc1, i => i.Properties, source.Token)).UpdatedDocument();
            var doc3 = doc2 with { Title = "banana" };
            var doc4 = (await c.PushCoreAsync(doc3, i => i.Properties, source.Token)).UpdatedDocument();
            Assert.Equal(doc1 with { Title = "banana" }, doc4.ClearEtag());
            Assert.True(doc4.HasAnEtag);
            Assert.NotEqual(doc3._eTag, doc4._eTag);
        }

        [Fact]
        public async Task PushCoreAsync_WhenEtagMismatch_ReturnConcurrencyConflict() {
            using var c = await CreateTargetAsync();
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var doc1 = Document.Random();
            var doc2 = (await c.PushCoreAsync(doc1, i => i.Properties, source.Token)).UpdatedDocument();
            var doc3 = doc2.WithRandomEtag() with { Title = "some new title" };
            var doc4 = await c.PushCoreAsync(doc3, i => i.Properties, source.Token);
            Assert.Equal((doc4 as ConcurrencyConflict<Document>).PushFailure, doc3);
        }

        private static void AssertSameDocuments(IEnumerable<Document> a, IEnumerable<Document> b) {
            var aSet = a.ToImmutableHashSet();
            var bSet = b.ToImmutableHashSet();
            Assert.True(aSet.SetEquals(bSet));
        }

        [Fact]
        public async Task PushBulkCoreAsync_ReturnEachWithUniqueEtag() {
            using var c = await CreateTargetAsync();
            const int itemCount = 200;
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var docs1 = Enumerable.Range(1, itemCount).Select(i => Document.Random()).ToArray();
            var docs2 = await c.PushBulkCoreAsync(docs1, i => i.Properties, source.Token);
            AssertSameDocuments(docs1, docs2.Select(i => i.UpdatedDocument().ClearEtag()));
            Assert.True(docs2.All(i => i.UpdatedDocument().HasAnEtag));
        }

        [Fact]
        public async Task PushBulkCoreAsync_WhenSomeConflicts_ReturnSuccessesAndFailures() {
            using var c = await CreateTargetAsync();
            const int itemCount = 1000;
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var docs1 = Enumerable.Range(1, itemCount).Select(i => Document.Random()).ToImmutableArray();
            var docs2 = (await c.PushBulkCoreAsync(docs1, i => i.Properties, source.Token)).Select(i => i.UpdatedDocument()).ToImmutableArray();
            var modifiedA = docs2[3].WithRandomEtag() with { Title = "banana" }; // what if no etag? 
            var modifiedB = docs2[750].WithRandomEtag() with { Title = "zebra" };
            var docs3 = docs2.RemoveAt(750).RemoveAt(3).Insert(0, modifiedA).Add(modifiedB);
            var docs4 = await c.PushBulkCoreAsync(docs3, i => i.Properties, source.Token);
            Assert.Equal(2, docs4.Count(i => i is ConcurrencyConflict<Document>));
            Assert.Equal(itemCount - 2, docs4.Count(i => i is not ConcurrencyConflict<Document>));
        }

        [Fact]
        public async Task PushBulkCore_IsFasterThanLoop() {
            using CancellationTokenSource source = new();
            const int itemCount = 1000;
            var docs = Enumerable.Range(1, itemCount).Select(i => Document.Random()).ToImmutableArray();
            CosmosConnector c = null;

            try {
                c = await CreateTargetAsync();
                Stopwatch bulk = Stopwatch.StartNew();
                await c.PushBulkCoreAsync(docs, i => i.Properties, source.Token);
                bulk.Stop();

                c = await CreateTargetAsync();
                Stopwatch oneAtATime = Stopwatch.StartNew();
                foreach (var d in docs) {
                    await c.PushCoreAsync(d, i => i.Properties, source.Token);
                }
                oneAtATime.Stop();

                var ratio = Convert.ToDouble(bulk.ElapsedMilliseconds) / Convert.ToDouble(oneAtATime.ElapsedMilliseconds);
                Assert.True(ratio < 0.5);
            }
            finally {
                c?.Dispose();
            }
        }

        [Fact]
        public async Task CanPushThenPullSameNumberOfItems() {
            // Would be easier to test using F# since could compare the actual State variables
            // with the eTag values removed
            using var c = await CreateTargetAsync();
            var stateA = StateModule.createSampleData(FamilyIdModule.anonymous, UserIdModule.anonymous);
            await Dto.pushRequest(stateA).DoAsync(changes => c.PushAsync(_familyId, changes));
            var pulledChanges = await c.PullEverythingAsync(_familyId);
            var pulledChangesAsImport = Dto.changesAsImport(pulledChanges);
            var stateB = StateModule.importChanges(pulledChangesAsImport.Value, stateA);
            Assert.Equal(stateA.Items.Item.Count, stateB.Items.Item.Count);
            Assert.Equal(stateA.Categories.Item.Count, stateB.Categories.Item.Count);
            Assert.Equal(stateA.Stores.Item.Count, stateB.Stores.Item.Count);
            Assert.Equal(stateA.NotSoldItems.Item.Count, stateB.NotSoldItems.Item.Count);
            Assert.Equal(stateA.Purchases.Item.Count, stateB.Purchases.Item.Count);
        }

        private static CosmosConnector TestConnector() => new(_localEndpointUri, _localPrimaryKey, _databaseId);

        private async Task<CosmosConnector> CreateTargetAsync() {
            var c = TestConnector();
            await c.DeleteDatabaseAsync();
            await c.CreateDatabaseAsync();
            return c;
        }
    }
}

