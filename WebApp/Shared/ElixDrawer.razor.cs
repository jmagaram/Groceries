using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebApp.Common;

namespace WebApp.Shared {
    public partial class ElixDrawer : IDisposable {
        ElementReference _drawer;
        DotNetObjectReference<CallbackHelper<bool>> _openedChangeCallbackReference;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                var _openedChangeCallback = new CallbackHelper<bool>(OnOpenedChange);
                _openedChangeCallbackReference = DotNetObjectReference.Create(_openedChangeCallback);
                await JSRuntime.InvokeVoidAsync("ElixOpenCloseMixin.addOpenedChangeEventListener",
                    _drawer,
                    "WebApp",
                    "Invoke",
                    _openedChangeCallbackReference);
            }
        }

        protected Task OnOpenedChange(bool isOpen) {
            if (!isOpen) {
                return Closed.InvokeAsync();
            }
            else {
                return Task.CompletedTask;
            }
        }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public ElixDrawerEdge FromEdge { get; set; } = ElixDrawerEdge.Bottom;

        /// <summary>
        /// Raised when the Drawer is closed by (1) clicking on the overlay, (2)
        /// pressing ESC, or (3) dragging the drawer closed. Not raised as a
        /// result of code that calls the Close() method.
        /// </summary>
        /// <remarks>
        /// Can't get reliably notified of Open and Close events. The only
        /// notification that works is getting a Close notification if the user
        /// clicks outside the Drawer on the overlay or presses ESC or drags it
        /// closed. No other events are raised properly.
        /// </remarks>
        [Parameter]
        public EventCallback Closed { get; set; }

        public async ValueTask Open() =>
            await JSRuntime.InvokeVoidAsync("HtmlElement.setProperty", _drawer, "opened", true);

        public async ValueTask Close() =>
            await JSRuntime.InvokeVoidAsync("HtmlElement.setProperty", _drawer, "opened", false);

        public void Dispose() {
            _openedChangeCallbackReference?.Dispose();
            _openedChangeCallbackReference = null;
        }
    }
}
