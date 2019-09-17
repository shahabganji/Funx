using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funx.Extensions
{
    public static class OptionAsyncExtensions
    {
        #region Match: 

        public static async Task<TR> MatchAsync<T, TR>(this Task<Option<T>> @this, Func<TR> none, Func<T, TR> some)
        {
            var taskResult = await @this.ConfigureAwait(false);

            Task<TR> NoneAsync() => Task.FromResult(none());
            Task<TR> SomeAsync(T t) => Task.FromResult(some(t));

            return await taskResult.MatchAsync(NoneAsync, SomeAsync).ConfigureAwait(false);
        }

        public static async Task<TR> MatchAsync<T, TR>(this Task<Option<T>> @this, Func<Task<TR>> noneAsync,
            Func<T, TR> some)
        {
            var taskResult = await @this.ConfigureAwait(false);

            return await taskResult.MatchAsync(noneAsync, some).ConfigureAwait(false);
        }

        public static async Task<TR> MatchAsync<T, TR>(this Task<Option<T>> @this, Func<TR> none,
            Func<T, Task<TR>> someAsync)
        {
            var taskResult = await @this.ConfigureAwait(false);
            return await taskResult.MatchAsync(none, someAsync).ConfigureAwait(false);
        }

        public static async Task<TR> MatchAsync<T, TR>(this Task<Option<T>> @this, Func<Task<TR>> noneAsync,
            Func<T, Task<TR>> someAsync)
        {
            var taskResult = await @this.ConfigureAwait(false);

            return await taskResult.MatchAsync(noneAsync, someAsync).ConfigureAwait(false);
        }

        #endregion
    }
}