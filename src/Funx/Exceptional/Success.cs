using System;

namespace Funx.Exceptional
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
