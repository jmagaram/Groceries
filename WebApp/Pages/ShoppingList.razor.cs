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
        IDisposable _items = null;
        IDisposable _stores = null;
        IDisposable _storeOptions = null;

        private void OnClickDelete(ItemId itemId) {
            StateService.Update(StateMessage.NewDeleteItem(itemId));
        }

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        protected override void OnInitialized() {
            base.OnInitialized();
            _items =
                StateService
                .Items
                .CombineLatest(StateService.ShoppingListViewOptions, (i, j) => (Items: i, Filter: j.StoreFilter))
                .Select(i => i.Items.Where(j => i.Filter.IsNone() || j.NotSoldAt.All(k => !k.StoreId.Equals(i.Filter.Value))))
                .Subscribe(i =>
                {
                    isUpdatinglist = true;
                    var items = i.ToList();
                    Items = items;
                    isUpdatinglist = false;
                    // switches to 6 items and then immediately to 8, huh?
                    // 
                });

            _stores =
                StateService.Stores
                .Subscribe(s =>
                {
                    StoreFilterChoices = s.OrderBy(i => i.StoreName).ToList();
                });

            _storeOptions =
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
        }

        bool isUpdatinglist = false;

        private void OnDeleteStore() {
            var secondStore = StoreFilterChoices.Skip(1).First().StoreId;
            StateService.Update(StateMessage.NewDeleteStore(secondStore));
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

        protected Guid StoreFilter { get; private set; }

        protected IEnumerable<QueryTypes.ItemQry> Items { get; private set; }

        protected List<QueryTypes.StoreQry> StoreFilterChoices { get; private set; }

        public void Dispose() {
            _items?.Dispose();
            _stores?.Dispose();
            _storeOptions?.Dispose();
        }
    }
}
