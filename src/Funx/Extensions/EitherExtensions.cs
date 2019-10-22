using System;
using static Funx.Helpers;
using Unit = System.ValueTuple;

namespace Funx.Extensions
{
    public static class EitherExtensions
    {
        public static Either<L, RR> Map<L, R, RR>(this Either<L, R> either, Func<R, RR> f)
            => either.Match<Either<L, RR>>(l => Left(l), r => Right(f(r)));
        
        public static Either<LL, R> MapLeft<L, R, LL>(this Either<L, R> either, Func<L, LL> f)
        => either.Match<Either<LL, R>>(l => Left(f(l)), r => Right(r));

        public static Either<L, RR> Select<L, R, RR>(this Either<L, R> either, Func<R, RR> f)
            => either.Map(f);

        public static Either<L, Unit> ForEach<L, R>(this Either<L, R> either, Action<R> act)
            => either.Map(act.ToFunc());

        public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> either, Func<R, Either<L, RR>> f)
            => either.Match(l => Left(l), f);
    }
}
