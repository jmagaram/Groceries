using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WebApp.Common {
    public static class ElementExtensions {
        public static async Task FocusAsync(this ElementReference elementRef, IJSRuntime jsRuntime) {
            await jsRuntime.InvokeVoidAsync(
                "interopFunctions.focusElement", elementRef);
        }
    }
}
