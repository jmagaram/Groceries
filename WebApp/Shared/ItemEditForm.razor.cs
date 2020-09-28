using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using System.Collections.Generic;

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

        protected void OnNewCategoryNameChange2(ChangeEventArgs e) {

            Process(ItemForm.ItemFormMessage.NewNewCategoryNameSet((string)e.Value));
        }

        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) =>
            Process(ItemForm.ItemFormMessage.NewCategoryNameBlur);

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                var chooseUncategorized = ItemForm.ItemFormMessage.ChooseCategoryUncategorized;
                var modeIsChoose = ItemForm.ItemFormMessage.CategoryModeIsChooseExisting;
                var trans = ItemForm.ItemFormMessage.NewTransaction(new List<ItemForm.ItemFormMessage> { modeIsChoose, chooseUncategorized });
                Process(trans);
            }
            else if (value == chooseCreateNewCategory) {
                var modeIsCreateNew = ItemForm.ItemFormMessage.CategoryModeIsCreateNew;
                Process(modeIsCreateNew);
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                var chooseSomeCat = ItemForm.ItemFormMessage.NewChooseCategory(categoryId);
                var modeIsChoose = ItemForm.ItemFormMessage.CategoryModeIsChooseExisting;
                var trans = ItemForm.ItemFormMessage.NewTransaction(new List<ItemForm.ItemFormMessage> { modeIsChoose, chooseSomeCat });
                Process(trans);
            }
        }

        protected void OnRepeatChange(ChangeEventArgs e) {
            if (int.TryParse((string)(e.Value), out int d)) {
                //if (d == -1) {
                //    Form = Form.ScheduleOnlyOnce();
                //}
                Process(ItemForm.ItemFormMessage.NewFrequencySet(d));
            }
        }

        const string notPostponed = "notPostponed";

        protected void OnPostponeChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            //if (value == notPostponed) {
            //    Form = Form.RemovePostpone();
            //}
            if (int.TryParse(value, out int days)) {
                Process(ItemForm.ItemFormMessage.NewPostponeSet(days));
            }
        }

        protected void OnStoreChange(ChangeEventArgs e, StateTypes.StoreId store) =>
            Process(ItemForm.ItemFormMessage.NewStoresSetAvailability(store, (bool)e.Value));

        [Parameter]
        public EventCallback<MouseEventArgs> OnSubmitCallback { get; set; }
    }
}
