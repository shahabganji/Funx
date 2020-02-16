using System;
using System.Threading.Tasks;
using Funx.Exceptional;
using Funx.Extensions;
using Unit = System.ValueTuple;

namespace Funx
{
    public struct Exceptional<T>
    {
        private readonly Exception _exception;
        private readonly T _value;

        public bool IsSuccess { get;  }
        public bool IsException => !IsSuccess;

        private Exceptional(Exception exception)
        {
            _exception = exception;
            _value = default;

            IsSuccess = false;
        }

        private Exceptional(T data)
        {
            _exception = default;
            _value = default;

            IsSuccess = true;
        }

        public static implicit operator Exceptional<T>(Exception ex) => new Exceptional<T>(ex);
        public static implicit operator Exceptional<T>(T data) => new Exceptional<T>(data);
        
        public static implicit operator Exceptional<T>(Success<T> data) => new Exceptional<T>(data.Value);


        public R Match<R>(Func<Exception, R> onException, Func<T, R> onSuccess)
            => this.IsException
                ? onException(_exception)
                : onSuccess(_value);
        
        public Task<R> MatchAsync<R>(Func<Exception, Task<R>> onExceptionAsync, Func<T, Task<R>> onSuccessAsync)
            => this.IsSuccess ? onSuccessAsync(_value) : onExceptionAsync(_exception);
        public Task<R> MatchAsync<R>(Func<Exception,R> onException, Func<T, Task<R>> onSuccessAsync)
        {
            Task<R> AdapterNoneAsync(Exception exception) => Task.FromResult(onException(exception));

            return this.MatchAsync(AdapterNoneAsync, onSuccessAsync);
        }

        public Task<R> MatchAsync<R>(Func<Exception,Task<R>> onExceptionAsync, Func<T, R> onSuccess)
        {
            Task<R> AdapterSomeAsync(T t) => Task.FromResult(onSuccess(t));

            return this.MatchAsync(onExceptionAsync, AdapterSomeAsync);
        }
        public Unit Match(Action<Exception> onException, Action<T> onSuccess)
            => this.Match(onException.ToFunc(), onSuccess.ToFunc());
        
        public Unit OnException(Action<Exception> onException)
        {
            if (this.IsException) onException(_exception);
            return new Unit();
        }
        public Task OnExceptionAsync(Func<Exception, Task> onExceptionAsync) 
            => this.IsException ? onExceptionAsync(_exception) : Task.CompletedTask;

        public Unit OnSuccess(Action<T> onSuccess)
        {
            if (this.IsSuccess) onSuccess(this._value);
            return new Unit();
        }
        public Task OnSuccessAsync(Func<T, Task> onSuccessAsync)
            => this.IsSuccess ? onSuccessAsync(this._value) : Task.CompletedTask;


    }
}
