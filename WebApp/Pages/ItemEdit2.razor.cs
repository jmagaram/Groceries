using Microsoft.AspNetCore.Components;
using Models;
using System;

namespace WebApp.Pages {
    public partial class ItemEdit2 : ComponentBase {
        protected override void OnInitialized() {
            base.OnInitialized();
            var state = StateService.Current;
            if (Id.HasValue) {
                Form = ItemForm.editItemFromGuid(Id.Value, StateService.Clock, state);
            }
            else {
                Form = ItemForm.createNewItem(
                    ItemName ?? "",
                    StateModule.stores.Invoke(state),
                    StateModule.categories.Invoke(state));
            }
        }

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Parameter]
        public string ItemName { get; set; } = "";

        [Parameter]
        public Guid? Id { get; set; }

        public Models.ItemForm.Form Form { get; private set; }

        protected void OnClickOk(ItemForm.ItemFormResult r) {
            var transaction = ItemForm.itemFormResultAsTransaction(r, StateService.Current);
            StateService.Update(transaction);
            Navigation.NavigateTo("shoppinglist");
        }

        protected void OnCancel() => Navigation.NavigateTo("shoppinglist");
    }
}
