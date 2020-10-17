using Microsoft.AspNetCore.Components;
using Models;
using System;

namespace WebApp.Pages {
    public partial class ItemEditPage : ComponentBase, IDisposable {
        IDisposable _subscription;

        protected override void OnInitialized() {
            if (!string.IsNullOrWhiteSpace(Id)) {
                var pageMessage = StateTypes.ItemEditPageMessage.NewBeginEditItem(Id);
                var stateMessage = StateTypes.StateMessage.NewItemEditPageMessage(pageMessage);
                StateService.Update(stateMessage);
            }
            else {
                var pageMessage =
                    string.IsNullOrWhiteSpace(ItemName)
                    ? StateTypes.ItemEditPageMessage.BeginCreateNewItem
                    : StateTypes.ItemEditPageMessage.NewBeginCreateNewItemWithName(ItemName);
                var stateMessage = StateTypes.StateMessage.NewItemEditPageMessage(pageMessage);
                StateService.Update(stateMessage);
            }
            _subscription = StateService.ItemEditPage.Subscribe(i => Form = i);
        }

        [Inject]
        public Data.ApplicationStateService StateService { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Parameter]
        public string ItemName { get; set; }

        [Parameter]
        public string Id { get; set; }

        public StateTypes.ItemForm Form { get; private set; }

        protected void OnFormMessage(StateTypes.ItemFormMessage message) {
            var pageMessage = StateTypes.ItemEditPageMessage.NewItemEditFormMessage(message);
            var stateMessage = StateTypes.StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
        }

        protected void OnClickOk() {
            var pageMessage = StateTypes.ItemEditPageMessage.SubmitItemEditForm;
            var stateMessage = StateTypes.StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
            Navigation.NavigateTo("shoppinglist");
        }

        protected void OnDelete() {
            var pageMessage = StateTypes.ItemEditPageMessage.DeleteItem;
            var stateMessage = StateTypes.StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
            Navigation.NavigateTo("shoppinglist");
        }

        protected void OnCancel() {
            var pageMessage = StateTypes.ItemEditPageMessage.CancelItemEditForm;
            var stateMessage = StateTypes.StateMessage.NewItemEditPageMessage(pageMessage);
            StateService.Update(stateMessage);
            Navigation.NavigateTo("shoppinglist");
        }

        public void Dispose() => _subscription?.Dispose();
    }
}
