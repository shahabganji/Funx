using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Funx.Helpers;
using Unit = System.ValueTuple;

namespace Funx.Extensions
{
    public static class OptionExtensions
    {
        public static Option<TR> Map<T, TR>(this Option<T> option, Func<T, TR> func) =>
            option.Match(
                () => None,
                value => Some(func(value))
            );

        public static Option<TR> Select<T, TR>(this Option<T> option, Func<T, TR> func) =>
            option.Map(func);

        public static Option<TR> SelectMany<T, TR>(this Option<T> option, Func<T, Option<TR>> func) =>
            option.Bind(func);

        public static Option<TR> Bind<T, TR>(this Option<T> option, Func<T, Option<TR>> func)
            => option.Match(() => None, func);

        public static Task<Option<TR>> MapAsync<T, TR>(this Option<T> option, Func<T, Task<TR>> funcAsync)
        {
            var tcs = new TaskCompletionSource<Option<TR>>();

            Task.Run(async () =>
            {
                try
                {
                    var result = await option.MatchAsync(
                        () => None,
                        async t =>
                        {
                            var mapped = await funcAsync(t).ConfigureAwait(false);
                            return Some(mapped);
                        }).ConfigureAwait(false);

                    tcs.SetResult(result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        public static Task<Option<TR>> BindAsync<T, TR>(this Option<T> @this, Func<T, Task<Option<TR>>> funcAsync)
            => @this.MatchAsync(() => None, funcAsync);
        
        // this is exactly MapAsync
//        public static Task<Option<TR>> BindAsync<T, TR>(this Option<T> option, Func<T, Task<TR>> funcAsync)
//        {
//            async Task<Option<TR>> AdapterFuncAsync(T t)
//            {
//                return Some(await funcAsync(t).ConfigureAwait(false));
//            }
//
//            return option.BindAsync(AdapterFuncAsync);
//        }

        public static Option<T> Where<T>(this Option<T> @this, Func<T, bool> predicate)
            => @this.Match(() => None, t => predicate(t) ? Some(t) : None);

        public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action)
            => @this.Map(action.ToFunc());

        public static IEnumerable<TR> Bind<T, TR>(this IEnumerable<T> list, Func<T, Option<TR>> func)
            => list.Bind(x => func(x).AsEnumerable());

        public static IEnumerable<TR> Bind<T, TR>(this Option<T> option, Func<T, IEnumerable<TR>> func)
            => option.AsEnumerable().Bind(func);

    }
}