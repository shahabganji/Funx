using System;
using System.Threading.Tasks;
using static Nyx.Helpers.OptionHelpers;

namespace Nyx {
    public class Option<T> {

        readonly T _value;
        readonly bool _isSome;

        private Option( ) { }

        private Option( T value ) {
            this._value = value;
            this._isSome = true;
        }

        public static implicit operator Option<T>( Option.None _ ) => new Option<T>( );

        public static implicit operator Option<T>( Option.Some<T> some ) => new Option<T>( some.Value );

        public static implicit operator Option<T>( T value )
            => value == null ? (Option<T>)None : Some( value );

        public R Match<R>( Func<R> none, Func<T, R> some ) => _isSome ? some( this._value ) : none( );
        public Task<R> Match<R>( Func<Task<R>> none, Func<T, Task<R>> some ) => _isSome ? some( this._value ) : none( );

    }
}
