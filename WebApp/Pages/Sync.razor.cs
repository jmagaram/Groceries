using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Common;

namespace WebApp.Pages {
    public partial class Sync : ComponentBase, IDisposable {
        private CosmosConnector _cosmos = null;

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        private void LogMessage(string s) => Log.Insert(0, s);

        public List<string> Log { get; } = new List<string>();

        protected override async Task OnInitializedAsync() {
            ModalStatusMessage = "Initializing...";
            _cosmos = CreateConnector();
            await _cosmos.CreateDatabase();
            ModalStatusMessage = "";
        }

        private async Task ResetToEmpty() {
            LogMessage("Resetting database...");
            ModalStatusMessage = "Resetting database...";
            await _cosmos.DeleteDatabase();
            await _cosmos.CreateDatabase();
            ModalStatusMessage = "";
        }

        private static CosmosConnector CreateConnector() {
            string localEndpointUri = "https://localhost:8081";
            string localPrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            string applicationName = "CosmosDBDotnetQuickstart";
            return new CosmosConnector(localEndpointUri, localPrimaryKey, applicationName);
        }

        protected async Task PushAsync() {
            LogMessage($"PUSH start");
            await _cosmos.CreateDatabase();
            await _cosmos.Push(StateService.Current);
            LogMessage($"PUSH done");
        }

        protected async Task PullAsync() {
            LogMessage($"PULL start");
            var (state, ts) = await _cosmos.Pull(StateService.LastCosmosSyncTimestamp, StateService.Current);
            StateService.ReplaceState(state);
            if (ts.HasValue) {
                StateService.LastCosmosSyncTimestamp = ts.Value;
            }
            LogMessage($"PULL done");
        }

        protected bool IsReady => ModalStatusMessage == "";

        protected string ModalStatusMessage { get; set; } = "";

        public void Dispose() => _cosmos.Dispose();
    }
}
