using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Funx.Extensions;
using Funx.Validation;

namespace Funx
{
    public readonly struct Validation<T>
    {

        public static Validation<T> Valid(T data) => Helpers.Valid(data);

        private T Data { get; }
        private IEnumerable<Error> Errors { get;}
        public bool IsValid => !this.Errors.SafeAny();

        private Validation(T data)
        {
            this.Data = data;
            Errors = null;
        }
        
        private Validation(params Error[] errors)
        {
            this.Data = default;
            this.Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public static implicit operator Validation<T>(T data) => Valid(data);
        public static implicit operator Validation<T>( Valid<T> valid ) => new Validation<T>(valid.Data);
        
        public static implicit operator Validation<T>(Error error) => new Validation<T>(error);
        public static implicit operator Validation<T>(Error[] errors) => new Validation<T>(errors);
        public static explicit operator Option<T>(Validation<T> validation)
            => validation.ToOption();

        public void OnValid(Action<T> onValid)
        {
            if (this.IsValid) onValid(this.Data);
        }

        public Task OnValidAsync(Func<T,Task> onValidAsync)
            => this.IsValid ? onValidAsync(Data) : Task.CompletedTask;
            
        public void OnFailure(Action<Error[]> onFailure)
        {
            if (!this.IsValid) onFailure(this.Errors.ToArray());
        }
        
        public Task OnFailureAsync(Func<Error[],Task> onFailureAsync) 
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
