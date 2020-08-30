using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomainTypes;
using System.Reactive.Linq;

namespace Client.Shared
{
    public partial class ItemEditComponent : ComponentBase, IDisposable
    {
        private IDisposable _subscription = null;
        public DomainTypes.ItemEditView _view = null;

        [Inject]
        protected Data.ApplicationStateService StateService { get; set; }

        [Parameter]
        public EventCallback OnSubmit { get; set; }

        [Parameter]
        public EventCallback OnDelete { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _subscription =
                ItemEditView
                .fromObservable(StateService.StateObservable.Select(i => i.EditModel))
                .Subscribe(OnNextView);
        }

        protected void OnTitleChange(ChangeEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewTitleMessage(FormFieldMessage<string>.NewPropose((string)e.Value))));

        public void OnTitleFocusIn(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewTitleMessage(FormFieldMessage<string>.GainedFocus)));

        public void OnTitleFocusOut(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewTitleMessage(FormFieldMessage<string>.LostFocus)));

        protected void OnNoteChange(ChangeEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewNoteMessage(FormFieldMessage<string>.NewPropose((string)e.Value))));

        public void OnNoteFocusIn(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewNoteMessage(FormFieldMessage<string>.GainedFocus)));

        public void OnNoteFocusOut(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewNoteMessage(FormFieldMessage<string>.LostFocus)));

        protected void OnQuantityChange(ChangeEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewQuantityMessage(FormFieldMessage<string>.NewPropose((string)e.Value))));

        public void OnQuantityFocusIn(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewQuantityMessage(FormFieldMessage<string>.GainedFocus)));

        public void OnQuantityFocusOut(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewQuantityMessage(FormFieldMessage<string>.LostFocus)));
        protected void OnRepeatChange(ChangeEventArgs e)
        {
            string selected = (string)(e.Value);
            var selectedRepeat = _view.Repeat.Deserialize.Invoke(selected);
            StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewRepeatMessage(FormFieldMessage<DomainTypes.Repeat>.NewPropose(selectedRepeat))));
        }
        public void OnRepeatFocusIn(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewRepeatMessage(FormFieldMessage<DomainTypes.Repeat>.GainedFocus)));
        public void OnRepeatFocusOut(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewRepeatMessage(FormFieldMessage<DomainTypes.Repeat>.LostFocus)));
        protected void OnRelativeStatusChange(ChangeEventArgs e)
        {
            string selected = (string)(e.Value);
            var selectedRelativeStatus = _view.RelativeStatus.Deserialize.Invoke(selected);
            StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewSetRelativeStatus(FormFieldMessage<DomainTypes.RelativeStatus>.NewPropose(selectedRelativeStatus))));
        }

        public void OnRelativeStatusFocusIn(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewSetRelativeStatus(FormFieldMessage<DomainTypes.RelativeStatus>.GainedFocus)));

        public void OnRelativeStatusFocusOut(FocusEventArgs e) => StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewSetRelativeStatus(FormFieldMessage<DomainTypes.RelativeStatus>.LostFocus)));

        protected void QuantityDecrease() =>
    StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewInvokeCommand(ItemEditCommand.QuantityDecrease)));

        protected void QuantityIncrease() =>
            StateService.Update(StateMessage.NewItemEditMessage(ItemEditMessage.NewInvokeCommand(ItemEditCommand.QuantityIncrease)));

        protected string RepeatAsText(DomainTypes.Repeat r) => global::Repeat.formatEnglish.Invoke(r);

        protected string RelativeStatusAsText(DomainTypes.RelativeStatus r)
        {
            if (r.IsComplete)
                return "Complete";
            else if (r.IsActive)
                return "Active";
            else
            {
                (int, string) units(int days) =>
                    (days % 30 == 0)
                    ? (days / 30, days == 30 ? "month" : "months")
                    : (days % 7 == 0)
                    ? (days / 7, days == 7 ? "week" : "weeks")
                    : (days, days == 1 ? "day" : "days");
                var d = (r as DomainTypes.RelativeStatus.PostponedDays).Item;
                var (count, unitType) = units(d);
                if (d == 1)
                    return $"Postponed until tomorrow";
                else if (d == 0)
                    return $"Due today";
                else if (d > 0)
                    return $"Postponed for {count} {unitType}";
                else
                    return $"Overdue {-count} {unitType}";
            }
        }

        void OnNextView(DomainTypes.ItemEditView view) => _view = view;

        public void Dispose() => _subscription?.Dispose();
    }
}
