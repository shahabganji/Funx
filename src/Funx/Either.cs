﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funx.Either;
using Funx.Extensions;
using static Funx.Helpers;
using Unit = System.ValueTuple;

namespace Funx
{
    public struct Either<L, R>
    {
        public static Either<L, R> Left(L l) => Helpers.Left(l);
        public static Either<L, R> Right(R r) => Helpers.Right(r);

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


        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        public static implicit operator Either<L, R>(Left<L> left) => new Either<L, R>(left.Value);
        public static implicit operator Either<L, R>(Right<R> right) => new Either<L, R>(right.Value);

        // ToDo: add unit tests for this 
        public static implicit operator Option<R>(Either<L, R> either) => either.ToOption(); 


        public TR Match<TR>(Func<L, TR> left, Func<R, TR> right)
            => IsRight ? right(_right) : left(_left);

        public Task<TR> MatchAsync<TR>(Func<L, Task<TR>> leftAsync, Func<R, Task<TR>> rightAsync)
            => this.IsRight ? rightAsync(_right) : leftAsync(_left);

        public Task<TR> MatchAsync<TR>(Func<L, TR> left, Func<R, Task<TR>> rightAsync)
        {
            Task<TR> AdapterLeftAsync(L l) => Task.FromResult(left(l));

            return this.MatchAsync(AdapterLeftAsync, rightAsync);
        }
        
        public Task<TR> MatchAsync<TR>(Func<L, Task<TR>> leftAsync, Func<R, TR> right)
        {
            Task<TR> AdapterRightAsync(R r) => Task.FromResult(right(r));

            return this.MatchAsync(leftAsync, AdapterRightAsync);
        }


        public Unit Match(Action<L> left, Action<R> right)
            => this.Match(left.ToFunc(), right.ToFunc());
        
        public Unit WhenLeft(Action<L> left)
        {
            if (this.IsLeft) left(_left);
            return new Unit();
        }
        public Task WhenLeftAsync(Func<L,Task> leftAsync) => this.IsLeft ? leftAsync(_left) : Task.CompletedTask;
        public Unit WhenRight(Action<R> right)
        {
            if (this.IsRight) right(_right);
            return new Unit();
        }
        public Task WhenRightAsync(Func<R, Task> rightAsync) => this.IsRight ? rightAsync(_right) : Task.CompletedTask;


        // TODO: Add unit tests for this:
        public Option<R> ToOption() => this.Match(_ => None, Some);
        
        public IEnumerable<R> AsEnumerable()
        {
            if (this.IsRight) yield return _right;
        }
        public override string ToString()
        {
            return Match(l => $"Left({l})", r => $"Right({r})");
        }
    }
}