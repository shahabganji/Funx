﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funx.Option;
using Unit = System.ValueTuple;

namespace Funx
{
    public readonly struct Option<T> : IEquatable<None>, IEquatable<Option<T>>
    {
        public static Option<T> None => Factories.None;
        public static Option<T> Some(T value) => Factories.Some(value);
        
        private readonly T _value;
        public T UnwrappedValue => this.IsNone
            ? throw new InvalidOperationException(
                $"{nameof(Unwrap)} without providing default value can only be called on " +
                $"Options with a not `None` values")
            : this._value;

        public bool IsSome { get; }
        public bool IsNone => !IsSome;

        private Option(T value)
        {
            _value = value;
            IsSome = true;
        }

        public static implicit operator Option<T>(T value)
            => value == null ? Factories.None : Some(value);

        public static implicit operator Option<T>(None _) => new();
        public static implicit operator Option<T>(Some<T> some) => new(some.Value);

        public TR Match<TR>(Func<TR> none, Func<T, TR> some)
            => IsSome ? some(_value) : none();

        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, Task<TR>> someAsync)
            => IsSome ? someAsync(_value) : noneAsync();

        public Task<TR> MatchAsync<TR>(Func<TR> none, Func<T, Task<TR>> someAsync)
        {
            return this.MatchAsync(AdapterNoneAsync, someAsync);

            Task<TR> AdapterNoneAsync() => Task.FromResult(none());
        }

        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, TR> some)
        {
            return this.MatchAsync(noneAsync, AdapterSomeAsync);

            Task<TR> AdapterSomeAsync(T t) => Task.FromResult(some(t));
        }


        public Option<T> WhenNone(Action none)
        {
            if (this.IsNone) none();
            return this;
        }

        public async Task WhenNoneAsync(Func<Task> noneAsync)
        {
            if (this.IsNone)
                await noneAsync().ConfigureAwait(false);
        }

        public void WhenSome(Action<T> some)
        {
            if (this.IsSome) some(this._value);
        }

        public async Task WhenSomeAsync(Func<T, Task> someAsync)
        {
            if (this.IsSome)
                await someAsync(this._value).ConfigureAwait(false);
        }

        public Either<L, T> ToEither<L>(Func<L> leftFactory)
            => this.Match<Either<L, T>>(() => leftFactory(), v => v);
        
        public IEnumerable<T> AsEnumerable()
        {
            if (this.IsSome)
                yield return this._value;
        }

        public T Unwrap(T defaultValue = default) => this.IsNone ? defaultValue : this._value;
        public T Unwrap(Func<T> defaultValueFunc) => this.IsNone ? defaultValueFunc() : this._value;

        public Task<T> UnwrapAsync(Func<Task<T>> defaultValueFuncAsync)
            => this.IsNone
                ? defaultValueFuncAsync()
                : Task.FromResult(this._value);

        public bool Equals(None other) => IsNone;

        public bool Equals(Option<T> other)
            => IsSome == other.IsSome && (IsNone || this._value.Equals(other._value));

        public override bool Equals(object obj)
        {
            return obj is Option<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (IsSome.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(_value);
            }
        }

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
            => this.IsSome ? $"Some({_value})" : "None";
    }
}
