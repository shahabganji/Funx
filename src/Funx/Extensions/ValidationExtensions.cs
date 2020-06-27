using System;
using static Funx.Helpers;

namespace Funx.Extensions
{
    public static class ValidationExtensions
    {
        public static Validation<R> Map<T,R>(this Validation<T> validation, Func<T, R> f)
            => validation.Match(
                l => l, 
                r => Valid(f(r)));
        
        public static Validation<R> Select<T,R>(this Validation<T> validation, Func<T, R> f)
            => validation.Map(f);

        public static void ForEach<T>(this Validation<T> validation, Action<T> act)
            => validation.Map(act.ToFunc());

        public static Validation<R> Bind<T,R>(this Validation<T> validation, Func<T, Validation<R>> f)
            => validation.Match(
                l => l,
                f);

        public static Option<R> Bind<T,R>(this Validation<T> validation, Func<T, Option<R>> f)
            => validation.Match(
                _ => None, 
                f);
        
        public static Result<T> ToResult<T>(this Validation<T> validation)
            => validation.Match<Result<T>>(
                error => error,
                data => data);
    }
}
