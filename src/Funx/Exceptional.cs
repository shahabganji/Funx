using System;
using System.Threading.Tasks;
using Funx.Exceptional;
using static Funx.Helpers;
using Unit = System.ValueTuple;

namespace Funx
{
    public readonly struct Exceptional<T>
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

        public static explicit operator Option<T>(Exceptional<T> exceptional)
            => exceptional.ToOption();


        public R Match<R>(Func<Exception, R> onException, Func<T, R> onSuccess)
            => this.IsException
                ? onException(_exception)
                : onSuccess(_value);

        public Task<R> MatchAsync<R>(Func<Exception, Task<R>> onExceptionAsync, Func<T, Task<R>> onSuccessAsync)
            => this.IsSuccess
                ? onSuccessAsync(_value)
                : onExceptionAsync(_exception);

        public Task<R> MatchAsync<R>(Func<Exception, R> onException, Func<T, Task<R>> onSuccessAsync)
        {
            Task<R> AdapterExceptionAsync(Exception exception) => Task.FromResult(onException(exception));

            return this.MatchAsync(AdapterExceptionAsync, onSuccessAsync);
        }

        public Task<R> MatchAsync<R>(Func<Exception, Task<R>> onExceptionAsync, Func<T, R> onSuccess)
        {
            Task<R> AdapterSuccessAsync(T t) => Task.FromResult(onSuccess(t));

            return this.MatchAsync(onExceptionAsync, AdapterSuccessAsync);
        }

        public void OnException(Action<Exception> onException)
        {
            if (this.IsException) onException(_exception);
        }

        public Task OnExceptionAsync(Func<Exception, Task> onExceptionAsync)
            => this.IsException ? onExceptionAsync(_exception) : Task.CompletedTask;

        public void OnSuccess(Action<T> onSuccess)
        {
            if (this.IsSuccess) onSuccess(this._value);
        }

        public Task OnSuccessAsync(Func<T, Task> onSuccessAsync)
            => this.IsSuccess ? onSuccessAsync(this._value) : Task.CompletedTask;

        public Option<T> ToOption()
            => this.Match<Option<T>>(
                _ => None,
                data => data);
    }
}
