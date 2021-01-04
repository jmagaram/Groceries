using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GroceryApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.FSharp.Core;
using Models;
using static Models.DtoTypes;
using static Models.ServiceTypes;

#nullable enable

namespace GroceryApp.Common
{
    public class ServerStorage : ICosmosConnector
    {
        private readonly HttpClient _httpClient;

        public ServerStorage(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public Task CreateDatabaseAsync()
        {
            return Task.CompletedTask;
        }

        public Task DeleteDatabaseAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<Changes> PullEverythingAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<Changes>("Storage/GetEverything");
#pragma warning disable CS8603 // Possible null reference return.
            return result;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Changes> PullSinceAsync(int lastSync, FSharpOption<int> earlierThan)
        {
            var queryString =
                earlierThan.AsNullable() is int e ? $"after={lastSync}&before={e}" : $"after={lastSync}";
            var result = await _httpClient.GetFromJsonAsync<Changes>($"Storage/GetIncremental?{queryString}");
#pragma warning disable CS8603 // Possible null reference return.
            return result;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public Task<Changes> PushAsync(Changes value)
        {
            return Task.FromResult(Models.Dto.emptyChanges);
        }
    }
}
