using Funx.Option;

// ReSharper disable once CheckNamespace
namespace Funx
{
    public static class Helpers
    {
        public static None None => None.Default;
        public static Option<T> Some<T>(T value) => new Some<T>(value);
    }
}
