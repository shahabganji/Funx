using System;

namespace Funx.Either
{
    public struct Right<R>
    {
        internal R Value { get; }

        internal Right(R right)
        {
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            Value = right;
        }

        public override string ToString()
        {
            return $"Right({Value})";
        }
    }
}