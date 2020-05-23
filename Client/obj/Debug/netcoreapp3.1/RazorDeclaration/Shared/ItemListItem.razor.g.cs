#pragma checksum "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3efc01f035e524072328e9e31d09ab6beddaef73"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
        }
        #pragma warning restore 1998
#nullable restore
#line 9 "C:\Users\justi\source\repos\Groceries\Client\Shared\ItemListItem.razor"
       
    [Parameter]
    public DomainTypes.Item Item { get; set; }

    public enum When
    {
        Overdue,
        Today,
        Later
    }

    private int DaysUntil(DateTime now, DateTime later)
    {
        var totalDays = (later - now).TotalDays;
        return Convert.ToInt32(Math.Round(totalDays));
    }

    private string PostponeDateToString(DateTime now, DateTime later)
    {
        var daysAway = DaysUntil(now, later);
        if (daysAway == 0)
        {
            return "Due Today";
        }
        else if (daysAway < 0)
        {
            return "Overdue";
        }
        else
        {
            return $"+ {daysAway} days";
        }
    }

    public string ScheduleText()
    {
        var now = DateTime.Now;
        if (Item.Schedule.IsComplete)
        {
            return "Complete";
        }
        else if (Item.Schedule.IsIncomplete)
        {
            return "Incomplete";
        }
        else if (Item.Schedule is DomainTypes.Schedule.Postponed p)
        {
            return PostponeDateToString(now, p.Item);
        }
        else if (Item.Schedule is DomainTypes.Schedule.Repeat r)
        {
            if (r.Item.PostponedUntil.IsNone())
            {
                return "Active";
            }
            else
            {
                return PostponeDateToString(now, r.Item.PostponedUntil.Value);
            }
        }
        else throw new NotImplementedException();
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
