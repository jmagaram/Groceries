using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using WebApp.Common;
using static Models.StateTypes;

namespace WebApp.Pages {
    public partial class ShoppingList : ComponentBase {
        IDisposable _updateItemList = null;
        IDisposable _updateStorePickerList = null;
        IDisposable _updateStorePickerCurrentValue = null;
        IDisposable _updateTextFilter = null;

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        protected override void OnInitialized() {
            base.OnInitialized();
            _updateItemList = UpdateItems();
            _updateStorePickerList = UpdateStoreFilterPickerList();
            _updateStorePickerCurrentValue = UpdateStoreFilterSelectedItem();
            _updateTextFilter = UpdateTextFilter();
        }

        private IDisposable UpdateStoreFilterSelectedItem() =>
            StateService.ShoppingList
            .Select(i => i.StoreFilter)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilter = s.IsNone() ? Guid.Empty : s.Value.StoreId.Item);

        private IDisposable UpdateTextFilter() =>
            StateService.ShoppingList
            .Select(i => i.SearchTerm)
            .DistinctUntilChanged()
            .Subscribe(s => TextFilter = s.IsNone() ? "" : s.Value.Item);

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

        protected void OnTextFilterChange(ChangeEventArgs e) =>
            StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewSetItemFilter((string)e.Value)));

        protected void OnTextFilterBlue(FocusEventArgs e) { }

        protected string TextFilter { get; private set; }

        protected Guid StoreFilter { get; private set; }

        protected IEnumerable<ShoppingListModule.Item> Items { get; private set; }

        protected List<Store> StoreFilterChoices { get; private set; }

        public void Dispose() {
            _updateItemList?.Dispose();
            _updateStorePickerList?.Dispose();
            _updateStorePickerCurrentValue?.Dispose();
            _updateTextFilter?.Dispose();
        }
    }
}
