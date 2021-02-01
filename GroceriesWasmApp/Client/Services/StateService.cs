using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using GroceriesWasmApp.Shared;
using Microsoft.FSharp.Control;
using Models;
using static Models.DtoTypes;
using static Models.ViewTypes;

#nullable enable

namespace GroceriesWasmApp.Client.Services {

    public class StateService {
        private readonly ICosmosConnector _cosmos;
        private bool _hasSynchronized;

        public StateService(ICosmosConnector cosmos) {
            _cosmos = cosmos;
            _hasSynchronized = false;
            State = null;
            SynchronizationStatus = SynchronizationStatus.NoChanges;
        }

        public SynchronizationStatus SynchronizationStatus { get; private set; }

        /// <summary>
        /// Raised when <see cref="State"/> or <see cref="SynchronizationStatus"/> changes.
        /// </summary>
        public event Action? OnChange;

        public StateTypes.State? State { get; private set; }

        public async Task InitializeAsync(CoreTypes.FamilyId? familyId = null) {
            if (SynchronizationStatus == SynchronizationStatus.Synchronizing) {
                return;
            }
            if (State == null) {
                if (familyId == null) {
                    throw new InvalidOperationException("Can't initialize the service without an existing familyId.");
                }
                else {
                    State = StateModule.createDefault(familyId.Value, UserIdModule.anonymous);
                    _hasSynchronized = false;
                    SynchronizationStatus = HasChanges();
                    await SyncEverythingAsync();
                }
            }
            else {
                if (familyId == null || State.FamilyId.Equals(familyId.Value)) {
                    if (!_hasSynchronized) {
                        await SyncEverythingAsync();
                    }
                }
                else {
                    switch (SynchronizationStatus) {
                        case SynchronizationStatus.Synchronizing:
                            break;
                        case SynchronizationStatus.HasChanges:
                            await SyncIncrementalAsync();
                            _hasSynchronized = false;
                            await InitializeAsync(familyId);
                            break;
                        case SynchronizationStatus.NoChanges:
                            State = StateModule.createDefault(familyId.Value, UserIdModule.anonymous);
                            await SyncEverythingAsync();
                            break;
                    }
                }
            }
        }

        public async Task UpdateAsync(StateTypes.StateMessage message) {
            if (State == null) {
                throw new InvalidOperationException("Can not start synchronizing until the service is initialized.");
            }
            State = StateModule.updateUsingStandardClock(message, State);
            OnChange?.Invoke();
            if (StateModule.hasChanges(State)) {
                await SyncCoreAsync(isIncremental: true, ignoreIfSynchronizing: false);
            }
        }

        public Task SyncEverythingAsync() => 
            SyncCoreAsync(isIncremental: false, ignoreIfSynchronizing: true);

        public Task SyncIncrementalAsync() => 
            SyncCoreAsync(isIncremental: true, ignoreIfSynchronizing: true);

        public async Task<CoreTypes.Family?> UpsertFamily(CoreTypes.Family family) {
            var familyDoc = Dto.serializeFamily(family);
            var result = await _cosmos.UpsertFamily(familyDoc);
            var resultFamily = Dto.deserializeFamily(result);
            return resultFamily.IsOk ? resultFamily.ResultValue : null;
        }

        public async Task DeleteFamily(CoreTypes.FamilyId familyId) {
            await _cosmos.DeleteFamily(FamilyIdModule.serialize(familyId));
            if (State != null && State.FamilyId.Equals(familyId)) {
                State = null;
            }
        }

        public async Task<CoreTypes.Family[]> MemberOf() {
            var result = await _cosmos.MemberOf("");
            return result.Select(i => Dto.deserializeFamily(i)).Where(i => i.IsOk).Select(i => i.ResultValue).ToArray();
        }

        public async Task<CoreTypes.Family?> EditFamily(CoreTypes.Family family) {
            var familyDoc = Dto.serializeFamily(family);
            var result = await _cosmos.UpsertFamily(familyDoc);
            var resultFamily = Dto.deserializeFamily(result);
            return resultFamily.IsOk ? resultFamily.ResultValue : null;
        }

        private async Task SyncCoreAsync(bool isIncremental, bool ignoreIfSynchronizing) {
            if (State == null) {
                throw new InvalidOperationException("Can not start synchronizing until the service is initialized.");
            }
            if (SynchronizationStatus == SynchronizationStatus.Synchronizing && !ignoreIfSynchronizing) {
                return;
            }
            SynchronizationStatus = SynchronizationStatus.Synchronizing;
            OnChange?.Invoke();
            try {
                int? after = isIncremental ? State!.LastCosmosTimestamp.AsNullable() : null;
                int? before = await PushCoreAsync();
                await PullCoreAsync(after, before);
                // When the pull completes it is possible that foreign keys will
                // be broken, causing additional changes that need to be pushed.
                await PushCoreAsync();
            }
            catch (Exception) {
            }
            SynchronizationStatus = HasChanges();
            OnChange?.Invoke();
            _hasSynchronized = true;
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
                ? await _cosmos.PullEverythingAsync(FamilyIdModule.serialize(State!.FamilyId))
                : await _cosmos.PullIncrementalAsync(FamilyIdModule.serialize(State!.FamilyId), after ?? int.MinValue, before);
            var import = Dto.changesAsImport(pullResponse);
            if (import.IsSome()) {
                var message = StateTypes.StateMessage.NewImport(import.Value);
                State = StateModule.updateUsingStandardClock(message, State);
                OnChange?.Invoke();
            }
        }

        /// <summary>
        /// Pushes the current state changes, if any, to Cosmos and merges the results
        /// into the current State. The LastCosmosTimestamp set
        /// </summary>
        /// <returns>The earliest timestamp of the pushed documents.</returns>
        /// <remarks>The LastCosmosTimestamp </remarks>
        private async Task<int?> PushCoreAsync() {
            var pushRequest = Dto.pushRequest(State);
            if (pushRequest.IsSome()) {
                var pushResponse = await _cosmos.PushAsync(FamilyIdModule.serialize(State!.FamilyId), pushRequest.Value);
                var import = Dto.changesAsImport(pushResponse);
                if (import.IsSome()) {
                    var message = StateTypes.StateMessage.NewImport(import.Value);
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
