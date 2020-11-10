using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using WebApp.Common;
using static Models.ItemFormModule.ItemFormExtensions;
using FormMessage = Models.ItemFormModule.Message;
using TextBoxMessage = Models.CoreTypes.TextBoxMessage;

namespace WebApp.Shared {
    public partial class ItemEditFormSlick : ComponentBase {

        [Parameter]
        public CoreTypes.ItemForm Form { get; set; }

        [Parameter]
        public EventCallback OnDeleteCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        [Parameter]
        public EventCallback OnSaveChangesCallback { get; set; }

        [Parameter]
        public EventCallback<FormMessage> OnItemFormMessage { get; set; }

        protected void OnCancel() => OnCancelCallback.InvokeAsync(null);

        protected void OnDelete() => OnDeleteCallback.InvokeAsync(null);

        protected void OnSaveChanges() => OnSaveChangesCallback.InvokeAsync(null);


        protected void AddToShoppingListAgain() {
            Process(FormMessage.ScheduleOnce);
            OnSaveChangesCallback.InvokeAsync(null);
        }

        protected void OnSubmitPurchased() {
            Process(FormMessage.Purchased);
            OnSaveChangesCallback.InvokeAsync(null);
        }

        private void Process(FormMessage msg) =>
            OnItemFormMessage.InvokeAsync(msg);

        protected void OnToggleComplete() => Process(FormMessage.ToggleComplete);

        protected void OnItemNameChange(ChangeEventArgs e) =>
            Process(FormMessage.NewItemName(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnItemNameFocusOut(FocusEventArgs e) =>
            Process(FormMessage.NewItemName(TextBoxMessage.LoseFocus));

        protected void OnNoteChange(string s) =>
            Process(FormMessage.NewNote(TextBoxMessage.NewTypeText(s)));

        protected void OnNoteFocusOut() =>
            Process(FormMessage.NewNote(TextBoxMessage.LoseFocus));

        protected void OnQuantityChange(string s) =>
            Process(FormMessage.NewQuantity(TextBoxMessage.NewTypeText(s)));

        protected void OnQuantityFocusOut() =>
            Process(FormMessage.NewQuantity(TextBoxMessage.LoseFocus));

        protected void OnCategoryChooseExisting(string id) {
            if (Guid.TryParse(id, out Guid categoryGuid)) {
                var chooseSomeCat = FormMessage.NewChooseCategory(categoryGuid); // should be serialized string not Guid
                var modeIsChoose = FormMessage.CategoryModeChooseExisting;
                var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseSomeCat });
                Process(trans);
                GoBackHome();
            }
        }

        protected void OnCategoryChooseUncategorized() {
            var chooseUncategorized = FormMessage.ChooseCategoryUncategorized;
            var modeIsChoose = FormMessage.CategoryModeChooseExisting;
            var clearNew = FormMessage.NewNewCategoryName(TextBoxMessage.NewTypeText(""));
            var newLoseFocus = FormMessage.NewNewCategoryName(TextBoxMessage.LoseFocus);
            var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseUncategorized, clearNew, newLoseFocus });
            Process(trans);
            GoBackHome();
        }

        // varargs instead!
        protected void OnNewCategoryNameChange(string s) {
            if (string.IsNullOrWhiteSpace(s)) {
                var chooseUncategorized = FormMessage.ChooseCategoryUncategorized;
                var modeIsChoose = FormMessage.CategoryModeChooseExisting;
                var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseUncategorized });
                Process(trans);
            }
            else {
                var chooseNew = FormMessage.CategoryModeCreateNew;
                var e = FormMessage.NewNewCategoryName(TextBoxMessage.NewTypeText(s));
                var trans = FormMessage.NewTransaction(new List<FormMessage> { chooseNew, e });
                Process(trans);
            }
        }

        protected void OnNewCategoryNameFocusOut() =>
            Process(FormMessage.NewNewCategoryName(TextBoxMessage.LoseFocus));

        protected void OnNewCategoryNameCommit() {
            Process(FormMessage.CategoryModeCreateNew);
            GoBackHome();
        }

        protected void OnCategoryCancel() {
            if (Form.CategoryMode.IsCreateNew && Form.CategoryNameValidation().IsError) {
                OnCategoryChooseUncategorized();
            }
            else {
                GoBackHome();
            }
        }

        protected void OnNewCategoryNameCancel() {
            if (Form.CategoryNameValidation().IsError) {
                OnCategoryChooseUncategorized();
            }
            else {
                Process(FormMessage.CategoryModeChooseExisting);
                GoCategories();
            }
        }

        public int ActiveIndex { get; set; } = 0;

        public void GoBackHome() => ActiveIndex = 0;

        public void GoCategories() => ActiveIndex = 1;

        public void GoFrequency() => ActiveIndex = 2;

        public void GoStores() => ActiveIndex = 3;

        public void GoNote() => ActiveIndex = 4;

        public void GoPostpone() => ActiveIndex = 5;

        protected void OnRepeatChange(int d) {
            if (d <= 0) {
                var removePostpone = FormMessage.PostponeClear;
                var scheduleOnce = FormMessage.ScheduleOnce;
                var trans = FormMessage.NewTransaction(new List<FormMessage> { removePostpone, scheduleOnce });
                Process(trans); // why isn't this a Task?
                GoBackHome();
            }
            else {
                var scheduleIsRepeat = FormMessage.ScheduleRepeat;
                var setFrequency = FormMessage.NewFrequencySet(d);
                var trans = FormMessage.NewTransaction(new List<FormMessage> { scheduleIsRepeat, setFrequency });
                Process(trans);
                GoBackHome();
            }
        }

        protected void OnPostponeDays(int days) {
            Process(FormMessage.NewPostponeSet(days));
            GoBackHome();
        }

        protected void OnPostponeRemove() {
            Process(FormMessage.PostponeClear);
            GoBackHome();
        }

        protected void OnStoreChange(bool isSold, CoreTypes.StoreId store) =>
            Process(FormMessage.NewStoresSetAvailability(store, isSold));
    }
}
