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

        public static IEnumerable<Unit> ForEach<T>
            (this IEnumerable<T> ts, Action<T> action)
            => ts.Map(action.ToFunc()).ToImmutableList();
    }
}
