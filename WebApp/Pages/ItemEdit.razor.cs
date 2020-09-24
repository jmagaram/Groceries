using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using static Models.ItemEditFormModule;

namespace WebApp.Pages {
    public partial class ItemEdit : ComponentBase {

        protected override void OnInitialized() {
            base.OnInitialized();
            Form = createNew;
        }

        public string Testing { get; set; }

        public T Form { get; private set; }

        protected void OnItemNameChange(ChangeEventArgs e) =>
            Form = Form.ItemNameEdit((string)e.Value);

        protected void OnItemNameFocusOut(FocusEventArgs e) =>
            Form = Form.ItemNameLoseFocus();

        protected void OnQuantityChange(ChangeEventArgs e) =>
            Form = Form.QuantityEdit((string)e.Value);

        protected void OnQuantityFocusOut(FocusEventArgs e) =>
            Form = Form.QuantityLoseFocus();

        protected void OnNoteChange(ChangeEventArgs e) =>
            Form = Form.NoteEdit((string)e.Value);

        protected void OnNoteFocusOut(FocusEventArgs e) =>
            Form = Form.NoteLoseFocus();

        protected void OnStoreChange(ChangeEventArgs e, StateTypes.StoreId store) =>
            Form = Form.SetStoreAvailability(store, (bool)e.Value);

        protected void OnRepeatChange(ChangeEventArgs e) {
            if (int.TryParse((string)(e.Value), out int d)) {
                if (d == -1) {
                    Form = Form.ScheduleOnlyOnce();
                }
                else {
                    Form = Form.ScheduleRepeat(d);
                }
            }
        }

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            if (int.TryParse((string)(e.Value), out int d)) {
                if (d == -1) {
                    Form = Form.ModeNoCategory();
                }
            }
            else {
                if (Guid.TryParse((string)e.Value, out Guid g)) {
                    Form = Form.ChooseExistingCategory(g);
                }
                else {

                }
            }
        }
    }
}
