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

        // inject the interface!
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
            StateService.Update(StateTypes.StateMessage.ResetToSampleData);
            await StateService.PushRequest().DoAsync(c => Cosmos.PushAsync(c));
            var changes = await Cosmos.PullEverythingAsync();
            var import = Dto.pullResponse(changes.Items, changes.Categories, changes.Stores, changes.NotSoldItems);
            StateService.Update(StateTypes.StateMessage.NewImport(import));
            ModalStatusMessage = "";
        }

        protected async Task PushAsync() {
            LogMessage($"PUSH start");
            await StateService.PushRequest().DoAsync(c => Cosmos.PushAsync(c));
            LogMessage($"PUSH done");
        }

        protected async Task PullAsync() {
            LogMessage($"PULL start");
            var state = StateService.Current;
            var changes =
                state.LastCosmosTimestamp.IsSome()
                    ? await Cosmos.PullSinceAsync(state.LastCosmosTimestamp.Value)
                    : await Cosmos.PullEverythingAsync();
            var import = Dto.pullResponse(changes.Items, changes.Categories, changes.Stores, changes.NotSoldItems);
            var msg = StateTypes.StateMessage.NewImport(import);
            StateService.Update(msg);
            LogMessage($"PULL done");
        }

        protected bool IsReady => ModalStatusMessage == "";

        protected string ModalStatusMessage { get; set; } = "";
    }
}
