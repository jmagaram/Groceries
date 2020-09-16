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

        private void OnClickDelete(ItemId itemId) {
            StateService.Update(StateMessage.NewDeleteItem(itemId));
        }

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        protected override void OnInitialized() {
            base.OnInitialized();
            _updateItemList = UpdateItems();
            _updateStorePickerList = UpdateStoreList();
            _updateStorePickerCurrentValue = UpdateViewOptions();
        }

        private IDisposable UpdateViewOptions() =>
            StateService.ShoppingListViewOptions
            .Subscribe(s =>
            {
                if (s.StoreFilter.IsNone()) {
                    StoreFilter = Guid.Empty;
                }
                else {
                    StoreFilter = s.StoreFilter.Value.Item;
                }
            });

        private IDisposable UpdateStoreList() =>
            StateService.Stores
            .Subscribe(s =>
            {
                StoreFilterChoices = s.OrderBy(i => i.StoreName).ToList();
            });

        // worried about combining observables; one will change before the other
        // somewhat arbitrarily, depending on how i set them up. the data
        // between them may not be in sync. consider just having one observable
        // per view and define on server.
        private IDisposable UpdateItems() =>
            StateService
            .Items
            .CombineLatest(StateService.ShoppingListViewOptions, (i, j) => (Items: i, Filter: j.StoreFilter))
            .Select(i => i.Items.Where(j => i.Filter.IsNone() || j.NotSoldAt.All(k => !k.StoreId.Equals(i.Filter.Value))))
            .Subscribe(i =>
            {
                var items = i.ToList();
                Items = items;
            });

        private void OnDeleteStore() {
            //var toDelete = StoreFilterChoices.Skip(1).First().StoreId;
            var toDelete = StoreFilterChoices.First(i => i.StoreId.Item.Equals(StoreFilter)).StoreId;
            StateService.Update(StateMessage.NewDeleteStore(toDelete));
        }

        private void OnStoreFilterChange(ChangeEventArgs e) {
            Guid.TryParse((string)e.Value, out Guid selectedStore);
            var store = StoreFilterChoices.FirstOrDefault(i => i.StoreId.Item == selectedStore);
            if (store == null) {
                StateService.Update(StateMessage.NewShoppingListMessage(ShoppingListMessage.ClearStoreFilter));
            }
            else {
                StateService.Update(StateMessage.NewShoppingListMessage(ShoppingListMessage.NewSetStoreFilterTo(store.StoreId)));
            }
        }

        private Guid storeFilter;

        protected Guid StoreFilter
        {
            get => storeFilter; private set
            {
                storeFilter = value;
            }
        }

        protected IEnumerable<QueryTypes.ItemQry> Items { get; private set; }

        protected List<QueryTypes.StoreQry> StoreFilterChoices { get; private set; }

        public void Dispose() {
            _updateItemList?.Dispose();
            _updateStorePickerList?.Dispose();
            _updateStorePickerCurrentValue?.Dispose();
        }
    }
}
