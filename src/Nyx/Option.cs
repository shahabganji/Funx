using System;
using System.Threading.Tasks;
using Nyx.Helpers;
using Nyx.Option;
using static Nyx.Helpers.OptionHelpers;

namespace Nyx
{
    public class Option<T>
    {
        private readonly bool _isSome;

        private readonly T _value;

        private Option()
        {
        }

        private Option(T value)
        {
            _value = value;
            _isSome = true;
        }

        public static implicit operator Option<T>(None _)
        {
            return new Option<T>();
        }

        public static implicit operator Option<T>(Some<T> some)
        {
            return new Option<T>(some.Value);
        }

        public static implicit operator Option<T>(T value)
        {
            return value == null ? (Option<T>) OptionHelpers.None : Some(value);
        }

        public TR Match<TR>(Func<TR> none, Func<T, TR> some)
        {
            return _isSome ? some(_value) : none();
        }

        public Task<R> MatchAsync<R>(Func<Task<R>> none, Func<T, Task<R>> some)
        {
            return _isSome ? some(_value) : none();
        }
    }
}