using Funx.Option;
using Funx.Either;
using Funx.Exceptional;

// ReSharper disable once CheckNamespace
namespace Funx
{
    public static class Helpers
    {
        // Factory methods for Option<T>
        public static None None => None.Default;
        public static Option<T> Some<T>(T value) => new Some<T>(value);


        // Factory methods for Either<L,R>
        public static Left<L> Left<L>(L l) => new Left<L>(l);
        public static Right<R> Right<R>(R r) => new Right<R>(r);


        // Factory methods for Exceptional<T>
        public static Exceptional<T> Success<T>(T data) => new Success<T>(data);

        // TODO: Factory methods for Validation<T>: Valid, Invalid
    }
}
