using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Models;
using WebApp.Common;

#nullable enable

namespace WebApp.Services {

    // try catch errors due to network connectivity?

    public class StateService {
        private readonly ICosmosConnector _cosmos;
        private bool _hasSynchronized;

        public StateService(StateTypes.State state, ICosmosConnector cosmos) {
            _cosmos = cosmos;
            _hasSynchronized = false;
            State = state;
            SynchronizationStatus = HasChanges();
        }

        public StateService(ICosmosConnector cosmos) : this(StateModule.createDefault(UserIdModule.anonymous), cosmos) { }

        public SynchronizationStatus SynchronizationStatus { get; private set; }

        /// <summary>
        /// Raised when <see cref="State"/> or <see
        /// cref="SynchronizationStatus"/> changes.
        /// </summary>
        public event Action? OnChange;

        public StateTypes.State State { get; private set; }

        public StateTypes.State? PriorState { get; private set; }

        /// <summary>
        /// Performs and incremental synchronization but only if the service has
        /// not synchronized yet.
        /// </summary>
        public async Task Initialize() {
            if (!_hasSynchronized) {
                await SyncCoreAsync(isIncremental: true, ignoreIfSynchronizing: true);
            }
        }

        public async Task UpdateAsync(StateTypes.StateMessage message) {
            PriorState = State;
            State = StateModule.updateUsingStandardClock(message, State);
            OnChange?.Invoke();
            await SyncCoreAsync(isIncremental: true, ignoreIfSynchronizing: false);
        }

        public Task SyncEverythingAsync() => SyncCoreAsync(isIncremental: false, ignoreIfSynchronizing: true);

        public Task SyncIncrementalAsync() => SyncCoreAsync(isIncremental: true, ignoreIfSynchronizing: true);

        private async Task SyncCoreAsync(bool isIncremental, bool ignoreIfSynchronizing) {
            bool isSynchronizing = SynchronizationStatus == SynchronizationStatus.Synchronizing;
            if (!isSynchronizing || ignoreIfSynchronizing) {
                SynchronizationStatus = SynchronizationStatus.Synchronizing;
                OnChange?.Invoke();

                try {
                    var earliestChange = await PushCoreAsync();
                    await PullCoreAsync(isIncremental ? State.LastCosmosTimestamp.AsNullable() : null, earliestChange);
                    // When the changes above are applied, it is possible that
                    // foreign keys will be broken, causing additional changes that
                    // need to be pushed.
                    await PushCoreAsync();
                }
                catch (CosmosOperationCanceledException) {
                }

                SynchronizationStatus = HasChanges();
                OnChange?.Invoke();
                _hasSynchronized = true;
            }
        }

        private SynchronizationStatus HasChanges() =>
            StateModule.hasChanges(State)
            ? SynchronizationStatus.HasChanges
            : SynchronizationStatus.NoChanges;

        /// <summary>
        /// Pulls documents from Cosmos matching the timestamp filter and merges
        /// the results into the current State.
        /// </summary>
        /// <param name="after">Modification timestamp in Unix seconds</param>
        /// <param name="before">Modification timestamp in Unix seconds</param>
        private async Task PullCoreAsync(int? after, int? before) {
            var pullResponse =
                (after is null && before is null)
                ? await _cosmos.PullEverythingAsync()
                : await _cosmos.PullIncrementalAsync(after ?? int.MinValue, before);
            var import = Dto.changesAsImport(pullResponse);
            if (import.IsSome()) {
                var message = StateTypes.StateMessage.NewImport(import.Value);
                PriorState = State;
                State = StateModule.updateUsingStandardClock(message, State);
                OnChange?.Invoke();
            }
        }

        /// <summary>
        /// Pushes the current state changes, if any, to Cosmos and merges the results
        /// into the current State.
        /// </summary>
        /// <returns>The earliest timestamp of the pushed documents.</returns>
        private async Task<int?> PushCoreAsync() {
            var pushRequest = Dto.pushRequest(State);
            if (pushRequest.IsSome()) {
                var pushResponse = await _cosmos.PushAsync(pushRequest.Value);
                var import = Dto.changesAsImport(pushResponse);
                if (import.IsSome()) {
                    var message = StateTypes.StateMessage.NewImport(import.Value);
                    PriorState = State;
                    State = StateModule.updateUsingStandardClock(message, State);
                    OnChange?.Invoke();
                    return import.Value.EarliestTimestamp.AsNullable();
                }
                else {
                    return null;
                }
            }
            else {
                return null;
            }
        }
    }
}
