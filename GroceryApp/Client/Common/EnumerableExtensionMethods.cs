using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GroceryApp.Common
{
    public static class EnumerableExtensionMethods
    {
        public static T FirstOr<T>(this IEnumerable<T> items, T otherwise)
        {
            var (item, exists) = items.Select(i => (i, true)).FirstOrDefault();
            return exists ? item : otherwise;
        }
    }
}
