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

        private void ProcessMessage(ItemEditFormModule.Message msg) {
            Form = processMessage(msg, Form);
        }

        protected void OnItemNameChange(ChangeEventArgs e) => ProcessMessage(Message.NewItemNameEdit((string)e.Value));

        protected void OnItemNameFocusOut(FocusEventArgs e) => ProcessMessage(Message.ItemNameLoseFocus);

        protected void OnQuantityChange(ChangeEventArgs e) => ProcessMessage(Message.NewQuantityEdit((string)e.Value));

        protected void OnQuantityFocusOut(FocusEventArgs e) => ProcessMessage(Message.QuantityLoseFocus);

        protected void OnNoteChange(ChangeEventArgs e) => ProcessMessage(Message.NewNoteEdit((string)e.Value));

        protected void OnNoteFocusOut(FocusEventArgs e) => ProcessMessage(Message.NoteLoseFocus);

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

        protected void OnNewCategoryNameChange(ChangeEventArgs e) {
            var msg = Message.NewNewCategoryMessage(TextInputMessage.NewTypeText((string)e.Value));
            Form = processMessage(msg, Form);
        }

        protected void OnNewCategoryNameFocusOut(FocusEventArgs e) {
            var msg = Message.NewNewCategoryMessage(TextInputMessage.LoseFocus);
            Form = processMessage(msg, Form);
        }

        const string chooseUncategorized = "chooseUncategorized";
        const string chooseCreateNewCategory = "chooseNewCategory";

        protected void OnExistingCategoryChange(ChangeEventArgs e) {
            string value = (string)(e.Value);
            if (value == chooseUncategorized) {
                var msg = Message.NewExistingCategoryMessage(ChooseZeroOrOneMessage<Guid>.ClearSelection);
                Form = processMessage(msg, Form);
            }
            else if (value == chooseCreateNewCategory) {
                var msg = Message.StartCreatingNewCategory;
                Form = processMessage(msg, Form);
            }
            else if (Guid.TryParse(value, out Guid categoryId)) {
                var msg = Message.NewExistingCategoryMessage(ChooseZeroOrOneMessage<Guid>.NewChooseByKey(categoryId));
                Form = processMessage(msg, Form);
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
