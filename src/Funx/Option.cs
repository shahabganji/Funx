using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        private readonly T _value;

        public bool IsSome { get; }
        public bool IsNone => !IsSome;

        private Option(T value)
        {
            _value = value;
            IsSome = true;
        }

        public static implicit operator Option<T>(T value)
            => value == null ? Helpers.None : Some(value);
        
        public static implicit operator Option<T>(None _) => new Option<T>();
        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);

        public TR Match<TR>(Func<TR> none, Func<T, TR> some)
            => IsSome ? some(_value) : none();
        public Task<TR> MatchAsync<TR>(Func<Task<TR>> noneAsync, Func<T, Task<TR>> someAsync)
            => IsSome ? someAsync(_value) : noneAsync();
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
        

        public Option<T> WhenNone(Action none)
        {
            if (this.IsNone) none();
            return this;
        }
        public async Task<Option<T>> WhenNoneAsync(Func<Task> noneAsync)
        {
            if (this.IsNone)
                await noneAsync().ConfigureAwait(false);
            
            return this;
        }
        
        public Option<T> WhenSome(Action<T> some)
        {
            if (this.IsSome) some(this._value);
            return this;
        }

        public async Task<Option<T>> WhenSomeAsync(Func<T, Task> someAsync)
        {
            if( this.IsSome)
                await someAsync(this._value).ConfigureAwait(false);

            return this;
        }

        // ToDo: add unit tests
        public Either<L, T> ToEither<L>(Func<L> leftFactory)
            => this.Match<Either<L,T>>(() => leftFactory(), v => v);
        

        public IEnumerable<T> AsEnumerable()
        {
            if (this.IsSome)
                yield return this._value;
        }

        #region Equality methods:

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
            => this.IsSome ? $"Some({_value})" : "None";
    }
}
