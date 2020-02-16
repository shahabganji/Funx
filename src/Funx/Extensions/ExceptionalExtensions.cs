using System;
using Unit = System.ValueTuple;
using static Funx.Helpers;

namespace Funx.Extensions
{
    public static class ExceptionalExtensions
    {
        public static Exceptional<R> Map<T, R>(this Exceptional<T> exp, Func<T, R> f)
            => exp.Match(Exception<R>, data => f(data));
        
        public static Exceptional<R> Select<T,R>(this Exceptional<T> exp, Func<T, R> f)
            => exp.Map(f);

        public static Exceptional<Unit> ForEach<T>(this Exceptional<T> exp, Action<T> act)
            => exp.Map(act.ToFunc());

        public static Exceptional<R> Bind<T, R>(this Exceptional<T> exp, Func<T, Exceptional<R>> f)
            => exp.Match(Exception<R>, f);
    }
}
