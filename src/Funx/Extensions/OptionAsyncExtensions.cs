using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Funx.Helpers;


namespace Funx.Extensions
{
    public static class OptionAsyncExtensions
    {
        #region Match: 

        public static async Task<TR> MatchAsync<T, TR>(this Task<Option<T>> @this, Func<TR> none, Func<T, TR> some)
        {
            var taskResult = await @this.ConfigureAwait(false);

            Task<TR> AdapterNoneAsync() => Task.FromResult(none());
            Task<TR> AdapterSomeAsync(T t) => Task.FromResult(some(t));

            return await taskResult.MatchAsync(AdapterNoneAsync, AdapterSomeAsync).ConfigureAwait(false);
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

        public static Task<Option<TR>> BindAsync<T, TR>(
            this Task<Option<T>> @this, Func<T, Task<Option<TR>>> funcAsync)
            => @this.MatchAsync(() => None, funcAsync);

        public static Task<Option<TR>> MapAsync<T, TR>(this Task<Option<T>> @this, Func<T, Task<TR>> funcAsync)
        {
            async Task<Option<TR>> AdapterFuncAsync(T t)
            {
                return Some(await funcAsync(t).ConfigureAwait(false));
            }

            return @this.BindAsync(AdapterFuncAsync);
        }

        // ReSharper disable once AsyncConverter.AsyncMethodNamingHighlighting
        public static Task<Option<TR>> Select<T, TR>(this Task<Option<T>> @this, Func<T, Task<TR>> funcAsync)
            => @this.MapAsync(funcAsync);

        // ReSharper disable once AsyncConverter.AsyncMethodNamingHighlighting
        public static async Task<Option<T>> Where<T>(this Task<Option<T>> @this, Func<T, bool> predicate)
        {
            var taskResult = await @this.ConfigureAwait(false);

            Task<Option<T>> NoneAsync() => Task.FromResult((Option<T>)None);

            Task<Option<T>> SomeAsync(T t) => predicate(t) ? Task.FromResult(Some(t)) : Task.FromResult((Option<T>)None);

            return await @this.MatchAsync(NoneAsync, SomeAsync).ConfigureAwait(false);
        }

    }
}
