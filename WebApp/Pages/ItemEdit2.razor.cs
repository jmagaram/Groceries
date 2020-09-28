using Microsoft.AspNetCore.Components;
using Models;

namespace WebApp.Pages {
    public partial class ItemEdit2 : ComponentBase {

        protected override void OnInitialized() {
            base.OnInitialized();
            Form = ItemForm.createNewItem(
                StateModule.stores.Invoke(StateService.Current),
                StateModule.categories.Invoke(StateService.Current));
        }

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        public Models.ItemForm.Form Form { get; private set; }

        protected void OnClickOk(ItemForm.ItemFormResult r) {
            var transaction = ItemForm.itemFormResultAsTransaction(r, StateService.Current);
            StateService.Update(transaction);
        }
    }
}
