using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funx.Extensions;
using Funx.Validation;

namespace Funx
{
    public readonly struct Validation<T>
    {

        public static Validation<T> Valid(T data) => Helpers.Valid(data);
        public static Validation<T> Invalid(params Error[] errors) => Helpers.Invalid<T>(errors);
        
        public T Data { get; }
        public IEnumerable<Error> Errors { get;}
        public bool IsValid => !this.Errors.SafeAny();

        private Validation(T data)
        {
            this.Data = data;
            Errors = null;
        }
        
        private Validation(params Error[] errors)
        {
            this.Data = default;
            Errors = errors;
        }

        public static implicit operator Validation<T>(T data) => Valid(data);
        public static implicit operator Validation<T>(Error error) => Invalid(error);
        public static implicit operator Validation<T>(Error[] errors) => Invalid(errors);
        
        public static implicit operator Validation<T>( Valid<T> valid ) => new Validation<T>(valid.Data);
        public static implicit operator Validation<T>( Invalid invalid ) => new Validation<T>(invalid.Errors.ToArray());

        
        

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
    }
}
