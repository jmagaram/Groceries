using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using System.Collections.Generic;
using WebApp.Common;
using static Models.ItemFormModule.ItemFormExtensions;

namespace WebApp.Shared {
    public partial class ItemEditForm : ComponentBase {
        [Parameter]
        public StateTypes.ItemForm Form { get; set; }

        private void Process(StateTypes.ItemFormMessage msg) => 
            OnItemFormMessage.InvokeAsync(msg);

        protected void OnItemNameChange(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.NewItemNameSet((string)e.Value));

        protected void OnItemNameFocusOut(FocusEventArgs e) =>
            Process(StateTypes.ItemFormMessage.ItemNameBlur);

        protected void OnQuantityChange(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.NewQuantitySet((string)e.Value));

        protected void OnQuantityFocusOut(FocusEventArgs e) =>
            Process(StateTypes.ItemFormMessage.QuantityBlur);

        protected void OnNoteChange(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.NewNoteSet((string)e.Value));

        protected void OnNoteFocusOut(FocusEventArgs e) =>
            Process(StateTypes.ItemFormMessage.NoteBlur);

        protected void OnNewCategoryNameChange(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.NewNewCategoryNameSet((string)e.Value));

        protected void OnScheduleOnce(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.ScheduleOnce);

        protected void OnClickScheduleOnce() =>
            Process(StateTypes.ItemFormMessage.ScheduleOnce);

        protected void OnScheduleRepeat(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.ScheduleRepeat);

        protected void OnScheduleCompleted(ChangeEventArgs e) =>
            Process(StateTypes.ItemFormMessage.ScheduleCompleted);

        protected void OnSaveChanges() => OnSaveChangesCallback.InvokeAsync(null);

        protected void OnSubmitPurchased() {
            Process(StateTypes.ItemFormMessage.Purchased);
            OnSaveChangesCallback.InvokeAsync(null);
        }

        protected void OnSubmitPostponed(int days) {
            Process(StateTypes.ItemFormMessage.NewPostponeSet(days));
            OnSaveChangesCallback.InvokeAsync(null);
        }

        protected void AddToShoppingListAgain() {
            Process(StateTypes.ItemFormMessage.ScheduleOnce);
            OnSaveChangesCallback.InvokeAsync(null);
        }

        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) =>
            Process(StateTypes.ItemFormMessage.NewCategoryNameBlur);

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                var chooseUncategorized = StateTypes.ItemFormMessage.ChooseCategoryUncategorized;
                var modeIsChoose = StateTypes.ItemFormMessage.CategoryModeChooseExisting;
                var trans = StateTypes.ItemFormMessage.NewTransaction(new List<StateTypes.ItemFormMessage> { modeIsChoose, chooseUncategorized });
                Process(trans);
            }
            else if (value == chooseCreateNewCategory) {
                var modeIsCreateNew = StateTypes.ItemFormMessage.CategoryModeCreateNew;
                Process(modeIsCreateNew);
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                var chooseSomeCat = StateTypes.ItemFormMessage.NewChooseCategory(categoryId);
                var modeIsChoose = StateTypes.ItemFormMessage.CategoryModeChooseExisting;
                var trans = StateTypes.ItemFormMessage.NewTransaction(new List<StateTypes.ItemFormMessage> { modeIsChoose, chooseSomeCat });
                Process(trans);
            }
        }

        const int notRepeating = -1;

        protected void OnRepeatChange(ChangeEventArgs e) {
            if (int.TryParse((string)(e.Value), out int d)) {
                if (d == notRepeating) {
                    var removePostpone = StateTypes.ItemFormMessage.PostponeClear;
                    var scheduleOnce = StateTypes.ItemFormMessage.ScheduleOnce;
                    var trans = StateTypes.ItemFormMessage.NewTransaction(new List<StateTypes.ItemFormMessage> { removePostpone, scheduleOnce });
                    Process(trans);
                }
                else {
                    var scheduleIsRepeat = StateTypes.ItemFormMessage.ScheduleRepeat;
                    var setFrequency = StateTypes.ItemFormMessage.NewFrequencySet(d);
                    var trans = StateTypes.ItemFormMessage.NewTransaction(new List<StateTypes.ItemFormMessage> { scheduleIsRepeat, setFrequency });
                    Process(trans);
                }
            }
        }

        const string notPostponed = "notPostponed";

        protected void OnPostponeChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == notPostponed) {
                Process(StateTypes.ItemFormMessage.PostponeClear);
            }
            else if (int.TryParse(value, out int days)) {
                Process(StateTypes.ItemFormMessage.NewPostponeSet(days));
            }
        }

        protected void OnPostponeClick(int days) =>
            Process(StateTypes.ItemFormMessage.NewPostponeSet(days));

        protected void OnPostponeClear() =>
            Process(StateTypes.ItemFormMessage.PostponeClear);

        protected void OnPostponeToggle() {
            if (Form.Postpone.IsNone()) {
                Process(StateTypes.ItemFormMessage.NewPostponeSet(7));
            }
            else {
                Process(StateTypes.ItemFormMessage.PostponeClear);
            }
        }

        protected void OnCancel() => OnCancelCallback.InvokeAsync(null);

        protected void OnDelete() => OnDeleteCallback.InvokeAsync(null);

        protected void OnStoreChange(ChangeEventArgs e, StateTypes.StoreId store) =>
            Process(StateTypes.ItemFormMessage.NewStoresSetAvailability(store, (bool)e.Value));

        [Parameter]
        public EventCallback OnDeleteCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        [Parameter]
        public EventCallback OnSaveChangesCallback { get; set; }

        [Parameter]
        public EventCallback<StateTypes.ItemFormMessage> OnItemFormMessage { get; set; }

    }
}
