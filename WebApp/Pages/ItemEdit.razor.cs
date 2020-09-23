using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using static Models.ItemEditFormModule;

namespace WebApp.Pages {
    public partial class ItemEdit : ComponentBase {

        protected override void OnInitialized() {
            base.OnInitialized();
            Form = createNew;
        }

        public string Testing { get; set; }
        public T Form { get; private set; }
        protected void OnItemNameChange(ChangeEventArgs e) => Form = Form.ItemNameEdit((string)e.Value);

        protected void OnItemNameFocusOut(FocusEventArgs e) => Form = Form.ItemNameLoseFocus();

        protected void OnQuantityChange(ChangeEventArgs e) => Form = Form.QuantityEdit((string)e.Value);

        protected void OnQuantityFocusOut(FocusEventArgs e) => Form = Form.QuantityLoseFocus();

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
    }
}
