using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Models;
using WebApp.Common;
using WebApp.Data;

namespace WebApp.Pages {
    public partial class Sync : ComponentBase {
        [Inject]
        public Service StateService { get; set; }

        [Inject]
        public ICosmosConnector Cosmos { get; set; }

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
            StateService.Update(StateTypes.StateMessage.ResetToSampleData);
            await StateService.Push();
            await StateService.PullIncremental();
            ModalStatusMessage = "";
        }

        protected async Task PushAsync() {
            LogMessage($"PUSH start");
            await StateService.Push();
            LogMessage($"PUSH done");
        }

        protected async Task PullAsync() {
            LogMessage($"PULL start");
            await StateService.PullIncremental();
            LogMessage($"PULL done");
        }

        protected bool IsReady => ModalStatusMessage == "";

        protected string ModalStatusMessage { get; set; } = "";
    }
}
