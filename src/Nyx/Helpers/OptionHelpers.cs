namespace Nyx.Helpers {
    public static partial class OptionHelpers {
        public static Option.None None => Option.None.Default;
        public static Option.Some<T> Some<T>(T value) => new Nyx.Option.Some<T>(value);
    }
}