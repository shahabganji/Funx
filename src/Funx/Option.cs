using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funx.Option;
using static Funx.Helpers;

using Unit = System.ValueTuple;


namespace Funx
{
    public struct Option<T> : IEquatable<None>, IEquatable<Option<T>>
    {
        private readonly bool _isSome;
        private readonly T _value;

        private bool IsNone => !_isSome;

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
            return value == null ? Helpers.None : Some(value);
        }

        public TR Match<TR>(Func<TR> none, Func<T, TR> some)
            => _isSome ? some(_value) : none();
        
        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, Task<TR>> someAsync)
            => _isSome ? someAsync(_value) : noneAsync();

        public Task<TR> MatchAsync<TR>(Func<TR> none, Func<T, Task<TR>> someAsync)
        {
            Task<TR> AdapterNoneAsync() => Task.FromResult(none());

            return this.MatchAsync(AdapterNoneAsync, someAsync);
        }

        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T,TR> some)
        {
            Task<TR> AdapterSomeAsync(T t) => Task.FromResult(some(t));

            return this.MatchAsync(noneAsync, AdapterSomeAsync);
        }


        public IEnumerable<T> AsEnumerable()
        {
            if (this._isSome)
                yield return this._value;
        }

        #region Equality methds:

        public bool Equals(None other) => IsNone;

        public bool Equals(Option<T> other)
            => _isSome == other._isSome && (IsNone || this._value.Equals(other._value));

        public override bool Equals(object obj)
        {
            return obj is Option<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_isSome.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(_value);
            }
        }

        #endregion

        // TODO: Add unit tests for operators
        #region Operators:

        public static bool operator ==(Option<T> @this, Option<T> other)
            => @this.Equals(other);

        public static bool operator !=(Option<T> @this, Option<T> other)
            => !(@this == other);

        public static bool operator ==(Option<T> @this, T other)
            => @this.Equals(other);

        public static bool operator !=(Option<T> @this, T other)
            => !(@this == other);

        public static bool operator ==(T other, Option<T> @this)
            => @this.Equals(other);

        public static bool operator !=(T other, Option<T> @this)
            => !(@this == other);

        #endregion

        public override string ToString()
            => this._isSome ? $"Some({_value})" : "None";
    }
}