using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using Models;

using WebApp.Common;
using WebApp.Data;

namespace WebApp.Pages {
    public partial class Sync : ComponentBase {
        [Inject]
        public Models.Service StateService { get; set; }

        [Inject]
        public CosmosConnector Cosmos { get; set; }

        private void LogMessage(string s) => Log.Insert(0, s);

        public List<string> Log { get; } = new List<string>();

        protected override async Task OnInitializedAsync() {
            ModalStatusMessage = "Initializing...";
            await Cosmos.CreateDatabase();
            ModalStatusMessage = "";
        }

        private async Task ResetToEmpty() {
            LogMessage("Resetting database...");
            ModalStatusMessage = "Resetting database...";
            await Cosmos.DeleteDatabase();
            await Cosmos.CreateDatabase();
            ModalStatusMessage = "";
        }

        private async Task ResetToSampleData() {
            ModalStatusMessage = "Resetting to sample data...";
            LogMessage("Resetting to sample data...");
            await Cosmos.DeleteDatabase();
            await Cosmos.CreateDatabase();
            StateService.Update(StateTypes.StateMessage.ResetToSampleData);
            await Cosmos.Push(StateService.Current);
            var changes = await Cosmos.Pull(null, StateService.Current);
            StateService.Update(StateTypes.StateMessage.NewImport(changes));
            ModalStatusMessage = "";
        }

        protected async Task PushAsync() {
            LogMessage($"PUSH start");
            await Cosmos.CreateDatabase();
            await Cosmos.Push(StateService.Current);
            LogMessage($"PUSH done");
        }

        protected async Task PullAsync() {
            LogMessage($"PULL start");
            var state = StateService.Current;
            var pullResponse = await Cosmos.Pull(state.LastCosmosTimestamp.AsNullable(), state);
            var msg = StateTypes.StateMessage.NewImport(pullResponse);
            StateService.Update(msg);
            LogMessage($"PULL done");
        }

        protected bool IsReady => ModalStatusMessage == "";

        protected string ModalStatusMessage { get; set; } = "";
    }
}
