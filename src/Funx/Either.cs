using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Funx.Extensions;
using Unit = System.ValueTuple;

namespace Funx
{

    public struct Either<L, R>
    {
        private readonly L _left;
        private readonly R _right;

        public bool IsRight { get; }
        public bool IsLeft => !IsRight;

        private Either(L left)
        {
            IsRight = false;

            _left = left;
            _right = default;
        }
        private Either(R right)
        {
            IsRight = true;
            _right = right;

            _left = default;
        }

        public static Either<>


        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        public static implicit operator Either<L,R>( Either.Left<L> left ) => new Either<L, R>(left.Value);
        public static implicit operator Either<L,R>( Either.Right<R> right ) => new Either<L, R>(right.Value);


        public TR Match<TR>(Func<L, TR> left, Func<R, TR> right)
            => IsRight ? right(_right) : left(_left);

        public Unit Match(Action<L> left, Action<R> right)
            => this.Match(left.ToFunc(), right.ToFunc());
        
        public IEnumerable<R> AsEnumerable()
        {
            if (this.IsRight) yield return _right;
        }

        public override string ToString()
        {
            return Match(l => $"Left({l})", r => $"Right({r})");
        }
    }


    namespace Either
    {
        public struct Left<L>
        {
            internal L Value { get; }

            internal Left(L left)
            {
                if (left == null)
                    throw new InvalidEnumArgumentException(nameof(left));

                this.Value = left;
            }

            public override string ToString()
            {
                return $"Left({Value})";
            }
        }

        public struct Right<R>
        {
            internal R Value { get; }

            internal Right(R right)
            {
                if (right == null)
                    throw new InvalidEnumArgumentException(nameof(right));

                Value = right;
            }

            public override string ToString()
            {
                return $"Right({Value})";
            }
        }
    }
}