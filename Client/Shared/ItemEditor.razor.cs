using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomainTypes;

namespace Client.Shared
{
    public partial class ItemEditor : ComponentBase
    {
        [Parameter]
        public DomainTypes.ItemEditorModel Model { get; set; }

        protected TextBoxHandler<DomainTypes.ItemEditorModel, ItemEditorMessage> _title;
        protected TextBoxHandler<DomainTypes.ItemEditorModel, ItemEditorMessage> _quantity;
        protected TextBoxHandler<DomainTypes.ItemEditorModel, ItemEditorMessage> _note;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TextBoxHandler<DomainTypes.ItemEditorModel, ItemEditorMessage> createHandler(Func<TextBoxMessage, ItemEditorMessage> msg) =>
                new TextBoxHandler<DomainTypes.ItemEditorModel, ItemEditorMessage>(
                    () => Model,
                    m => Model = m,
                    msg,
                    global::ItemEditorModel.update);

            var x = ItemEditorMessage.NewTitleMessage(TextBoxMessage.GetFocus);
            _title = createHandler(ItemEditorMessage.NewTitleMessage);
            _note = createHandler(ItemEditorMessage.NewNoteMessage);
            _quantity = createHandler(ItemEditorMessage.NewQuantityMessage);
        }

        protected void OnRepeatChange(ChangeEventArgs e)
        {
            var selectedKey = (string)(e.Value);
            var selected = global::Repeat.deserialize(selectedKey).ResultValue;
            var msg = DomainTypes.ItemEditorMessage.NewRepeatMessage(selected);
            Model = global::ItemEditorModel.update(msg, Model);
        }

        protected string RepeatAsText(DomainTypes.Repeat r) => global::Repeat.formatEnglish.Invoke(r);

        protected void IncreaseQuantity() =>
            Model = global::ItemEditorModel.update(ItemEditorMessage.NewQuantitySpinner(SpinnerMessage.Increase), Model);

        protected void DecreaseQuantity() =>
            Model = global::ItemEditorModel.update(ItemEditorMessage.NewQuantitySpinner(SpinnerMessage.Decrease), Model);
    }

    public class TextBoxHandler<TModel, TMessage>
    {
        Func<TModel> _getModel;
        Action<TModel> _setModel;
        Func<TextBoxMessage, TMessage> _msg;
        Func<TMessage, TModel, TModel> _update;

        public TextBoxHandler(Func<TModel> getModel, Action<TModel> setModel, Func<TextBoxMessage, TMessage> msg, Func<TMessage, TModel, TModel> update)
        {
            _getModel = getModel;
            _setModel = setModel;
            _msg = msg;
            _update = update;
        }

        public void OnTextChange(ChangeEventArgs e) => ProcessMessage(global::DomainTypes.TextBoxMessage.NewSetText(e.Value.ToString()));

        public void OnFocusIn(FocusEventArgs e) => ProcessMessage(global::DomainTypes.TextBoxMessage.GetFocus);

        public void OnFocusOut(FocusEventArgs e) => ProcessMessage(global::DomainTypes.TextBoxMessage.LoseFocus);

        protected void ProcessMessage(TextBoxMessage msg)
        {
            var modelMessage = _msg(msg);
            var model = _getModel();
            var updatedModel = _update(modelMessage, model);
            _setModel(updatedModel);
        }
    }
}
