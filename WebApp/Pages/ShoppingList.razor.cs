using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using WebApp.Common;
using static Models.CoreTypes;
using static Models.ServiceTypes;
using ItemMessage = Models.ItemModule.Message;
using SettingsMessage = Models.ShoppingListSettingsModule.Message;
using StateItemMessage = Models.StateTypes.ItemMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public partial class ShoppingList : ComponentBase {
        [Inject]
        public Service StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync() {
            await StateService.InitializeAsync();
            await ClearTextFilter(force: true);
        }

        private void OnNavigateToCategory(CategoryId id) {
            Navigation.NavigateTo($"/categories");
        }

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
            await StateService.UpdateAsync(transaction);
        }

        private async Task OnClickComplete(ItemId itemId) {
            var itemMessage = StateMessage.NewItemMessage(StateItemMessage.NewModifyItem(itemId, ItemMessage.MarkComplete));
            var settingsStateMessage = StateMessage.NewShoppingListSettingsMessage(SettingsMessage.ClearItemFilter);
            var transaction = StateMessage.NewTransaction(new List<StateMessage> { itemMessage, settingsStateMessage });
            ShowFilter = false;
            await StateService.UpdateAsync(transaction);
        }

        private async Task OnClickBuyAgain(ItemId itemId) {
            var itemMessage = ItemMessage.BuyAgain;
            var stateItemMessage = StateItemMessage.NewModifyItem(itemId, itemMessage);
            var stateMessage = StateMessage.NewItemMessage(stateItemMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        private async Task OnClickBuyAgainRepeat((ItemId itemId, int days) i) {
            var itemMessage = ItemMessage.BuyAgainWithRepeat.NewBuyAgainWithRepeat(i.days);
            var stateItemMessage = StateItemMessage.NewModifyItem(i.itemId, itemMessage);
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

        public bool ShowFilter { get; set; }

        protected async Task HideTextFilter() {
            ShowFilter = false;
            await ClearTextFilter();
        }

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

        protected List<ShoppingListModule.Item> Items =>
            ShoppingListView.Items.ToList();

        protected List<Store> StoreFilterChoices =>
            ShoppingListView.Stores.OrderBy(i => i.StoreName).ToList();
    }
}
