using System.Collections.Generic;
using System.Linq;
using Funx.Extensions;

namespace Funx
{
    public class Result<T>
    {
        public bool Succeeded => this.Errors == null;
        public bool Failed => this.Errors != null;

        public T Data { get; }
        public IReadOnlyCollection<Error> Errors { get; }

        private Result(T data)
        {
            this.Data = data;
            this.Errors = null;
        }
        private Result(params Error[] errors)
        {
            this.Data = default;
            this.Errors = errors;
        }


        public static implicit operator Result<T>(T data)
            => new Result<T>(data);

        public static implicit operator Result<T>(Error error)
            => new Result<T>(error);

        public static implicit operator Result<T>(Error[] errors)
            => new Result<T>(errors);
        
        public static implicit operator Result<T>(Either<Error, T> either)
            => either.ToResult();

        public static implicit operator Result<T>(Either<IEnumerable<Error>, T> either)
            => either.ToResult();

        public static implicit operator Result<T>(Exceptional<T> exceptional)
            => exceptional.ToResult();

    }
}
