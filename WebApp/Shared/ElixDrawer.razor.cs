using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WebApp.Shared {
    public partial class ElixDrawer {
        ElementReference _drawer;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public ElixDrawerEdge FromEdge { get; set; } = ElixDrawerEdge.Start;

        public async ValueTask Open() =>
            await JSRuntime.InvokeVoidAsync("HtmlElement.setProperty", _drawer, "opened", true);

        public async ValueTask Close() =>
            await JSRuntime.InvokeVoidAsync("HtmlElement.setProperty", _drawer, "opened", false);
    }
}
