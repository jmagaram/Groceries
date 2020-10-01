using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using System.Collections.Generic;
using WebApp.Common;

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

        protected void OnNewCategoryNameChange(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NewNewCategoryNameSet((string)e.Value));

        protected void OnScheduleOnce(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.ScheduleOnce);

        protected void OnClickScheduleOnce() =>
            Process(ItemForm.ItemFormMessage.ScheduleOnce);

        protected void OnScheduleRepeat(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.ScheduleRepeat);

        protected void OnScheduleCompleted(ChangeEventArgs e) =>
            Process(ItemForm.ItemFormMessage.ScheduleCompleted);

        protected void OnSaveChanges() {
            OnSaveChangesCallback.InvokeAsync(Form.ItemFormResult(DateTimeOffset.Now));
        }

        protected void OnSubmitPurchased() {
            Process(ItemForm.ItemFormMessage.Purchased);
            OnSaveChangesCallback.InvokeAsync(Form.ItemFormResult(DateTimeOffset.Now));
        }

        protected void OnSubmitPostponed(int days) {
            Process(ItemForm.ItemFormMessage.NewPostponeSet(days));
            OnSaveChangesCallback.InvokeAsync(Form.ItemFormResult(DateTimeOffset.Now));
        }

        protected void AddToShoppingListAgain() {
            Process(ItemForm.ItemFormMessage.ScheduleOnce);
            OnSaveChangesCallback.InvokeAsync(Form.ItemFormResult(DateTimeOffset.Now));
        }

        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NewCategoryNameBlur);

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                var chooseUncategorized = ItemForm.ItemFormMessage.ChooseCategoryUncategorized;
                var modeIsChoose = ItemForm.ItemFormMessage.CategoryModeChooseExisting;
                var trans = ItemForm.ItemFormMessage.NewTransaction(new List<ItemForm.ItemFormMessage> { modeIsChoose, chooseUncategorized });
                Process(trans);
            }
            else if (value == chooseCreateNewCategory) {
                var modeIsCreateNew = ItemForm.ItemFormMessage.CategoryModeCreateNew;
                Process(modeIsCreateNew);
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                var chooseSomeCat = ItemForm.ItemFormMessage.NewChooseCategory(categoryId);
                var modeIsChoose = ItemForm.ItemFormMessage.CategoryModeChooseExisting;
                var trans = ItemForm.ItemFormMessage.NewTransaction(new List<ItemForm.ItemFormMessage> { modeIsChoose, chooseSomeCat });
                Process(trans);
            }
        }

        const int notRepeating = -1;

        protected void OnRepeatChange(ChangeEventArgs e) {
            if (int.TryParse((string)(e.Value), out int d)) {
                if (d == notRepeating) {
                    var removePostpone = ItemForm.ItemFormMessage.PostponeClear;
                    var scheduleOnce = ItemForm.ItemFormMessage.ScheduleOnce;
                    var trans = ItemForm.ItemFormMessage.NewTransaction(new List<ItemForm.ItemFormMessage> { removePostpone, scheduleOnce });
                    Process(trans);
                }
                else {
                    var scheduleIsRepeat = ItemForm.ItemFormMessage.ScheduleRepeat;
                    var setFrequency = ItemForm.ItemFormMessage.NewFrequencySet(d);
                    var trans = ItemForm.ItemFormMessage.NewTransaction(new List<ItemForm.ItemFormMessage> { scheduleIsRepeat, setFrequency });
                    Process(trans);
                }
            }
        }

        const string notPostponed = "notPostponed";

        protected void OnPostponeChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == notPostponed) {
                Process(ItemForm.ItemFormMessage.PostponeClear);
            }
            else if (int.TryParse(value, out int days)) {
                Process(ItemForm.ItemFormMessage.NewPostponeSet(days));
            }
        }

        protected void OnPostponeClick(int days) =>
            Process(ItemForm.ItemFormMessage.NewPostponeSet(days));

        protected void OnPostponeClear() =>
            Process(ItemForm.ItemFormMessage.PostponeClear);

        protected void OnPostponeToggle() {
            if (Form.Postpone.IsNone()) {
                Process(ItemForm.ItemFormMessage.NewPostponeSet(7));
            }
            else {
                Process(ItemForm.ItemFormMessage.PostponeClear);
            }
        }

        protected void OnCancel() => OnCancelCallback.InvokeAsync(null);

        protected void OnDelete() => OnDeleteCallback.InvokeAsync(Form.ItemId.Value);

        protected void OnStoreChange(ChangeEventArgs e, StateTypes.StoreId store) =>
            Process(ItemForm.ItemFormMessage.NewStoresSetAvailability(store, (bool)e.Value));

        [Parameter]
        public EventCallback<StateTypes.ItemId> OnDeleteCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        [Parameter]
        public EventCallback<ItemForm.ItemFormResult> OnSaveChangesCallback { get; set; }

    }
}
