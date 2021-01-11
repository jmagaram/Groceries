using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using static Models.CoreTypes;
using static Models.ViewTypes;
using GroceriesWasmApp.Client.Common;
using FormMessage = Models.ItemFormModule.Message;
using TextBoxMessage = Models.CoreTypes.TextBoxMessage;

namespace GroceriesWasmApp.Client.Shared
{
    public partial class ItemEditFormSlick : ComponentBase
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private CategoryDrawer _categoryDrawer;
        private PostponeDrawer _postponeDrawer;
        private StoresDrawer _storesDrawer;
#pragma warning restore IDE0044 // Add readonly modifier

        [Parameter]
        public ItemForm Form { get; set; }

        [Parameter]
        public EventCallback OnDeleteCallback { get; set; }

        [Parameter]
        public EventCallback OnCancelCallback { get; set; }

        [Parameter]
        public EventCallback OnSaveChangesCallback { get; set; }

        [Parameter]
        public EventCallback<FormMessage> OnItemFormMessage { get; set; }

        private async ValueTask OnClickCategory() =>
            await _categoryDrawer.Open(
                SelectZeroOrOneCategory.createFromPickList(Form.CategoryChoice, Form.CategoryChoiceList),
                Form.ItemName.ValueCommitted);

        private void OnCategorySelected(SelectZeroOrOne<Category> f)
        {
            if (SelectZeroOrOneModule.hasChanges(f))
            {
                if (f.CurrentChoice.IsNone())
                {
                    var chooseUncategorized = FormMessage.ChooseCategoryUncategorized;
                    var modeIsChoose = FormMessage.CategoryModeChooseExisting;
                    var clearNew = FormMessage.NewNewCategoryName(TextBoxMessage.NewTypeText(""));
                    var newLoseFocus = FormMessage.NewNewCategoryName(TextBoxMessage.LoseFocus);
                    var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseUncategorized, clearNew, newLoseFocus });
                    Process(trans);
                }
                else
                {
                    // should not serialize guid?
                    var categoryGuid = f.CurrentChoice.Value.CategoryId.Item;
                    var chooseSomeCat = FormMessage.NewChooseCategory(categoryGuid);
                    var modeIsChoose = FormMessage.CategoryModeChooseExisting;
                    var trans = FormMessage.NewTransaction(new List<FormMessage> { modeIsChoose, chooseSomeCat });
                    Process(trans);
                }
            }
        }

        private async ValueTask OnClickPostpone() =>
            await _postponeDrawer.Open(ItemModule.commonPostponeChoices, Form.Postpone.IsSome(), Form.ItemName.ValueCommitted);

        private void OnPostponeSelected(int? d)
        {
            if (d is int days)
            {
                Process(FormMessage.NewPostponeSet(days));
            }
            else
            {
                Process(FormMessage.PostponeClear);
            }
        }

        private async ValueTask OnClickStores()
        {
            var model = SelectManyStores.createFromAvailability(Form.Stores);
            await _storesDrawer.Open(model, Form.ItemName.ValueCommitted);
        }

        private void OnStoresSelected(SelectMany<Store> f)
        {
            if (SelectManyModule.hasChanges(f))
            {
                var msg = FormMessage.NewStoresSetAllAvailability(f.Selected);
                Process(msg);
            }
        }

        protected void OnCancel() => OnCancelCallback.InvokeAsync(null);

        protected void OnDelete() => OnDeleteCallback.InvokeAsync(null);

        protected void OnSaveChanges() => OnSaveChangesCallback.InvokeAsync(null);

        private void Process(FormMessage msg) =>
            OnItemFormMessage.InvokeAsync(msg);

        protected void OnToggleComplete()
        {
            HighlightPostpone = !Form.IsComplete;
            Process(FormMessage.ToggleComplete);
        }

        protected bool HighlightPostpone { get; set; }

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
    }
}
