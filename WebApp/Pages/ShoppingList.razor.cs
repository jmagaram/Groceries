using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models;
using WebApp.Common;
using static Models.CoreTypes;
using static Models.ServiceTypes;
using ItemMessage = Models.ItemModule.Message;
using SettingsMessage = Models.ShoppingListSettingsModule.Message;
using StateItemMessage = Models.StateTypes.ItemMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public partial class ShoppingList : ComponentBase, IDisposable {
        CompositeDisposable _disposables;
        string _textFilter = "";
        readonly Subject<string> _textFilterTyped = new Subject<string>();
        public bool _moveFocusToTextFilter = false;

        public bool ShowFilter { get; set; }

        public void ShowTextFilter(MouseEventArgs e) {
            ShowFilter = true;
            _moveFocusToTextFilter = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            await base.OnAfterRenderAsync(firstRender);
            if (ShowFilter && _moveFocusToTextFilter) {
                await JSRuntime.InvokeVoidAsync("interopFunctions.focusElement", filterTextInput);
                _moveFocusToTextFilter = false;
            }
        }

        public async Task HideTextFilter(MouseEventArgs e) {
            ShowFilter = false;
            await OnTextFilterClear();
        }

        private ElementReference filterTextInput;

        [Inject] IJSRuntime JSRuntime { get; set; }

        [Inject]
        public Service StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            var shoppingList =
                StateService.State
                .Select(i => i.ShoppingList(DateTimeOffset.Now))
                .Publish();

            _disposables = new CompositeDisposable
            {
                UpdateItems(shoppingList),
                UpdateStoreFilterPickerList(shoppingList),
                UpdateStoreFilterSelectedItem(shoppingList),
                UpdateTextFilter(shoppingList),
                UpdateSynchronizationStatus(),
                ProcessTextFilterTyped(),
                shoppingList.Connect()
            };
        }

        private IDisposable ProcessTextFilterTyped() =>
            _textFilterTyped
            .Skip(1)
            .DistinctUntilChanged()
            .Subscribe(async s =>
            {
                SettingsMessage settingsMessage = SettingsMessage.NewSetItemFilter(s);
                StateMessage stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
                await StateService.UpdateAsync(stateMessage);
                await InvokeAsync(() => StateHasChanged());
            });

        public SynchronizationStatus SyncStatus { get; set; } = SynchronizationStatus.NoChanges;

        public IDisposable UpdateSynchronizationStatus() =>
            StateService.SyncronizationStatus.Subscribe(i =>
            {
                SyncStatus = i;
                StateHasChanged();
            });

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

        private async Task OnNavigateToCategory(CategoryId id) {
            string categoryId = CategoryIdModule.serialize(id);
            await ClearTextFilter();
            Navigation.NavigateTo($"categoryedit/{categoryId}");
        }

        private async Task OnStoreFilterClear() =>
            await StateService.UpdateAsync(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearStoreFilter));

        private async Task OnStoreFilter(StoreId id) =>
            await StateService.UpdateAsync(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewSetStoreFilterTo(id)));

        private async Task OnClickSync() {
            if (SyncStatus.IsHasChanges) {
                await StateService.SyncIncrementalAsync();
            }
            else {
                await StateService.SyncEverythingAsync();
            }
        }

        private async Task OnClickDelete(ItemId itemId) {
            var stateItemMessage = StateItemMessage.NewDeleteItem(itemId);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickComplete(ItemId itemId) {
            var itemMessage = ItemMessage.MarkComplete;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickBuyAgain(ItemId itemId) {
            var itemMessage = ItemMessage.BuyAgain;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickRemovePostpone(ItemId itemId) {
            var itemMessage = ItemMessage.RemovePostpone;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickPostpone((ItemId itemId, int days) i) {
            var itemMessage = ItemMessage.NewPostpone(i.days);
            var stateItemMessage = StateItemMessage.NewModifyItem(i.itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickHideCompletedItems() =>
            await StateService.UpdateAsync(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewHideCompletedItems(true)));

        private async Task OnClickShowCompletedItems() =>
            await StateService.UpdateAsync(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewHideCompletedItems(false)));

        private async Task ShowPostponedWithinNext(int days) {
            await StateService.UpdateAsync(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.NewSetPostponedViewHorizon(days)));
        }

        protected void OnTextFilterChange(ChangeEventArgs e) {
            string valueTyped = (string)e.Value;
            _textFilterTyped.OnNext(valueTyped);
        }

        protected async Task OnTextFilterClear() =>
            await StateService.UpdateAsync(StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearItemFilter));

        protected async Task OnTextFilterKeyDown(KeyboardEventArgs e) {
            if (e.Key == "Escape") {
                bool shouldCancelFilter = TextFilter.Length == 0;
                await OnTextFilterClear();
                if (shouldCancelFilter) {
                    ShowFilter = false;
                }
            }
        }

        protected void OnTextFilterBlur(FocusEventArgs e) { }

        // Can probably get rid of this since I took out the Throttle ability
        // But might be needed since not yet using a TextBox, and this caused
        // problems with editing on ios 
        protected string TextFilter
        {
            get { return _textFilter; }
            set
            {
                if (value != _textFilter) {
                    _textFilter = value;
                    _textFilterTyped.OnNext(value);
                }
            }
        }

        protected async Task OnStartCreateNew() {
            Dispose();
            if (string.IsNullOrWhiteSpace(TextFilter)) {
                Navigation.NavigateTo("itemnew");
            }
            else {
                await ClearTextFilter();
                Navigation.NavigateTo($"/itemnew/{TextFilter}");
            }
        }

        private async Task ClearTextFilter() {
            SettingsMessage settingsMessage = SettingsMessage.ClearItemFilter;
            StateMessage stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        protected Guid StoreFilter { get; private set; } = Guid.Empty;

        protected List<ShoppingListModule.Item> Items { get; private set; } = new List<ShoppingListModule.Item>();

        protected List<Store> StoreFilterChoices { get; private set; } = new List<Store>();

        public void Dispose() => _disposables.Dispose();
    }
}
