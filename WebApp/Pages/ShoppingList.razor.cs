using Microsoft.AspNetCore.Components;
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

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        protected override void OnInitialized() {
            base.OnInitialized();
            _updateItemList = UpdateItems();
            _updateStorePickerList = UpdateStoreFilterPickerList();
            _updateStorePickerCurrentValue = UpdateStoreFilterSelectedItem();
        }

        private IDisposable UpdateStoreFilterSelectedItem() =>
            StateService.ShoppingListQry
            .Select(i => i.StoreFilter)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilter = s.IsNone() ? Guid.Empty : s.Value.StoreId.Item);

        private IDisposable UpdateStoreFilterPickerList() =>
            StateService.ShoppingListQry
            .Select(i => i.Stores)
            .DistinctUntilChanged()
            .Subscribe(s => StoreFilterChoices = s.OrderBy(i => i.StoreName).ToList());

        private IDisposable UpdateItems() =>
            StateService
            .ShoppingListQry
            .Select(i => i.Items)
            .DistinctUntilChanged()
            .Subscribe(i => Items = i.ToList());

        private void OnStoreFilterChange(ChangeEventArgs e) {
            Guid.TryParse((string)e.Value, out Guid selectedStore);
            var store = StoreFilterChoices.FirstOrDefault(i => i.StoreId.Item == selectedStore);
            if (store == null) {
                StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.ClearStoreFilter));
            }
            else {
                StateService.Update(StateMessage.NewSettingsMessage(SettingsMessage.NewSetStoreFilterTo(store.StoreId)));
            }
        }

        private void OnClickDelete(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewDeleteItem(itemId)));

        private void OnClickComplete(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewMarkComplete(itemId)));

        private void OnClickBuyAgain(ItemId itemId) =>
            StateService.Update(StateMessage.NewItemMessage(ItemMessage.NewBuyAgain(itemId)));

        protected Guid StoreFilter { get; private set; }

        protected IEnumerable<QueryTypes.ItemQry> Items { get; private set; }

        protected List<Store> StoreFilterChoices { get; private set; }

        public void Dispose() {
            _updateItemList?.Dispose();
            _updateStorePickerList?.Dispose();
            _updateStorePickerCurrentValue?.Dispose();
        }
    }
}
