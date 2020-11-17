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

        public bool ShowFilter { get; set; }

        [Inject]
        public Service StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        protected override void OnInitialized() {
            base.OnInitialized();
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
                ProcessTextFilterTyped(),
                shoppingList.Connect()
            };
        }

        protected override async Task OnInitializedAsync() {
            await StateService.InitializeAsync();
            await ClearTextFilter(force: true);
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
            Dispose();
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

        protected void OnTextFilterChange(ChangeEventArgs e) {
            string valueTyped = (string)e.Value;
            _textFilterTyped.OnNext(valueTyped);
        }

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

        protected void OnStartCreateNew() {
            Dispose();
            if (string.IsNullOrWhiteSpace(TextFilter)) {
                Navigation.NavigateTo("itemnew");
            }
            else {
                Navigation.NavigateTo($"/itemnew/{TextFilter}");
            }
        }

        protected Guid StoreFilter { get; private set; } = Guid.Empty;

        protected List<ShoppingListModule.Item> Items { get; private set; } = new List<ShoppingListModule.Item>();

        protected List<Store> StoreFilterChoices { get; private set; } = new List<Store>();

        public void Dispose() => _disposables?.Dispose();
    }
}
