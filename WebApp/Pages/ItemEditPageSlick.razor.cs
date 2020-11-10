using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Models;
using WebApp.Common;
using FormMessage = Models.ItemFormModule.Message;
using PageMessage = Models.StateTypes.ItemEditPageMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public partial class ItemEditPageSlick : ComponentBase, IDisposable {
        IDisposable _subscription;

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
            _subscription =
                StateService.State
                .Select(i => i.ItemEditPage)
                .Where(i => i.IsSome())
                .Select(i => i.Value)
                .DistinctUntilChanged()
                .Subscribe(i => Form = i);
        }

        [Inject]
        public Service StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Parameter]
        public string ItemName { get; set; }

        [Parameter]
        public string Id { get; set; }

        public CoreTypes.ItemForm Form { get; private set; }

        protected async Task OnFormMessage(FormMessage message) {
            var pageMessage = PageMessage.NewItemEditFormMessage(message);
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            await StateService.UpdateAsync(stateMessage);
        }

        protected async Task OnClickOk() {
            var pageMessage = PageMessage.SubmitItemEditForm;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            Dispose();
            Navigation.NavigateTo("shoppinglist");
            await StateService.UpdateAsync(stateMessage);
        }

        protected async Task OnDelete() {
            var pageMessage = PageMessage.DeleteItem;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            Dispose();
            Navigation.NavigateTo("shoppinglist");
            await StateService.UpdateAsync(stateMessage);
        }

        protected async Task OnCancel() {
            var pageMessage = PageMessage.CancelItemEditForm;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            Dispose();
            Navigation.NavigateTo("shoppinglist");
            await StateService.UpdateAsync(stateMessage);
        }

        public void Dispose() => _subscription?.Dispose();
    }
}
