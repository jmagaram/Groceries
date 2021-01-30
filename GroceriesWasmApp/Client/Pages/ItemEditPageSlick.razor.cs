using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Models;
using GroceriesWasmApp.Client.Common;
using GroceriesWasmApp.Client.Services;
using FormMessage = Models.ItemFormModule.Message;
using PageMessage = Models.StateTypes.ItemEditPageMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace GroceriesWasmApp.Client.Pages {
    public partial class ItemEditPageSlick : ComponentBase {

        protected override async Task OnInitializedAsync() {
            await StateService.InitializeAsync();
            if (!string.IsNullOrWhiteSpace(Id)) {
                var pageMessage = PageMessage.NewBeginEditItem(Id);
                var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
                await StateService.UpdateAsync(stateMessage);
            }
            else {
                var pageMessage =
                    string.IsNullOrWhiteSpace(ItemName)
                    ? PageMessage.BeginCreateNewItem
                    : PageMessage.NewBeginCreateNewItemWithName(ItemName);
                var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
                await StateService.UpdateAsync(stateMessage);
            }
        }

        [Inject]
        public StateService StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Parameter]
        public string ItemName { get; set; }

        [Parameter]
        public string Id { get; set; }

        public CoreTypes.ItemForm Form => StateService.State.ItemEditPage?.Value;

        protected async Task OnFormMessage(FormMessage message) {
            var pageMessage = PageMessage.NewItemEditFormMessage(message);
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        protected async Task OnClickOk() {
            var pageMessage = PageMessage.SubmitItemEditForm;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            Navigation.NavigateTo("shoppinglist");
            await StateService.UpdateAsync(stateMessage);
        }

        protected async Task OnDelete() {
            var pageMessage = PageMessage.DeleteItem;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            Navigation.NavigateTo("shoppinglist");
            await StateService.UpdateAsync(stateMessage);
        }

        protected async Task OnCancel() {
            var pageMessage = PageMessage.CancelItemEditForm;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            Navigation.NavigateTo("shoppinglist");
            await StateService.UpdateAsync(stateMessage);
        }
    }
}
