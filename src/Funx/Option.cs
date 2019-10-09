using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funx.Extensions;
using Funx.Option;
using Unit = System.ValueTuple;


namespace Funx
{
    public struct Option<T> : IEquatable<None>, IEquatable<Option<T>>
    {
        public static Option<T> None => Helpers.None;
        public static Option<T> Some(T value) => Helpers.Some(value);

        private readonly bool _isSome;
        private readonly T _value;

        public bool IsNone => !_isSome;

        private Option(T value)
        {
            _value = value;
            _isSome = true;
        }

        public static implicit operator Option<T>(T value)
            => value == null ? Helpers.None : Some(value);
        
        public static implicit operator Option<T>(None _) => new Option<T>();
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);

        public TR Match<TR>(Func<TR> none, Func<T, TR> some)
            => _isSome ? some(_value) : none();
        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, Task<TR>> someAsync)
            => _isSome ? someAsync(_value) : noneAsync();
        public Task<TR> MatchAsync<TR>(Func<TR> none, Func<T, Task<TR>> someAsync)
        {
            Task<TR> AdapterNoneAsync() => Task.FromResult(none());

            return this.MatchAsync(AdapterNoneAsync, someAsync);
        }

        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, TR> some)
        {
            Task<TR> AdapterSomeAsync(T t) => Task.FromResult(some(t));

            return this.MatchAsync(noneAsync, AdapterSomeAsync);
        }

        public Unit Match<TR>(Action none, Action<T> some)
            => this.Match(none.ToFunc(), some.ToFunc());


        public IEnumerable<T> AsEnumerable()
        {
            if (this._isSome)
                yield return this._value;
        }

        #region Equality methods:

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
