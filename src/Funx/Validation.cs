using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Funx.Extensions;
using Funx.Validation;

namespace Funx
{
    public readonly struct Validation<T>
    {
        public static Validation<T> Valid(T data) => Helpers.Valid(data);

        private readonly T _data;

        public T Data => IsValid
            ? _data
            : throw new InvalidOperationException("You cannot access data, while the `Isvalid` property is `false`.");

        private readonly List<Error> _errors;
        public IReadOnlyCollection<Error> Errors => _errors?.AsReadOnly();
        public bool IsValid => !this.Errors.SafeAny();

        private Validation(T data)
        {
            _data = data;
            _errors = new List<Error>();
        }

        private Validation(params Error[] errors)
        {
            _data = default;
            _errors = errors.ToList();
        }

        public static implicit operator Validation<T>(T data) => Valid(data);
        public static implicit operator Validation<T>(Valid<T> valid) => new Validation<T>(valid.Data);

        public static implicit operator Validation<T>(Error error) => new Validation<T>(error);
        public static implicit operator Validation<T>(Error[] errors) => new Validation<T>(errors);

        public static implicit operator Validation<T>(Either<Error, T> either)
            => either.ToValidation();

        public static implicit operator Validation<T>(Either<IEnumerable<Error>, T> either)
            => either.ToValidation();

        public static implicit operator Validation<T>(Exceptional<T> exceptional)
            => exceptional.ToValidation();

        public static explicit operator Option<T>(Validation<T> validation)
            => validation.ToOption();

        public void OnValid(Action<T> onValid)
        {
            if (this.IsValid) onValid(this.Data);
        }

        public Task OnValidAsync(Func<T, Task> onValidAsync)
            => this.IsValid ? onValidAsync(Data) : Task.CompletedTask;

        public void OnFailure(Action<Error[]> onFailure)
        {
            if (!this.IsValid) onFailure(this.Errors.ToArray());
        }

        public Task OnFailureAsync(Func<Error[], Task> onFailureAsync)
            => !this.IsValid ? onFailureAsync(this.Errors.ToArray()) : Task.CompletedTask;

        public R Match<R>(Func<Error[], R> onFailure, Func<T, R> onValid)
            => this.IsValid
                ? onValid(this.Data)
                : onFailure(this.Errors.ToArray());

        public Option<T> ToOption()
            => this.IsValid
                ? this.Data
                : Option<T>.None;
    }
}
