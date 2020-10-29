using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace WebApp.Shared {
    public partial class ElixDialog {

        bool _isOpen;
        bool _isOpenChanged = false;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen != value) {
                    _isOpen = value;
                    _isOpenChanged = true;
                    StateHasChanged();
                }
            }
        }

        [Parameter]
        public EventCallback<bool> IsOpenChanged { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (_isOpenChanged) {
                _isOpenChanged = false;
                await IsOpenChanged.InvokeAsync(IsOpen);
            }
        }
    }
}
