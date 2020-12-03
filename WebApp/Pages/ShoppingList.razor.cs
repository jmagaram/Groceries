using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Models;
using WebApp.Common;
using WebApp.Shared;
using static Models.CoreTypes;
using static Models.ServiceTypes;
using ItemMessage = Models.ItemModule.Message;
using SettingsMessage = Models.ShoppingListSettingsModule.Message;
using StateItemMessage = Models.StateTypes.ItemMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public partial class ShoppingList : ComponentBase, IDisposable {
        ItemQuickActionDrawer _itemQuickActionDrawer;
        PostponeDrawer _postponeDrawer;
        FrequencyDrawer _frequencyDrawer;
        CategoryNavigatorDrawer _categoryNavigationDrawer;
        StoreNavigatorDrawer _storesNavigatorDrawer;
        ViewOptionsDrawer _viewOptionsDrawer;
        ItemId? _quickEditContext;
        Dictionary<CategoryId, ElementReference> _categoryReferences = new Dictionary<CategoryId, ElementReference>();

        public void Dispose() {
            _itemQuickActionDrawer?.Dispose();
            _postponeDrawer?.Dispose();
            _frequencyDrawer?.Dispose();
            _categoryNavigationDrawer?.Dispose();
            _storesNavigatorDrawer?.Dispose();
            _viewOptionsDrawer?.Dispose();
        }

        public async Task SwitchToShoppingMode() {
            var messages = new List<SettingsMessage>
            {
                SettingsMessage.ClearItemFilter,
                SettingsMessage.NewHideCompletedItems(true),
                SettingsMessage.NewSetPostponedViewHorizon(-365),
            };
            var settingsMessage = SettingsMessage.NewTransaction(messages);
            var stateMessage = StateTypes.StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        public async Task SwitchToPlanningMode(int? days) {
            var messages = new List<SettingsMessage>
            {
                SettingsMessage.ClearItemFilter,
                SettingsMessage.NewHideCompletedItems(false),
                SettingsMessage.NewSetPostponedViewHorizon(days ?? 5),
                SettingsMessage.ClearStoreFilter
            };
            var settingsMessage = SettingsMessage.NewTransaction(messages);
            var stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }


        [Inject]
        public Service StateService { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync() {
            await StateService.InitializeAsync();
            await ClearTextFilter(force: true);
        }

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
            var messages = new List<SettingsMessage>
            {
                SettingsMessage.ClearItemFilter,
                //SettingsMessage.NewHideCompletedItems(true),
                //SettingsMessage.NewSetPostponedViewHorizon(14), 
            };
            if (storeId.HasValue) {
                messages.Add(SettingsMessage.NewSetStoreFilterTo(storeId.Value));
            }
            else {
                messages.Add(SettingsMessage.ClearStoreFilter);
            }
            var settingsMessage = SettingsMessage.NewTransaction(messages);
            var stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private void OnManageStores() => Navigation.NavigateTo("/stores");

        private async Task OnMenuItemSelected(string id) {
            await Task.CompletedTask;
            if (id == "shopall") {
                await ShopAt(null);
            }
            else if (id.StartsWith("s")) {
                await ShopAt(id.Substring(1));
            }
            else if (id == "planfuture") {
                await PlanDaysAhead(365);
            }
            else if (id == "plan7") {
                await PlanDaysAhead(7);
            }
            else if (id == "plan14") {
                await PlanDaysAhead(14);
            }
            else throw new NotImplementedException();
        }

        private async Task ShopAt(string storeId) {
            var messages = new List<SettingsMessage>
            {
                SettingsMessage.ClearItemFilter,
                SettingsMessage.NewHideCompletedItems(true),
                SettingsMessage.NewSetPostponedViewHorizon(-365),
            };
            if (!string.IsNullOrWhiteSpace(storeId)) {
                messages.Add(SettingsMessage.NewSetStoreFilterTo(StoreIdModule.deserialize(storeId).Value));
            }
            else {
                messages.Add(SettingsMessage.ClearStoreFilter);
            }
            var settingsMessage = SettingsMessage.NewTransaction(messages);
            var stateMessage = StateTypes.StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task PlanDaysAhead(int days) {
            var messages = new List<SettingsMessage>
            {
                SettingsMessage.ClearItemFilter,
                SettingsMessage.NewHideCompletedItems(false),
                SettingsMessage.NewSetPostponedViewHorizon(days),
                SettingsMessage.ClearStoreFilter
            };
            var settingsMessage = SettingsMessage.NewTransaction(messages);
            var stateMessage = StateTypes.StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickDelete(ItemId itemId) {
            var itemMessage = StateMessage.NewItemMessage(StateItemMessage.NewDeleteItem(itemId));
            var settingsMessage = StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearItemFilter);
            var transaction = StateMessage.NewTransaction(new List<StateMessage> { settingsMessage, itemMessage });
            ShowFilter = false;
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(transaction);
        }

        private async Task OnClickComplete(ItemId itemId) {
            var itemMessage = StateMessage.NewItemMessage(StateItemMessage.NewModifyItem(itemId, ItemMessage.MarkComplete));
            var settingsStateMessage = StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearItemFilter);
            var transaction = StateMessage.NewTransaction(new List<StateMessage> { itemMessage, settingsStateMessage });
            ShowFilter = false;
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(transaction);
        }

        private async Task OnClickItem(ItemId itemId) {
            var configuration = ItemQuickActionsModule.create(itemId, StateService.CurrentState);
            await _itemQuickActionDrawer.Open(configuration);
        }

        private async Task OnClickRemovePostpone(ItemId itemId) {
            var itemMessage = ItemMessage.RemovePostpone;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await _itemQuickActionDrawer.Close();
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickChoosePostpone(ItemId itemId) {
            _quickEditContext = itemId;
            var viewModel = SelectZeroOrOnePostpone.createFromItemId(itemId, DateTimeOffset.Now, StateService.CurrentState);
            await _itemQuickActionDrawer.Close();
            await _postponeDrawer.Open(viewModel);
        }

        private async Task OnClickPostponeDays((ItemId itemId, int days) i) {
            var stateMessage = StateMessage.NewItemMessage(StateItemMessage.NewModifyItem(i.itemId, ItemMessage.NewPostpone(i.days)));
            await StateService.UpdateAsync(stateMessage);
            await _itemQuickActionDrawer.Close();
        }

        private async Task OnClickChooseFrequency(ItemId itemId) {
            _quickEditContext = itemId;
            var viewModel = SelectZeroOrOneFrequency.createFromItemId(itemId, StateService.CurrentState);
            await _itemQuickActionDrawer.Close();
            await _frequencyDrawer.Open(viewModel);
        }

        private async Task OnFrequencyChosen(SelectZeroOrOneModule.SelectZeroOrOne<Frequency> i) {
            var stateMessage = SelectZeroOrOneFrequency.asStateMessage(_quickEditContext.Value, i);
            await StateService.UpdateAsync(stateMessage);
            await _frequencyDrawer.Close();
        }

        private async Task OnPostponeDurationChosen(SelectZeroOrOneModule.SelectZeroOrOne<int> i) {
            var stateMessage = SelectZeroOrOnePostpone.asStateMessage(_quickEditContext.Value, i);
            await StateService.UpdateAsync(stateMessage);
            await _postponeDrawer.Close();
        }

        private async Task OnEditItem(ItemId itemId) {
            await _itemQuickActionDrawer.Close();
            Navigation.NavigateTo($"./ItemEdit/{itemId.Serialize()}");
        }

        public bool ShowFilter { get; set; }

        protected async Task HideTextFilter() {
            ShowFilter = false;
            await ClearTextFilter();
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected async Task ClearTextFilter(bool force = false) {
            if (force || !string.IsNullOrWhiteSpace(TextFilter)) {
                SettingsMessage settingsMessage = SettingsMessage.ClearItemFilter;
                StateMessage stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
                await StateService.UpdateAsync(stateMessage);
            }
        }

        protected async Task OnTextFilterKeyDown(KeyboardEventArgs e) {
            if (e.Key == "Escape") {
                bool shouldCancelFilter = TextFilter.Length == 0;
                await ClearTextFilter();
                await JSRuntime.InvokeVoidAsync("HtmlElement.setPropertyById", "searchInput", "value", "");
                if (shouldCancelFilter) {
                    ShowFilter = false;
                }
            }
        }

        private async Task OnTextFilterBlur() {
            var textBoxMessage = TextBoxMessage.LoseFocus;
            var settingsMessage = SettingsMessage.NewTextFilter(textBoxMessage);
            var stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
            await InvokeAsync(() => StateHasChanged());
        }

        private async Task OnTextFilterInput(string s) {
            var textBoxMessage = TextBoxMessage.NewTypeText(s);
            var settingsMessage = SettingsMessage.NewTextFilter(textBoxMessage);
            var stateMessage = StateMessage.NewShoppingListSettingsMessage(settingsMessage);
            await StateService.UpdateAsync(stateMessage);
            await InvokeAsync(() => StateHasChanged());
        }

        protected string TextFilter => ShoppingListView.TextFilter.ValueTyping;

        protected void OnStartCreateNew() {
            string uri = string.IsNullOrWhiteSpace(TextFilter) ? "itemnew" : $"/itemnew/{TextFilter}";
            Navigation.NavigateTo(uri);
        }

        protected Guid StoreFilter =>
            ShoppingListView.StoreFilter.IsNone() ? Guid.Empty : ShoppingListView.StoreFilter.Value.StoreId.Item;

        protected ShoppingListSettings Settings =>
            DataRow.current(StateService.CurrentState.ShoppingListSettings).Value;

        protected ShoppingListModule.ShoppingList ShoppingListView =>
            StateService.CurrentState.ShoppingList(DateTimeOffset.Now);

        protected List<Store> StoreFilterChoices =>
            ShoppingListView.Stores.OrderBy(i => i.StoreName).ToList();
    }
}
