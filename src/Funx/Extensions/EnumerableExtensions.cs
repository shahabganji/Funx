using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unit = System.ValueTuple;

namespace Funx.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TR> Map<T, TR>(this IEnumerable<T> @this, Func<T, TR> func)
            => @this.Select(func);

        public static IEnumerable<TR> Bind<T, TR>(this IEnumerable<T> @this, Func<T, IEnumerable<TR>> func)
            => @this.SelectMany(func);

        public static IEnumerable<T> Return<T>(params T[] items)
            => items.ToImmutableList();

        public static void ForEach<T>
            (this IEnumerable<T> ts, Action<T> action)
            => ts.Map(action.ToFunc()).ToImmutableList();

        public static void ForEach<T>
            (this IEnumerable<T> ts, Action<T, long> action)
        {
            var arr = ts as T[] ?? ts.ToArray();
            for (var i = 0; i < arr.LongLength; i++)
            {
                var t = arr[i];
                action(t, i);
            }
        }

        public static bool SafeAny<T>(this IEnumerable<T> @this)
            => @this != null && @this.Any();

        public static bool SafeAny<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
            => @this != null && @this.Any(predicate);
        
        public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> set) =>
            set.Select((value, index) => (index, value));
    }
}
