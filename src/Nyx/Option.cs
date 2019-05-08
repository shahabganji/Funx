using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nyx.Helpers;
using Nyx.Option;
using static Nyx.Helpers.OptionHelpers;

namespace Nyx {
    public struct Option<T> {
        private readonly bool _isSome;
        private readonly T _value;

        private Option(T value) {
            _value = value;
            _isSome = true;
        }

        public static implicit operator Option<T>(None _) {
            return new Option<T>();
        }

        public static implicit operator Option<T>(Some<T> some) {
            return new Option<T>(some.Value);
        }

        public static implicit operator Option<T>(T value) {
            return value == null ? (Option<T>) OptionHelpers.None : Some(value);
        }

        public TR Match<TR>(Func<TR> none, Func<T, TR> some) {
            return _isSome ? some(_value) : none();
        }

        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, Task<TR>> someAsync) {
            return _isSome ? someAsync(_value) : noneAsync();
        }

        public IEnumerable<T> AsEnumerable() {
            if (this._isSome)
                yield return this._value;
        }

    }
}