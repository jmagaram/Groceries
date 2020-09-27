using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using System;
using static Models.FormsTypes;
using static Models.ItemEditFormModule;

namespace WebApp.Pages {
    public partial class ItemEdit : ComponentBase {

        protected override void OnInitialized() {
            base.OnInitialized();
            Form = createNew;
        }

        public T Form { get; private set; }

        private void Process(Message msg) {
            Form = processMessage(msg, Form);
        }

        protected void OnItemNameChange(ChangeEventArgs e) =>
            Process(Message.NewItemNameMessage(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnItemNameFocusOut(FocusEventArgs e) =>
            Process(Message.NewItemNameMessage(TextBoxMessage.LoseFocus));

        protected void OnNoteChange(ChangeEventArgs e) =>
            Process(Message.NewNoteMessage(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnNoteFocusOut(FocusEventArgs e) =>
            Process(Message.NewNoteMessage(TextBoxMessage.LoseFocus));

        protected void OnQuantityChange(ChangeEventArgs e) =>
            Process(Message.NewQuantityMessage(TextBoxMessage.NewTypeText((string)e.Value)));

        protected void OnQuantityFocusOut(FocusEventArgs e) =>
            Process(Message.NewQuantityMessage(TextBoxMessage.LoseFocus));

        protected void OnStoreChange(ChangeEventArgs e, StateTypes.StoreId store) =>
            Process(Message.NewStoreAvailability(store, (bool)e.Value));

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

        protected void OnNewCategoryNameChange(ChangeEventArgs e) =>
            Process(Message.NewCategoryMessage(CategoryMessage.NewNewCategoryMessage(TextBoxMessage.NewTypeText((string)e.Value))));

        protected void OnNewCategoryNameChange2(ChangeEventArgs e) =>
            Process(Message.NewCategoryMessage(CategoryMessage.NewNewCategoryMessage(TextBoxMessage.NewTypeText((string)e.Value))));


        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) =>
            Process(Message.NewCategoryMessage(CategoryMessage.NewNewCategoryMessage(TextBoxMessage.LoseFocus)));

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                var msg = Message.NewCategoryMessage(CategoryMessage.NewSelectorMessage(ChooseZeroOrOneMessage<Guid>.ClearSelection));
                Process(msg);
            }
            else if (value == chooseCreateNewCategory) {
                var msg = Message.NewCategoryMessage(CategoryMessage.NewSetMode(CategoryMode.CreateNewMode));
                Process(msg);
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                var msg = Message.NewCategoryMessage(CategoryMessage.NewSelectorMessage(ChooseZeroOrOneMessage<Guid>.NewChooseByKey(categoryId)));
                Process(msg);
            }
        }

        const string notPostponed = "notPostponed";

        protected void OnPostponeChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == notPostponed) {
                Form = Form.RemovePostpone();
            }
            else if (int.TryParse(value, out int days)) {
                Form = Form.SchedulePostpone(days);
            }
        }
    }
}
