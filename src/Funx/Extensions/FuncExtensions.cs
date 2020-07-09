using System;

namespace Funx.Extensions
{
    public static class FuncExtensions
    {
        public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> func, T1 t)
            => t2 => func(t, t2);

        public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 t1)
            => (t2, t3) => func(t1, t2, t3);

        public static Func<T2, T3, T4, R> Apply<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> func, T1 t1)
            => (t2, t3, t4) => func(t1, t2, t3, t4);


        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func)
            => t1 => t2 => func(t1, t2);

        public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> func)
            => t1 => t2 => t3 => func(t1, t2, t3);

        public static Func<T1, Func<T2, Func<T3, Func<T4, R>>>> Curry<T1, T2, T3, T4, R>(
            this Func<T1, T2, T3, T4, R> func)
            => t1 => t2 => t3 => t4 => func(t1, t2, t3, t4);
    }
}
