using System.Collections.Generic;
using System.Linq;

namespace Funx
{
    public class ResultDto<T>
    {
        public bool Succeeded => this.Errors == null;
        public bool Failed => this.Errors != null;

        public T Data { get; }
        public IReadOnlyCollection<Error> Errors { get; }

        private ResultDto(T data)
        {
            this.Data = data;
            this.Errors = null;
        }
        
        private ResultDto(params Error[] errors)
        {
            this.Data = default;
            this.Errors = errors;
        }


        public static implicit operator ResultDto<T>(T data)
            => new ResultDto<T>(data);

        public static implicit operator ResultDto<T>(Error error)
            => new ResultDto<T>(error);

        public static implicit operator ResultDto<T>(Error[] errors)
            => new ResultDto<T>(errors);
        

        public static implicit operator ResultDto<T>(Either<Error, T> either)
            => either.Match<ResultDto<T>>(error => error, data => data);

        public static implicit operator ResultDto<T>(Either<IEnumerable<Error>, T> either)
            => either.Match<ResultDto<T>>(errors => errors.ToArray(), data => data);
        
        
        public static implicit operator ResultDto<T>(Exceptional<T> either)
            => either.Match<ResultDto<T>>(exp => new Error(exp.Message), data => data);

    }
}
