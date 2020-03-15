using System;
using static Funx.Helpers;
using Unit = System.ValueTuple;

namespace Funx.Extensions
{
    public static class ExceptionalExtensions
    {
        public static Exceptional<R> Map<T, R>(this Exceptional<T> exp, Func<T, R> f)
            => exp.Match<Exceptional<R>>(ex => ex, data => f(data));
        
        public static Exceptional<R> Select<T,R>(this Exceptional<T> exp, Func<T, R> f)
            => exp.Map(f);

        public static void ForEach<T>(this Exceptional<T> exp, Action<T> act)
            => exp.Map(act.ToFunc());

        public static Exceptional<R> Bind<T, R>(this Exceptional<T> exp, Func<T, Exceptional<R>> f)
            => exp.Match(
                exception => exception,
                f);
        
        public static Option<R> Bind<T, R>(this Exceptional<T> exp, Func<T, Option<R>> f)
            => exp.Match(
                exception => None,
                f);

        public static  Result<T> ToResult<T>(this Exceptional<T> exceptional)
            => exceptional.Match<Result<T>>(exp => new Error(exp.Message), data => data);
    }
}
