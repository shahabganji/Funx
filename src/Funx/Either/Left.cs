using System;

namespace Funx.Either
{
    public struct Left<L>
    {
        internal L Value { get; }

        internal Left(L left)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            this.Value = left;
        }

        public override string ToString()
        {
            return $"Left({Value})";
        }
    }
}