using System;
using Unit = System.ValueTuple;

namespace Funx.Extensions
{
    public static class ActionExtensions
    {

        public static Func<Unit> ToFunc(this Action @this)
            => () =>
            {
                @this();
                return new Unit();
            };

        public static Func<T, Unit> ToFunc<T>(this Action<T> @this)
            => t =>
            {
                @this(t);
                return new Unit();
            };

        public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> @this)
            => (t1, t2) =>
            {
                @this(t1,t2);
                return new Unit();
            };

        public static Func<T1, T2, T3, Unit> ToFunc<T1, T2, T3>(this Action<T1, T2, T3> @this)
            => (t1, t2, t3) =>
            {
                @this(t1, t2, t3);
                return new Unit();
            };

        public static Func<T1, T2, T3, T4, Unit> ToFunc<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this)
            => (t1, t2, t3, t4) =>
            {
                @this(t1, t2, t3, t4);
                return new Unit();
            };

        public static Func<T1, T2, T3,T4,T5, Unit> ToFunc<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this)
            => (t1, t2, t3, t4, t5) =>
            {
                @this(t1, t2, t3, t4,t5 );
                return new Unit();
            };

        public static Func<T1, T2, T3, T4, T5, T6, Unit> ToFunc<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this)
            => (t1, t2, t3, t4, t5, t6) =>
            {
                @this(t1, t2, t3, t4, t5, t6);
                return new Unit();
            };

        public static Func<T1, T2, T3, T4, T5, T6, T7, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this)
            => (t1, t2, t3, t4, t5, t6, t7) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7);
                return new Unit();
            };

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this)
            => (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8);
                return new Unit();
            };

    }
}
