using System;
using Unit = System.ValueTuple;

namespace Nyx.Extensions
{
    public static class ActionExtensions
    {
        public static Func<Unit> ToFunc(this Action action)
            => () =>
            {
                action();
                return new Unit();
            };
        
        public static Func<T1, Unit> ToFunc<T1>(this Action<T1> action)
            => t1 =>
            {
                action(t1);
                return new Unit();
            };
    }
}
