using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Models;
using static Models.CoreTypes;
using WebApp.Common;
using FormMessage = Models.ItemFormModule.Message;
using TextBoxMessage = Models.CoreTypes.TextBoxMessage;

namespace WebApp.Shared
{
    public partial class ItemEditFormSlick : ComponentBase
    {
        private CategoryDrawer _categoryDrawer;
        private PostponeDrawer _postponeDrawer;
        private StoresDrawer _storesDrawer;

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

        private async ValueTask OnClickCategory() =>
            await _categoryDrawer.Open(SelectZeroOrOneCategory.createFromPickList(Form.CategoryChoice, Form.CategoryChoiceList));

        private void OnCategorySelected(SelectZeroOrOneModule.SelectZeroOrOne<Category> f)
        {
            if (f.HasChanges)
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
            await _postponeDrawer.Open(SelectZeroOrOnePostpone.createFromRelativeDate(Form.Postpone));

        private void OnPostponeSelected(SelectZeroOrOneModule.SelectZeroOrOne<int> f)
        {
            if (f.HasChanges)
            {
                if (f.CurrentChoice.IsNone())
                {
                    Process(FormMessage.PostponeClear);
                }
                else
                {
                    Process(FormMessage.NewPostponeSet(f.CurrentChoice.Value));
                }
            }
        }

        private async ValueTask OnClickStores()
        {
            var model = StoresPickerModule.createFromAvailability(Form.Stores);
            await _storesDrawer.Open(model);
        }

        private void OnStoresSelected(SelectMany<Store> f)
        {
            if (f.HasChanges)
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
