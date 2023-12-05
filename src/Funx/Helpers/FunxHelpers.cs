using Funx.Option;
using Funx.Either;

// ReSharper disable once CheckNamespace
namespace Funx
{
    public static class Factories
    {
        public static None None => None.Default;
        public static Option<T> Some<T>(T value) => new Some<T>(value);

        
        public static Left<L> Left<L>(L l) => new(l);
        public static Right<R> Right<R>(R r) => new(r);
    }
}
