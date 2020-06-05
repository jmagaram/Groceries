using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ItemBuilder;
using static DomainTypes.Frequency;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.FSharp.Core;
using Microsoft.Extensions.ObjectPool;

using static DomainTypes;
using Microsoft.AspNetCore.Components.Web;

namespace Client.Pages
{
    public partial class EditItem
    {
        protected DomainTypes.Model model;

        protected string FrequencyText(DomainTypes.Frequency f)
        {
            var days = Frequency.asDays(f);
            var (months, remainer) = (days / 30, days % 30);
            //if (days % 30 as int d == 0)
            //    return x
            // every 6 weeks
            // every 1 month 
            // every 13 days
            return "";
        }

        protected string RepeatText(FSharpOption<int> daysInterval)
        {
            return
                daysInterval
                .AsEnumerable()
                .Select(i => (i % 30 == 0) ? $"{i / 30} months"
                : (i % 7 == 0) ? $"{i / 7} weeks" : $"{i} days")
                .FirstOrDefault() ?? "Does not repeat";
        }



        // make this its own file around RepeatInterval (which is just an int option)
        protected string SerializeRepeatInterval(int? daysInterval)
        {
            return RepeatSelector.serializeInterval(daysInterval);
        }

        //protected string CurrentFrequencyKey() => model.Repeat.IsNone()
        //    ? frequencyChoices.Single(i => i.Frequency == null).Key
        //    : frequencyChoices.Single(i => model.Repeat.Value.Equals(i.Frequency)).Key;

        protected void Activate() => model = activate(model).Value;

        protected void OnTitleChange(ChangeEventArgs c) => model = setTitle((string)c.Value, model);

        protected void OnNoteChange(ChangeEventArgs c) => model = setNote((string)c.Value, model);

        protected void SetQty(ChangeEventArgs c)
        {
            string qty = (string)c.Value;
            model = ItemBuilder.setQuantity(qty, model);
        }

        protected void IncreaseQuantity() => model = ItemBuilder.increaseQty.Invoke(model).Value;

        protected void DecreaseQuantity() => model = ItemBuilder.decreaseQty.Invoke(model).Value;

        protected bool CanIncreaseQuantity() => increaseQty.Invoke(model).IsSome();

        protected bool CanDecreaseQuantity() => decreaseQty.Invoke(model).IsSome();

        protected void OnMakeRecurring() => model = ItemBuilder.setRepeat(7, model);

        //protected string ChoiceAsText(Status s)
        //{
        //    if (s.IsActive)
        //    {
        //        return "Active";
        //    }
        //    else if (s.IsCompleted)
        //    {
        //        return "Purchased";
        //    }
        //    else
        //    {
        //        string Units(int count, string singular, string plural) =>
        //            count == 0 ? "" :
        //            count == 1 ? $"{count} {singular}" :
        //            $"{count} {plural}";
        //        var i = chunk(s.TryPostponed.Value);
        //        var months = Units(i.Months, "month", "months") + " ";
        //        var weeks = Units(i.Weeks, "week", "weeks") + " ";
        //        var days = Units(i.Days, "day", "days") + " ";
        //        return $"Postpone {months}{weeks}{days}".Trim();
        //    }
        //}

        protected void OnRecurrenceChange(ChangeEventArgs c)
        {
            string chosenItemValue = (string)c.Value;
            var chosen = RepeatSelector.deserializeInterval(chosenItemValue);
            if (chosen == null || chosen.IsNone())
            {
                model = removeRepeat(model);
            }
            else
            {
                model = setRepeat(chosen.Value, model);
            }
        }



        protected void ChangeStatus(ChangeEventArgs c)
        {
            //var choice = ItemBuilder.deserialize((string)c.Value);
            //if (choice.IsActive)
            //{
            //    model = activate(model).Value;
            //}
            //else if (choice.IsPostponed)
            //{
            //    model = postpone(model).Value.Invoke(choice.TryPostponed.Value);
            //}
            //else if (choice.IsCompleted)
            //{
            //    model = complete(model).Value;
            //}
        }

        //protected Status CurrentStatus() => ItemBuilder.currentStatus(model.Schedule);

        protected override void OnInitialized()
        {
            base.OnInitialized();
            model = ItemBuilder.create;
            tb = ItemBuilder.textBoxInitial;
        }

        protected TextBox<string> tb;

        protected void UpdateTb(TextBoxMessage msg) => 
            tb = ItemBuilder.textBoxUpdate.Invoke(msg).Invoke(tb);

        protected void tbonfocus(FocusEventArgs f) => UpdateTb(DomainTypes.TextBoxMessage.GetFocus);

        protected void tbonblur(FocusEventArgs f) => UpdateTb(DomainTypes.TextBoxMessage.LoseFocus);

        protected void tbchange(ChangeEventArgs c) => UpdateTb(DomainTypes.TextBoxMessage.NewSetText(c.Value.ToString()));
    }
}
