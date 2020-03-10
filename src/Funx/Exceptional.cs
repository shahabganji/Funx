using System;
using System.Threading.Tasks;
using Funx.Exceptional;
using Unit = System.ValueTuple;

namespace Funx
{
    public struct Exceptional<T>
    {
        private readonly T _value;
        private readonly Exception _exception;

        public static Exceptional<T> Success(T value) => Helpers.Success(value);

        public bool IsSuccess => _exception == null;
        public bool IsException => _exception != null;

        private Exceptional(Exception exception)
        {
            _exception = exception;
            _value = default;
            
        }

        private Exceptional(T data)
        {
            _exception = null;
            _value = data;
        }

        public static implicit operator Exceptional<T>(T data) => new Exceptional<T>(data);
        
        public static implicit operator Exceptional<T>(Exception ex) => new Exceptional<T>(ex);
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
