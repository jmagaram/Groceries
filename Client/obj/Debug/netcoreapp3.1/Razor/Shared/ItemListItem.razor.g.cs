#pragma checksum "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b7301314c444275d2ff30be66a7bd83b65d09ebd"
// <auto-generated/>
#pragma warning disable 1591
namespace Client.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\justi\source\repos\Groceries\Client\_Imports.razor"
using Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
using Microsoft.FSharp.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
using Client.Shared;

#line default
#line hidden
#nullable disable
    public partial class ItemListItem : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "p");
            __builder.OpenElement(1, "span");
            __builder.AddAttribute(2, "style", "font-weight:bold");
            __builder.AddContent(3, 
#nullable restore
#line 3 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
                                   Item.Title.Item

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 3 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
                                                          if (Item.Quantity.IsSome())
{

#line default
#line hidden
#nullable disable
            __builder.OpenElement(4, "span");
            __builder.AddContent(5, " * ");
            __builder.AddContent(6, 
#nullable restore
#line 4 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
           Item.Quantity.Value.Item

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 4 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
                                          }

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
                                            if (Item.Note.IsSome())
{

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(7, "<br>");
            __builder.OpenElement(8, "span");
            __builder.AddContent(9, 
#nullable restore
#line 5 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
             Item.Note.Value.Item

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 5 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
                                        }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 7 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
       
    [Parameter]
    public DomainTypes.Item Item { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
