using System;
using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;

using Models;
using WebApp.Common;
using FormMessage = Models.ItemFormModule.Message;
using PageMessage = Models.StateTypes.ItemEditPageMessage;
using StateMessage = Models.StateTypes.StateMessage;

namespace WebApp.Pages {
    public partial class ItemEditPage : ComponentBase, IDisposable {
        IDisposable _subscription;

        protected override void OnInitialized() {
            if (!string.IsNullOrWhiteSpace(Id)) {
                var pageMessage = PageMessage.NewBeginEditItem(Id);
                var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
                StateService.Update(stateMessage);
            }
            else {
                var pageMessage =
                    string.IsNullOrWhiteSpace(ItemName)
                    ? PageMessage.BeginCreateNewItem
                    : PageMessage.NewBeginCreateNewItemWithName(ItemName);
                var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
                StateService.Update(stateMessage);
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
        public Models.Service StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Parameter]
        public string ItemName { get; set; }

        [Parameter]
        public string Id { get; set; }

        public CoreTypes.ItemForm Form { get; private set; }

        protected void OnFormMessage(FormMessage message) {
            var pageMessage = PageMessage.NewItemEditFormMessage(message);
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
        }

        protected void OnClickOk() {
            var pageMessage = PageMessage.SubmitItemEditForm;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
            Navigation.NavigateTo("shoppinglist");
        }

        protected void OnDelete() {
            var pageMessage = PageMessage.DeleteItem;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
            Navigation.NavigateTo("shoppinglist");
        }

        protected void OnCancel() {
            var pageMessage = PageMessage.CancelItemEditForm;
            var stateMessage = StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
            Navigation.NavigateTo("shoppinglist");
        }

        public void Dispose() => _subscription?.Dispose();
    }
}
