using Microsoft.FSharp.Core;
using System;
using System.Runtime.CompilerServices;
using WebApp.Shared;

namespace WebApp.Common
{
    public static class NullableExtensionMethods
    {
        public static U? Map<T, U>(this T? item, Func<T, U> mapper) where T : struct where U : struct =>
            item.HasValue ? new U?(mapper(item.Value)) : new U?();

        public static FSharpOption<T> ToFSharpOption<T>(this T? item) where T : struct =>
            item.HasValue ? FSharpOption<T>.Some(item.Value) : FSharpOption<T>.None;

        public static FSharpOption<T> ToFSharpOption<T>(this T item) where T : class =>
            item is null ? FSharpOption<T>.None : FSharpOption<T>.Some(item);
    }
}
