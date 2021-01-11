using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using GroceriesWasmApp.Client.Common;

namespace GroceriesWasmApp.Client.Shared
{
    public partial class ElixDrawer : ComponentBase, IDisposable, IAsyncDisposable
    {
#pragma warning disable IDE0044 // Add readonly modifier
        ElementReference _drawer;
#pragma warning restore IDE0044 // Add readonly modifier
        IJSObjectReference _elixModule;
        IJSObjectReference _generalModule;
        DotNetObjectReference<CallbackHelper<bool>> _openedChangeCallbackReference;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _elixModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/elix.js").AsTask();
                _generalModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/general.js").AsTask();
                var _openedChangeCallback = new CallbackHelper<bool>(OnOpenedChange);
                _openedChangeCallbackReference = DotNetObjectReference.Create(_openedChangeCallback);
                await _elixModule.InvokeVoidAsync(
                    "OpenCloseMixinAddOpenedChangeEventListener",
                    _drawer,
                    "WebApp",
                    nameof(_openedChangeCallback.Invoke),
                    _openedChangeCallbackReference);
            }
        }

        protected Task OnOpenedChange(bool isOpen) => 
            !isOpen ? Closed.InvokeAsync() : Task.CompletedTask;

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
        [Parameter]
        public EventCallback Closed { get; set; }

        public async ValueTask Open() =>
            await _generalModule.InvokeVoidAsync("setProperty", _drawer, "opened", true);

        public async ValueTask Close() =>
            await _generalModule.InvokeVoidAsync("setProperty", _drawer, "opened", false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _openedChangeCallbackReference?.Dispose();
                (_elixModule as IDisposable)?.Dispose();
                (_generalModule as IDisposable)?.Dispose();
            }
            _openedChangeCallbackReference = null;
            _elixModule = null;
            _generalModule = null;
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_elixModule is not null)
            {
                await _elixModule.DisposeAsync().ConfigureAwait(false);
                _elixModule = null;
            }
            if (_generalModule is not null)
            {
                await _generalModule.DisposeAsync().ConfigureAwait(false);
                _generalModule = null;
            }
            _openedChangeCallbackReference?.Dispose();
            _openedChangeCallbackReference = null;
        }
    }
}
