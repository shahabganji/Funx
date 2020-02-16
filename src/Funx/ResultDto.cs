using System.Collections.Generic;
using System.Linq;

namespace Funx
{
    public class ResultDto<T>
    {
        public bool Succeeded { get; }
        public bool Failed => !this.Succeeded;

        public T Data { get; }
        public IReadOnlyCollection<Error> Errors { get; }

        private ResultDto(T data)
        {
            this.Data = data;
            this.Succeeded = true;
        }
        
        private ResultDto(params Error[] errors)
        {
            this.Errors = errors;
            this.Succeeded = false;
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
        
        // TODO: Add same operators for Exceptional<T>, and Validation<T>
    }
}
