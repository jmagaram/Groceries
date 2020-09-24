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

        protected void OnNewCategoryNameChange(ChangeEventArgs e) =>
            Form = Form.NewCategoryEdit((string)e.Value);

        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) =>
            Form = Form.NewCategoryLoseFocus();

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

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                Form = Form.ModeNoCategory();
            }
            else if (value == chooseCreateNewCategory) {
                Form = Form.ModeCreateNew();
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                Form = Form.ChooseExistingCategory(categoryId);

            }
        }

        const string notPostponed = "notPostponed";

        protected void OnPostponeChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == notPostponed) {
                Form = Form.ModeNoCategory();
            }
            else if (int.TryParse(value, out int days)) {
                Form = Form.SchedulePostpone(days);
            }
        }
    }
}
