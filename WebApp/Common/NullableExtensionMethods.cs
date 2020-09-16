using System;

namespace WebApp.Common {
    public static class NullableExtensionMethods {
        public static U? Map<T, U>(this T? item, Func<T, U> mapper) where T : struct where U : struct =>
            item.HasValue ? new U?(mapper(item.Value)) : new U?();
    }
}
