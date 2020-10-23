using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using WebApp.Common;
using WebApp.Data;
using static Models.CoreTypes;
using ItemMessage = Models.ItemModule.Message;
using SettingsMessage = Models.ShoppingListSettingsModule.Message;
using StateItemMessage = Models.StateTypes.ItemMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public enum SyncStatus { SynchronizingNow, NoChangesToPush, ShouldSync }

    public partial class ShoppingList : ComponentBase, IDisposable {
        CompositeDisposable _disposables;
        string _textFilter;
        readonly Subject<string> _textFilterTyped = new Subject<string>();

        [Inject]
        public Service StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        public SyncStatus SyncStatus { get; set; } = SyncStatus.NoChangesToPush;

        protected ISubject<object> ClickManualSync { get; set; } = new Subject<object>();

        protected override void OnInitialized() {
            base.OnInitialized();
            OnTextFilterClear();
            var shoppingList =
                StateService.ShoppingList.Publish();
            _disposables = new CompositeDisposable
            {
                UpdateItems(shoppingList),
                UpdateStoreFilterPickerList(shoppingList),
                UpdateStoreFilterSelectedItem(shoppingList),
                UpdateTextFilter(shoppingList),
                ManageAutomaticAndManualSync(shoppingList),
                ProcessTextFilterTyped(),
                shoppingList.Connect()
            };
        }

        private IDisposable ProcessTextFilterTyped() =>
            _textFilterTyped
            .DistinctUntilChanged()
            .Throttle(TimeSpan.FromSeconds(0.75))
            .Subscribe(s =>
            {
                StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewSetItemFilter(s)));
                InvokeAsync(() => StateHasChanged());
            });

        enum SyncAction {
            NoChangesNeedToBePushed,
            ChagesNeedToBePushed,
            UserRequestedManualSync
        }

        // Hack; how to avoid this re-entrancy?
        int _syncDepth = 0;

        public IDisposable ManageAutomaticAndManualSync(IObservable<ShoppingListModule.ShoppingList> shoppingList) {
            var manual = ClickManualSync.Select(_ => SyncAction.UserRequestedManualSync);
            var hasChanges = shoppingList.Select(i => i.HasChanges);
            var automatic = hasChanges.Select(i => i ? SyncAction.ChagesNeedToBePushed : SyncAction.NoChangesNeedToBePushed);
            return
                manual
                .Merge(automatic)
                .WithLatestFrom(hasChanges, (i, j) => new { Source = i, HasChanges = j })
                .Subscribe(async i =>
                {
                    _syncDepth++;
                    if (i.Source == SyncAction.ChagesNeedToBePushed || i.Source == SyncAction.UserRequestedManualSync) {
                        var before = SyncStatus;
                        if (SyncStatus != SyncStatus.SynchronizingNow) {
                            SyncStatus = SyncStatus.SynchronizingNow;
                            await InvokeAsync(() => StateHasChanged());
                        }
                        try {
                            await StateService.Push();
                            await StateService.PullIncremental(); // can cause re-entrancy
                        }
                        catch {
                        }
                        if (_syncDepth == 1) {
                            SyncStatus = i.HasChanges ? SyncStatus.ShouldSync : SyncStatus.NoChangesToPush;
                            await InvokeAsync(() => StateHasChanged());
                            await Task.Delay(TimeSpan.FromSeconds(0.5));
                        }
                    }
                    else {
                        SyncStatus = SyncStatus.NoChangesToPush;
                        await InvokeAsync(() => StateHasChanged());
                        await Task.Delay(TimeSpan.FromSeconds(0.5));
                    }
                    _syncDepth--;
                });
        }

        private IDisposable UpdateStoreFilterSelectedItem(IObservable<ShoppingListModule.ShoppingList> shoppingList) =>
            shoppingList
            .Select(i => i.StoreFilter)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilter = s.IsNone() ? Guid.Empty : s.Value.StoreId.Item);

        private IDisposable UpdateTextFilter(IObservable<ShoppingListModule.ShoppingList> shoppingList) =>
            shoppingList
            .Select(i => i.SearchTerm)
            .DistinctUntilChanged()
            .Subscribe(s =>
            {
                TextFilter = s.IsNone() ? "" : s.Value.Item;
            });

        private IDisposable UpdateStoreFilterPickerList(IObservable<ShoppingListModule.ShoppingList> shoppingList) =>
            shoppingList
            .Select(i => i.Stores)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilterChoices = s.OrderBy(i => i.StoreName).ToList());

        private IDisposable UpdateItems(IObservable<ShoppingListModule.ShoppingList> shoppingList) =>
            shoppingList
            .Select(i => i.Items)
            .DistinctUntilChanged()
            .Subscribe(i => Items = i.ToList());

        private void OnNavigateToCategory(CategoryId id) {
            string categoryId = CategoryIdModule.serialize(id);
            Navigation.NavigateTo($"categoryedit/{categoryId}");
        }

        private void OnStoreFilterClear() =>
            StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearStoreFilter));

        private void OnStoreFilter(StoreId id) =>
            StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewSetStoreFilterTo(id)));

        private void OnClickDelete(ItemId itemId) {
            var stateItemMessage = StateItemMessage.NewDeleteItem(itemId);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            StateService.Update(stateMessage);
        }

        private void OnClickComplete(ItemId itemId) {
            var itemMessage = ItemMessage.MarkComplete;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            StateService.Update(stateMessage);
        }

        private void OnClickBuyAgain(ItemId itemId) {
            var itemMessage = ItemMessage.BuyAgain;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            StateService.Update(stateMessage);
        }

        private void OnClickRemovePostpone(ItemId itemId) {
            var itemMessage = ItemMessage.RemovePostpone;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            StateService.Update(stateMessage);
        }

        private void OnClickPostpone((ItemId itemId, int days) i) {
            var itemMessage = ItemMessage.NewPostpone(i.days);
            var stateItemMessage = StateItemMessage.NewModifyItem(i.itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            StateService.Update(stateMessage);
        }

        private void OnClickHideCompletedItems() =>
            StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewHideCompletedItems(true)));

        private void OnClickShowCompletedItems() =>
            StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewHideCompletedItems(false)));

        private void ShowPostponedWithinNext(int days) {
            StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewSetPostponedViewHorizon(days)));
        }

        protected void OnTextFilterChange(ChangeEventArgs e) {
            string valueTyped = (string)e.Value;
            _textFilterTyped.OnNext(valueTyped);
        }

        protected void OnTextFilterClear() =>
            StateService.Update(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearItemFilter));

        protected void OnTextFilterKeyDown(KeyboardEventArgs e) {
            if (e.Key == "Escape") {
                OnTextFilterClear();
            }
        }

        protected void OnTextFilterBlur(FocusEventArgs e) { }

        protected string TextFilter
        {
            get { return _textFilter; }
            set
            {
                _textFilter = value;
                _textFilterTyped.OnNext(value);
            }
        }

        protected Guid StoreFilter { get; private set; }

        protected List<ShoppingListModule.Item> Items { get; private set; }

        protected List<Store> StoreFilterChoices { get; private set; }

        public void Dispose() => _disposables.Dispose();
    }
}
