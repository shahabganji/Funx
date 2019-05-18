using System;
using System.Threading.Tasks;
using static Nyx.Helpers.OptionHelpers;
using Unit = System.ValueTuple;

namespace Nyx.Extensions {
    public static class OptionExtensions {
        public static Option<TR> Map<T, TR>(this Option<T> option, Func<T, TR> func) => option.Match(
            () => None,
            value => Some(func(value))
        );

        public static Task<Option<TR>> MapAsync<T, TR>(this Option<T> option, Func<T, Task<TR>> funcAsync) {

            TaskCompletionSource<Option<TR>> tcs = new TaskCompletionSource<Option<TR>>();

            Task.Run(async() => {
                try {
                    var result = await option.MatchAsync(
                        () => Task.FromResult<Option<TR>>(None),
                        async t => {
                            var mapped = await funcAsync(t).ConfigureAwait(false);
                            return Some(mapped);
                        }).ConfigureAwait(false);

                    tcs.SetResult(result);
                } catch (Exception e) {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        public static Option<T> Where<T>(this Option<T> @this, Func<T, bool> predicate) => @this.Match(() => None, t => predicate(t) ? Some(t) : None);

        public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action) => @this.Map(action.ToFunc());

        public static Option<TR> Bind<T, TR>(this Option<T> option, Func<T, Option<TR>> func) => option.Match(() => None, func);

        public static Option<TR> Bind<T, TR>(this Option<T> option, Func<T, TR> func) => option.Match(() => None, x => Some(func(x)));
    }
}
