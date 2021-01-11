using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GroceriesWasmApp.Shared;
using static Models.DtoTypes;

namespace GroceriesWasmApp.Client.Services {
    public class ServerStorage : ICosmosConnector {
        private readonly HttpClient _httpClient;

        public ServerStorage(HttpClient httpClient) {
            this._httpClient = httpClient;
        }

        public Task CreateDatabaseAsync() {
            return Task.CompletedTask;
        }

        public Task DeleteDatabaseAsync() {
            return Task.CompletedTask;
        }

        public async Task<Changes> PullEverythingAsync(string familyId) {
            var result = await _httpClient.GetFromJsonAsync<Changes>($"api/storage/geteverything?familyId={familyId}");
            return result;
        }

        public async Task<Changes> PullIncrementalAsync(string familyId, int after, int? before) {
            var queryString =
                before is int e ? $"familyId={familyId}&after={after}&before={e}" : $"familyId={familyId}&after={after}";
            var result = await _httpClient.GetFromJsonAsync<Changes>($"api/storage/getincremental?{queryString}");
            return result;
        }

        public async Task<Changes> PushAsync(string familyId, Changes value) {
            var uri = $"api/storage/push?familyId={familyId}";
            var httpResponse = await _httpClient.PostAsJsonAsync<Changes>(uri, value);
            var result = await httpResponse.Content.ReadFromJsonAsync<Changes>();
            return result;
        }
    }
}