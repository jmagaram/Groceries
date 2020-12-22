using Microsoft.AspNetCore.Components;

namespace WebApp.Shared {
    public partial class ElixDialog {
        bool _isOpen;

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
                }
            }
        }
    }
}
