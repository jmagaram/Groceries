using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Models;
using WebApp.Common;
using WebApp.Server.Services;
using WebApp.Shared;
using static Models.ServiceTypes;

namespace WebApp.Pages {
    public partial class Sync : ComponentBase {
        [Inject]
        public Service StateService { get; set; }

        [Inject]
        public CosmosConnector Cosmos { get; set; }

        private void LogMessage(string s) => Log.Insert(0, s);

        public List<string> Log { get; } = new List<string>();

        private async Task ResetToEmpty() {
            LogMessage("Resetting database...");
            ModalStatusMessage = "Resetting database...";
            await Cosmos.DeleteDatabaseAsync();
            await Cosmos.CreateDatabaseAsync();
            ModalStatusMessage = "";
        }

        private async Task ResetToSampleData() {
            ModalStatusMessage = "Resetting to sample data...";
            LogMessage("Resetting to sample data...");
            await Cosmos.DeleteDatabaseAsync();
            await Cosmos.CreateDatabaseAsync();
            await StateService.UpdateAsync(StateTypes.StateMessage.ResetToSampleData);
            await StateService.SyncEverythingAsync();
            ModalStatusMessage = "";
        }

        protected async Task SyncIncrementalAsync() {
            LogMessage($"INCREMENTAL start");
            await StateService.SyncIncrementalAsync();
            LogMessage($"INCREMENTAL done");
        }

        protected async Task SyncEverythingAsync() {
            LogMessage($"EVERYTHING start");
            await StateService.SyncEverythingAsync();
            LogMessage($"EVERYTHING done");
        }

        protected bool IsReady => ModalStatusMessage == "";

        protected string ModalStatusMessage { get; set; } = "";
    }
}
