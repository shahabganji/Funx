using Nyx.Option;

namespace Nyx.Helpers
{
    public static class OptionHelpers
    {
        public static None None => None.Default;
        public static Option<T> Some<T>(T value) => new Some<T>(value);
    }
}
