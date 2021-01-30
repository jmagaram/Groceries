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

        public async Task<Document<Family>> UpsertFamily(Document<Family> family) {
            var uri = $"api/storage/upsertfamily";
            var httpResponse = await _httpClient.PostAsJsonAsync<Document<Family>>(uri, family);
            var result = await httpResponse.Content.ReadFromJsonAsync<Document<Family>>();
            return result;
        }

        public async Task<Document<Family>[]> MemberOf(string userEmail) {
            var result = await _httpClient.GetFromJsonAsync<Document<Family>[]>($"api/storage/memberof");
            return result;
        }

        public async Task DeleteFamily(string familyId) {
            var uri = $"api/storage/deletefamily?familyId={familyId}";
            var httpResponse = await _httpClient.PostAsync(requestUri: uri, null);
        }
    }
}