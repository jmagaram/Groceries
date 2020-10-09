using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WebApp.Common;
using WebApp.Data;
using static Models.StateTypes;

namespace WebApp.Pages {
    public enum SyncStatus { SynchronizingNow, NoChanges, ShouldSync }

    public partial class ShoppingList : ComponentBase, IDisposable {
        IDisposable _updateItemList = null;
        IDisposable _updateStorePickerList = null;
        IDisposable _updateStorePickerCurrentValue = null;
        IDisposable _updateTextFilter = null;
        IDisposable _updateCanSync = null;
        IDisposable _processTextFilterTyped = null;
        string _textFilter;
        BehaviorSubject<string> _textFilterTyped = new BehaviorSubject<string>("");

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        public SyncStatus SyncStatus { get; set; } = SyncStatus.NoChanges;

        protected override void OnInitialized() {
            base.OnInitialized();
            OnTextFilterClear();
            _updateItemList = UpdateItems();
            _updateStorePickerList = UpdateStoreFilterPickerList();
            _updateStorePickerCurrentValue = UpdateStoreFilterSelectedItem();
            _updateTextFilter = UpdateTextFilter();
            _updateCanSync = UpdateCanSync();
            _processTextFilterTyped = ProcessTextFilterTyped();
        }

        private IDisposable ProcessTextFilterTyped() =>
            _textFilterTyped
            .DistinctUntilChanged()
            .Throttle(TimeSpan.FromSeconds(0.75))
            .Subscribe(s =>
            {
                StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewSetItemFilter(s)));
                InvokeAsync(()=> StateHasChanged());
            });

        private IDisposable UpdateCanSync() =>
            StateService.ShoppingList
            .Select(i => i.HasChanges)
            .DistinctUntilChanged()
            .Subscribe(async hasChanges =>
            {
                if (hasChanges) {
                    switch (SyncStatus) {
                        case SyncStatus.SynchronizingNow:
                            SyncStatus = (hasChanges == true) ? SyncStatus.ShouldSync : SyncStatus.NoChanges;
                            await InvokeAsync(() => StateHasChanged());
                            break;
                        case SyncStatus.ShouldSync:
                        case SyncStatus.NoChanges:
                            try {
                                SyncStatus = SyncStatus.SynchronizingNow;
                                await InvokeAsync(() => StateHasChanged());
                                await Task.Delay(TimeSpan.FromSeconds(0.25));
                                await OnSync();
                            }
                            catch {
                                SyncStatus = SyncStatus.ShouldSync;
                                await InvokeAsync(() => StateHasChanged());
                                await Task.Delay(TimeSpan.FromSeconds(1));
                            }
                            break;
                    }
                }
                else {
                    SyncStatus = SyncStatus.NoChanges;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    await InvokeAsync(() => StateHasChanged());
                }
            });

        private IDisposable UpdateStoreFilterSelectedItem() =>
            StateService.ShoppingList
            .Select(i => i.StoreFilter)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilter = s.IsNone() ? Guid.Empty : s.Value.StoreId.Item);

        private IDisposable UpdateTextFilter() =>
            StateService.ShoppingList
            .Select(i => i.SearchTerm)
            .DistinctUntilChanged()
            .Subscribe(s =>
            {
                TextFilter = s.IsNone() ? "" : s.Value.Item;
            });

        private IDisposable UpdateStoreFilterPickerList() =>
            StateService.ShoppingList
            .Select(i => i.Stores)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilterChoices = s.OrderBy(i => i.StoreName).ToList());

        private IDisposable UpdateItems() =>
            StateService
            .ShoppingList
            .Select(i => i.Items)
            .DistinctUntilChanged()
            .Subscribe(i => Items = i.ToList());

        [Inject]
        NavigationManager Navigation { get; set; }

        private void OnNavigateToCategory(CategoryId id) =>
            Navigation.NavigateTo($"categoryedit/{id.Item}");

        [Inject]
        public CosmosConnector Cosmos { get; set; }

        private async Task OnSync() {
            await Cosmos.CreateDatabase();
            await Cosmos.Push(StateService.Current);
            var state = StateService.Current;
            var pullResponse = await Cosmos.Pull(state.LastCosmosTimestamp.AsNullable(), state);
            var msg = StateMessage.NewImport(pullResponse);
            StateService.Update(msg);
        }

        private void OnStoreFilterClear() =>
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.ClearStoreFilter));

        private void OnStoreFilter(StoreId id) =>
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewSetStoreFilterTo(id)));

        private void OnClickDelete(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewDeleteItem(itemId)));

        private void OnClickComplete(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewMarkComplete(itemId)));

        private void OnClickBuyAgain(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewBuyAgain(itemId)));

        private void OnClickRemovePostpone(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewRemovePostpone(itemId)));

        private void OnClickPostpone((ItemId itemId, int days) i) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewPostpone(i.itemId, i.days)));

        private void OnClickHideCompletedItems() =>
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewHideCompletedItems(true)));

        private void OnClickShowCompletedItems() =>
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewHideCompletedItems(false)));

        private void ShowPostponedWithinNext(int days) {
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewSetPostponedViewHorizon(days)));
        }

        protected void OnTextFilterChange(ChangeEventArgs e) {
            string valueTyped = (string)e.Value;
            _textFilterTyped.OnNext(valueTyped);
        }

        protected void OnTextFilterClear() =>
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.ClearItemFilter));

        protected void OnTextFilterKeyDown(KeyboardEventArgs e) {
            if (e.Key == "Escape") {
                OnTextFilterClear();
            }
        }

        protected void OnTextFilterBlur(FocusEventArgs e) { }

        protected string TextFilter
        {
            get { return _textFilter; }
            set {
                _textFilter = value;
                _textFilterTyped.OnNext(value);
            }
        }

        protected Guid StoreFilter { get; private set; }

        protected List<ShoppingListModule.Item> Items { get; private set; }

        protected List<Store> StoreFilterChoices { get; private set; }

        public void Dispose() {
            _updateItemList?.Dispose();
            _updateStorePickerList?.Dispose();
            _updateStorePickerCurrentValue?.Dispose();
            _updateTextFilter?.Dispose();
            _updateCanSync?.Dispose();
            _processTextFilterTyped?.Dispose();
        }
    }
}
