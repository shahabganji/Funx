using System;
using Funx.Exceptional;

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


        
    }

    namespace  Exceptional
    {
        public struct Success<T>
        {
            internal T Value { get; }

            internal Success(T value)
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                Value = value;
            }

            public override string ToString()
            {
                return $"Success({Value})";
            }
        }
    }
}
