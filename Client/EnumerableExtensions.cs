using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    public static class EnumerableExtensions
    {
        public static T FirstOr<T>(this IEnumerable<T> items, T ifEmpty) =>
            items.Append(ifEmpty).First();
    }
}
