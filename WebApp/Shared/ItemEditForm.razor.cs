using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;

namespace WebApp.Shared {
    public partial class ItemEditForm : ComponentBase {
        [Parameter]
        public ItemForm.Form Form { get; set; }

        private void Process(ItemForm.ItemFormMessage msg) {
            Form = ItemForm.handleMessage(msg, Form);
        }
        protected void OnItemNameChange(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NewItemNameSet((string)e.Value));

        protected void OnItemNameFocusOut(FocusEventArgs e) =>
            Process(ItemForm.ItemFormMessage.ItemNameBlur);

        protected void OnQuantityChange(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NewQuantitySet((string)e.Value));

        protected void OnQuantityFocusOut(FocusEventArgs e) =>
            Process(ItemForm.ItemFormMessage.QuantityBlur);

        protected void OnNoteChange(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NewNoteSet((string)e.Value));

        protected void OnNoteFocusOut(FocusEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NoteBlur);
    }
}
