using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using Models;

using WebApp.Common;

using FormMessage = Models.ItemFormModule.Message;
using TextBoxMessage = Models.CoreTypes.TextBoxMessage;

namespace WebApp.Shared {
    public partial class ItemEditForm : ComponentBase {
        [Parameter]
        public CoreTypes.ItemForm Form { get; set; }

        public bool IsFrequencyDialogOpen { get; set; }

        public void ShowFrequencyDialog() => IsFrequencyDialogOpen = true;

        public void HideFrequencyDialog() => IsFrequencyDialogOpen = false;


        private void Process(FormMessage msg) =>
            OnItemFormMessage.InvokeAsync(msg);

        protected void OnItemNameChange(ChangeEventArgs e) =>
            Process(FormMessage.NewItemName(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnItemNameFocusOut(FocusEventArgs e) =>
            Process(FormMessage.NewItemName(TextBoxMessage.LoseFocus));

        protected void OnQuantityChange(ChangeEventArgs e) =>
            Process(FormMessage.NewQuantity(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnQuantityFocusOut(FocusEventArgs e) =>
            Process(FormMessage.NewQuantity(TextBoxMessage.LoseFocus));

        protected void OnNoteChange(ChangeEventArgs e) =>
            Process(FormMessage.NewNote(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnNoteFocusOut(FocusEventArgs e) =>
            Process(FormMessage.NewNote(TextBoxMessage.LoseFocus));

        protected void OnNewCategoryNameChange(ChangeEventArgs e) =>
            Process(FormMessage.NewNewCategoryName(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) =>
            Process(FormMessage.NewNewCategoryName(TextBoxMessage.LoseFocus));

        protected void OnScheduleOnce(ChangeEventArgs e) =>
            Process(FormMessage.ScheduleOnce);

        protected void OnClickScheduleOnce() =>
            Process(FormMessage.ScheduleOnce);

        protected void OnScheduleRepeat(ChangeEventArgs e) =>
            Process(FormMessage.ScheduleRepeat);

        protected void OnScheduleCompleted(ChangeEventArgs e) =>
            Process(FormMessage.ScheduleCompleted);

        protected void OnSaveChanges() => OnSaveChangesCallback.InvokeAsync(null);

        protected void OnSubmitPurchased() {
            Process(FormMessage.Purchased);
            OnSaveChangesCallback.InvokeAsync(null);
        }

        protected void OnSubmitPostponed(int days) {
            Process(FormMessage.NewPostponeSet(days));
            OnSaveChangesCallback.InvokeAsync(null);
        }

        protected void AddToShoppingListAgain() {
            Process(FormMessage.ScheduleOnce);
            OnSaveChangesCallback.InvokeAsync(null);
        }

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                var chooseUncategorized = FormMessage.ChooseCategoryUncategorized;
                var modeIsChoose = FormMessage.CategoryModeChooseExisting;
                var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseUncategorized });
                Process(trans);
            }
            else if (value == chooseCreateNewCategory) {
                var modeIsCreateNew = FormMessage.CategoryModeCreateNew;
                Process(modeIsCreateNew);
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                var chooseSomeCat = FormMessage.NewChooseCategory(categoryId);
                var modeIsChoose = FormMessage.CategoryModeChooseExisting;
                var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseSomeCat });
                Process(trans);
            }
        }

        const int notRepeating = -1;

        protected void OnFrequencySelect(int d) {
            if (d == notRepeating) {
                var removePostpone = FormMessage.PostponeClear;
                var scheduleOnce = FormMessage.ScheduleOnce;
                var trans = FormMessage.NewTransaction(new List<FormMessage> { removePostpone, scheduleOnce });
                Process(trans);
                HideFrequencyDialog();
            }
            else {
                var scheduleIsRepeat = FormMessage.ScheduleRepeat;
                var setFrequency = FormMessage.NewFrequencySet(d);
                var trans = FormMessage.NewTransaction(new List<FormMessage> { scheduleIsRepeat, setFrequency });
                Process(trans);
                HideFrequencyDialog();
            }
        }

        protected void OnRepeatChange(ChangeEventArgs e) {
            if (int.TryParse((string)(e.Value), out int d)) {
                if (d == notRepeating) {
                    var removePostpone = FormMessage.PostponeClear;
                    var scheduleOnce = FormMessage.ScheduleOnce;
                    var trans = FormMessage.NewTransaction(new List<FormMessage> { removePostpone, scheduleOnce });
                    Process(trans);
                }
                else {
                    var scheduleIsRepeat = FormMessage.ScheduleRepeat;
                    var setFrequency = FormMessage.NewFrequencySet(d);
                    var trans = FormMessage.NewTransaction(new List<FormMessage> { scheduleIsRepeat, setFrequency });
                    Process(trans);
                }
            }
        }

        const string notPostponed = "notPostponed";

        protected void OnPostponeChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == notPostponed) {
                Process(FormMessage.PostponeClear);
            }
            else if (int.TryParse(value, out int days)) {
                Process(FormMessage.NewPostponeSet(days));
            }
        }

        protected void OnPostponeClick(int days) =>
            Process(FormMessage.NewPostponeSet(days));

        protected void OnPostponeClear() =>
            Process(FormMessage.PostponeClear);

        protected void OnPostponeToggle() {
            if (Form.Postpone.IsNone()) {
                Process(FormMessage.NewPostponeSet(7));
            }
            else {
                Process(FormMessage.PostponeClear);
            }
        }

        protected void OnCancel() => OnCancelCallback.InvokeAsync(null);

        protected void OnDelete() => OnDeleteCallback.InvokeAsync(null);

        protected void OnStoreChange(ChangeEventArgs e, CoreTypes.StoreId store) =>
            Process(FormMessage.NewStoresSetAvailability(store, (bool)e.Value));

        [Parameter]
        public EventCallback OnDeleteCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        [Parameter]
        public EventCallback OnSaveChangesCallback { get; set; }

        [Parameter]
        public EventCallback<FormMessage> OnItemFormMessage { get; set; }

    }
}
