using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FSharp.Collections;
using Microsoft.JSInterop;
using Models;
using WebApp.Common;
using WebApp.Services;
using WebApp.Shared;
using static Models.CoreTypes;
using static Models.ViewTypes;
using ItemMessage = Models.ItemModule.Message;
using ShoppingListSettingsMessage = Models.ShoppingListSettingsModule.Message;
using StateItemMessage = Models.StateTypes.ItemMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public partial class ShoppingList : ComponentBase, IDisposable {
#pragma warning disable IDE0044 // Add readonly modifier
        ItemQuickActionDrawer _itemQuickActionDrawer;
        PostponeDrawer _postponeDrawer;
        StoresDrawer _storesDrawer;
        CategoryNavigatorDrawer _categoryNavigationDrawer;
        StoreNavigatorDrawer _storesNavigatorDrawer;
        ViewOptionsDrawer _viewOptionsDrawer;
#pragma warning restore IDE0044 // Add readonly modifier
        ItemId? _quickEditContext;
        IDisposable _disposables;
        readonly Dictionary<CategoryId, ElementReference> _categoryReferences = new();

        protected override async Task OnInitializedAsync() {
            await StateService.InitializeAsync();
            await EndSearch();
        }

        protected override void OnInitialized() {
            var state = Observable
                .FromEvent(h => StateService.OnChange += h, h => StateService.OnChange -= h)
                .Publish();
            _disposables = new CompositeDisposable {
                state
                    .Select(_ => StateService.State)
                    .DistinctUntilChanged()
                    .Buffer(2, 1)
                    .Where(i => i.Count == 2)
                    .Select(i => ShoppingListModule.postponeChangedCore(i[0], i[1]))
                    .Subscribe(i=>PostponeUntilChanged=i),
                state.Subscribe(_=>StateHasChanged()),
                state.Connect()
            };
        }

        protected override void OnAfterRender(bool firstRender) {
            base.OnAfterRender(firstRender);
            PostponeUntilChanged = null;
        }

        protected ShoppingListModule.ShoppingList ShoppingListView =>
            CurrentState.ShoppingList(DateTimeOffset.Now);

        public void Dispose() => _disposables?.Dispose();

        public bool IsSearchBarVisible => CurrentState.LoggedInUserSettings().ShoppingListSettings.IsTextFilterVisible;

        public string SearchBarClass => IsSearchBarVisible ? "search-bar active" : "search-bar";

        public async Task EndSearch() {
            if (IsSearchBarVisible) {
                var msg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(ShoppingListSettingsMessage.EndSearch));
                await StateService.UpdateAsync(msg);
            }
        }

        public async Task StartSearch() {
            if (!IsSearchBarVisible) {
                await JSRuntime.InvokeVoidAsync("HtmlElement.setPropertyById", "searchInput", "value", "");
                var msg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(ShoppingListSettingsMessage.StartSearch));
                await StateService.UpdateAsync(msg);
            }
        }

        public async Task SwitchToShoppingMode() {
            var messages = new List<ShoppingListSettingsMessage>
            {
                ShoppingListSettingsMessage.EndSearch,
                ShoppingListSettingsMessage.HidePostponedItems
            };
            var shoppingListMessage = ShoppingListSettingsMessage.NewTransaction(messages);
            var userSettingsMessage = UserSettingsModule.Message.NewShoppingListSettingsMessage(shoppingListMessage);
            var stateMessage = StateTypes.StateMessage.NewUserSettingsMessage(userSettingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        public async Task SwitchToPlanningMode(int? days) {
            var messages = new List<ShoppingListSettingsMessage>
            {
                ShoppingListSettingsMessage.ClearItemFilter,
                ShoppingListSettingsMessage.NewSetPostponedViewHorizon(days ?? 5),
                ShoppingListSettingsMessage.ClearStoreFilter
            };
            var shoppingListMessage = ShoppingListSettingsMessage.NewTransaction(messages);
            var userSettingsMessage = UserSettingsModule.Message.NewShoppingListSettingsMessage(shoppingListMessage);
            var stateMessage = StateMessage.NewUserSettingsMessage(userSettingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        public async Task UseFontSize(FontSize f) {
            var settingsMessage = UserSettingsModule.Message.NewSetFontSize(f);
            var stateMessage = StateMessage.NewUserSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
            Navigation.NavigateTo("/shoppinglist");
        }

        [Inject]
        public StateService StateService { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        protected StateTypes.State CurrentState => StateService.State;

        protected FSharpSet<ItemId> PostponeUntilChanged { get; set; }

        private async Task OnClickCategoryHeader() {
            await _categoryNavigationDrawer.Open(ShoppingListView.Categories);
        }

        private async Task OnNavigateToCategory(CategoryId? id) {
            ElementReference categoryHeader = _categoryReferences[id.GetValueOrDefault()];
            // Occasionally fails; put a delay into scrollIntoView to see if it would fix the issue
            await JS.InvokeVoidAsync("HtmlElement.scrollIntoView", categoryHeader, 70);
        }

        private void OnManageCategories() => Navigation.NavigateTo($"/categories");

        private async Task OpenStoreNavigator() =>
            await _storesNavigatorDrawer.Open(ShoppingListView.Stores, ShoppingListView.StoreFilter.AsEnumerable().FirstOrDefault());

        private async Task OnChooseStore(StoreId? storeId) {
            var messages = new List<ShoppingListSettingsMessage>
            {
                ShoppingListSettingsMessage.ClearItemFilter,
                //SettingsMessage.NewHideCompletedItems(true),
                //SettingsMessage.NewSetPostponedViewHorizon(14), 
            };
            if (storeId.HasValue) {
                messages.Add(ShoppingListSettingsMessage.NewSetStoreFilterTo(storeId.Value));
            }
            else {
                messages.Add(ShoppingListSettingsMessage.ClearStoreFilter);
            }
            var shoppingListMessage = ShoppingListSettingsMessage.NewTransaction(messages);
            var userSettingsMessage = UserSettingsModule.Message.NewShoppingListSettingsMessage(shoppingListMessage);
            var stateMessage = StateMessage.NewUserSettingsMessage(userSettingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private void OnManageStores() => Navigation.NavigateTo("/stores");

        private async Task OnClickDelete(ItemId itemId) {
            var itemMsg = StateMessage.NewItemMessage(StateItemMessage.NewDeleteItem(itemId));
            var userSettingsMsg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(ShoppingListSettingsMessage.EndSearch));
            var transaction = StateMessage.NewTransaction(new List<StateMessage> { userSettingsMsg, itemMsg });
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(transaction);
        }

        private async Task OnClickComplete(ItemId itemId) {
            await _itemQuickActionDrawer.Close();
            var recordPurchase = StateMessage.NewRecordPurchase(itemId);
            var userSettingsMsg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(ShoppingListSettingsMessage.EndSearch));
            var transaction = StateMessage.NewTransaction(new List<StateMessage> { recordPurchase, userSettingsMsg });
            await StateService.UpdateAsync(transaction);
        }

        private async Task OnClickItem(ItemId itemId) {
            var storeFilter = CurrentState.LoggedInUserSettings().ShoppingListSettings.StoreFilter;
            var configuration =
                IsSearchBarVisible || storeFilter.IsNone()
                ? ItemQuickActionsViewModule.createNoActiveStore(itemId, CurrentState)
                : ItemQuickActionsViewModule.createWithActiveStore(itemId, storeFilter.Value, CurrentState);
            await _itemQuickActionDrawer.Open(configuration);
        }

        private async Task OnClickRemovePostpone(ItemId itemId) {
            var itemMsg = ItemMessage.RemovePostpone;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMsg);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickChoosePostpone(ItemId itemId) {
            _quickEditContext = itemId;
            string itemName = StateModule.tryFindItem(itemId, StateService.State).Value.ItemName.AsText();
            await _itemQuickActionDrawer.Close();
            bool isPostponed = StateModule.tryFindItem(itemId, CurrentState).Value.PostponeUntil.IsSome();
            await _postponeDrawer.Open(ItemModule.commonPostponeChoices, isPostponed, itemName);
        }

        private async Task OnClickAddToShoppingList(ItemId itemId) {
            var msg = StateMessage.NewItemMessage(StateItemMessage.NewModifyItem(itemId, ItemMessage.RemovePostpone));
            await StateService.UpdateAsync(msg);
        }

        private async Task OnClickPostponeDays((ItemId itemId, int days) i) {
            var stateItemMsg = StateItemMessage.NewModifyItem(i.itemId, ItemMessage.NewPostpone(i.days));
            var stateMsg = StateMessage.NewItemMessage(stateItemMsg);
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(stateMsg);
        }

        private async Task OnPostponeDurationChosen(int? d) {
            var itemMsg = (d is int days) ? ItemMessage.NewPostpone(days) : ItemMessage.RemovePostpone;
            var stateItemMsg = StateItemMessage.NewModifyItem(_quickEditContext.Value, itemMsg);
            var stateMsg = StateMessage.NewItemMessage(stateItemMsg);
            await _postponeDrawer.Close();
            await StateService.UpdateAsync(stateMsg);
        }

        private async Task OnSpecificStoresSelected(SelectMany<Store> f) {
            if (SelectManyModule.hasChanges(f)) {
                var stateMsg = StateMessage.NewItemOnlySoldAt(_quickEditContext.Value, f.Selected.Select(i => i.StoreId));
                await StateService.UpdateAsync(stateMsg);
            }
        }

        private async ValueTask OnCustomizeStores(ItemId itemId) {
            _quickEditContext = itemId;
            await _itemQuickActionDrawer.Close();
            var (itemName, model) = SelectManyStores.create(itemId, CurrentState);
            await _storesDrawer.Open(model, itemName.AsText());
        }

        private async Task OnNotSoldAtSpecificStore((ItemId itemId, StoreId storeId) i) {
            var stateMsg = StateMessage.NewItemNotSoldAt(i.itemId, i.storeId);
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(stateMsg);
        }

        private async Task OnEditItem(ItemId itemId) {
            await _itemQuickActionDrawer.Close();
            Navigation.NavigateTo($"./ItemEdit/{itemId.Serialize()}");
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected async Task OnTextFilterKeyDown(KeyboardEventArgs e) {
            if (IsSearchBarVisible) {
                if (e.Key == "Escape") {
                    if (!string.IsNullOrWhiteSpace(TextFilter)) {
                        await JSRuntime.InvokeVoidAsync("HtmlElement.setPropertyById", "searchInput", "value", "");
                        var msg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(ShoppingListSettingsMessage.ClearItemFilter));
                        await StateService.UpdateAsync(msg);
                    }
                    else {
                        await EndSearch();
                    }
                }
                else if (e.Key == "Return" || e.Key == "Enter") {
                    OnStartCreateNew();
                }
            }
        }

        private async Task OnTextFilterBlur() {
            if (IsSearchBarVisible) {
                var textBoxMsg = TextBoxMessage.LoseFocus;
                var settingsMsg = ShoppingListSettingsMessage.NewTextFilter(textBoxMsg);
                var stateMsg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(settingsMsg));
                await StateService.UpdateAsync(stateMsg);
                await InvokeAsync(() => StateHasChanged());
            }
        }

        private async Task OnTextFilterInput(string s) {
            if (IsSearchBarVisible) {
                var textBoxMsg = TextBoxMessage.NewTypeText(s);
                var settingsMsg = ShoppingListSettingsMessage.NewTextFilter(textBoxMsg);
                var stateMsg = StateMessage.NewUserSettingsMessage(UserSettingsModule.Message.NewShoppingListSettingsMessage(settingsMsg));
                await StateService.UpdateAsync(stateMsg);
                await InvokeAsync(() => StateHasChanged());
            }
        }

        protected string TextFilter => ShoppingListView.TextFilter.ValueTyping;

        protected void OnStartCreateNew() {
            string uri = string.IsNullOrWhiteSpace(TextFilter) ? "itemnew" : $"/itemnew/{TextFilter}";
            Navigation.NavigateTo(uri);
        }

        protected Guid StoreFilter =>
            ShoppingListView.StoreFilter.IsNone() ? Guid.Empty : ShoppingListView.StoreFilter.Value.StoreId.Item;

        protected List<Store> StoreFilterChoices =>
            ShoppingListView.Stores.OrderBy(i => i.StoreName).ToList();
    }
}
