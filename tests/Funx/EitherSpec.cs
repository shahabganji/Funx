using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Funx.Tests
{
    public class EitherSpec
    {
        [Fact]
        public void AsEnumerable_should_return_nothing_if_Either_is_left()
        {
            var either = Either<string, int>.Left("left");

            var enumerable = either.AsEnumerable();

            Assert.NotNull(enumerable);
            Assert.IsAssignableFrom<IEnumerable<int>>(enumerable);
            Assert.Empty(enumerable);
        }

        [Fact]
        public void AsEnumerable_should_return_the_Right_object_if_Either_is_right()
        {
            var either = Either<string, int>.Right(1);

            var enumerable = either.AsEnumerable();

            Assert.NotNull(enumerable);
            Assert.IsAssignableFrom<IEnumerable<int>>(enumerable);
            Assert.Single(enumerable);
        }

        [Fact]
        public void Match_should_call_the_Right_function_if_either_is_right()
        {
            var either = Either<string, int>.Right(1);

            var rightCalled = false;
            var leftCalled = false;

            var result = either.Match(LeftFunc, RightFunc);

            Assert.Equal("1", result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);
            return;

            string RightFunc(int i)
            {
                rightCalled = true;
                return i.ToString();
            }

            string LeftFunc(string s)
            {
                leftCalled = true;
                return s;
            }
        }

        [Fact]
        public void Match_should_call_the_Left_function_if_either_is_left()
        {
            var either = Either<string, int>.Left("invalid");

            var rightCalled = false;
            var leftCalled = false;

            var result = either.Match(LeftFunc, RightFunc);

            Assert.Equal("invalid", result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);
            return;

            string RightFunc(int i)
            {
                rightCalled = true;
                return i.ToString();
            }

            string LeftFunc(string s)
            {
                leftCalled = true;
                return s;
            }
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Right_function_if_either_is_right_both_async_overload()
        {
            Either<string, int> either = 1;

            var rightCalled = false;
            var leftCalled = false;

            var result = await either.MatchAsync(LeftFuncAsync, RightFuncAsync)
                .ConfigureAwait(false);

            Assert.Equal("1", result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);
            return;

            Task<string> RightFuncAsync(int i)
            {
                rightCalled = true;
                return Task.FromResult(i.ToString());
            }

            Task<string> LeftFuncAsync(string s)
            {
                leftCalled = true;
                return Task.FromResult(s);
            }
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Left_function_if_either_is_left_both_async_overload()
        {
            Either<string, int> either = "invalid";

            var rightCalled = false;
            var leftCalled = false;

            var result = await either.MatchAsync(LeftFuncAsync, RightFuncAsync)
                .ConfigureAwait(false);

            Assert.Equal("invalid", result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);
            return;

            Task<string> RightFuncAsync(int i)
            {
                rightCalled = true;
                return Task.FromResult(i.ToString());
            }

            Task<string> LeftFuncAsync(string s)
            {
                leftCalled = true;
                return Task.FromResult(s);
            }
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Right_function_if_either_is_right_right_async_overload()
        {
            Either<string, int> either = 1;

            var rightCalled = false;
            var leftCalled = false;

            var result = await either.MatchAsync(LeftFunc, RightFuncAsync)
                .ConfigureAwait(false);

            Assert.Equal("1", result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);
            return;

            Task<string> RightFuncAsync(int i)
            {
                rightCalled = true;
                return Task.FromResult(i.ToString());
            }

            string LeftFunc(string s)
            {
                leftCalled = true;
                return s;
            }
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Left_function_if_either_is_left_right_async_overload()
        {
            Either<string, int> either = "invalid";

            var rightCalled = false;
            var leftCalled = false;

            var result = await either.MatchAsync(LeftFunc, RightFuncAsync)
                .ConfigureAwait(false);

            Assert.Equal("invalid", result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);
            return;

            Task<string> RightFuncAsync(int i)
            {
                rightCalled = true;
                return Task.FromResult(i.ToString());
            }

            string LeftFunc(string s)
            {
                leftCalled = true;
                return s;
            }
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Right_function_if_either_is_right_left_async_overload()
        {
            Either<string, int> either = 1;

            var rightCalled = false;
            var leftCalled = false;

            var result = await either.MatchAsync(LeftFuncAsync, RightFunc)
                .ConfigureAwait(false);

            Assert.Equal("1", result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);
            return;

            string RightFunc(int i)
            {
                rightCalled = true;
                return i.ToString();
            }

            Task<string> LeftFuncAsync(string s)
            {
                leftCalled = true;
                return Task.FromResult(s);
            }
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Left_function_if_either_is_left_left_async_overload()
        {
            Either<string, int> either = "invalid";

            var rightCalled = false;
            var leftCalled = false;

            var result = await either.MatchAsync(LeftFuncAsync, RightFunc)
                .ConfigureAwait(false);

            Assert.Equal("invalid", result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);
            return;

            string RightFunc(int i)
            {
                rightCalled = true;
                return i.ToString();
            }

            Task<string> LeftFuncAsync(string s)
            {
                leftCalled = true;
                return Task.FromResult(s);
            }
        }


        [Fact]
        public void IsRight_should_return_true_when_either_is_right()
        {
            Either<string, int> either = 1;

            Assert.True(either.IsRight);
            Assert.False(either.IsLeft);
        }

        [Fact]
        public void IsLeft_should_return_true_when_either_is_left()
        {
            Either<string, int> either = "left";

            Assert.True(either.IsLeft);
            Assert.False(either.IsRight);
        }


        [Fact]
        public void ToOption_should_convert_Either_of_R_to_Option_of_R()
        {
            Either<bool, string> either = Factories.Right("shahab");

            var option = either.ToOption();

            Assert.True(option.IsSome);

            option
                .WhenNone(() => Assert.True(false))
                .WhenSome(v => { Assert.Equal("shahab", v); });
        }

        [Fact]
        public void Either_should_cast_to_Option_of_R()
        {
            Either<bool, string> either = Factories.Right("shahab");

            var option = (Option<string>) either;

            Assert.True(option.IsSome);

            option
                .WhenNone(() => Assert.True(false))
                .WhenSome(v => { Assert.Equal("shahab", v); });
        }


        [Fact]
        public void ToOption_should_convert_a_right_either_to_Option_of_T()
        {
            Either<string, int> either = Either<string, int>.Right(11);

            var option = either.ToOption();

            Assert.True(option.IsSome);
            var value = option.Unwrap();
            Assert.Equal(11, value);
        }

        [Fact]
        public void ToOption_should_convert_a_left_either_to_None_Option()
        {
            var either = Either<string, int>.Left("invalid");

            var option = either.ToOption();

            Assert.True(option.IsNone);
        }


        [Fact]
        public async Task WhenLeftAsync_should_call_the_provided_action_when_either_is_in_Left_state()
        {
            var either = Either<string, int>.Left("invalid");

            Assert.True(either.IsLeft);
            await either.WhenLeftAsync(l =>
            {
                Assert.Equal("invalid", l);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }


        [Fact]
        public async Task WhenRightAsync_should_call_the_provided_action_when_either_is_in_Right_state()
        {
            var either = Either<string, int>.Right(11);

            Assert.True(either.IsRight);
            await either.WhenRightAsync(r =>
            {
                Assert.Equal(11, r);
                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }
    }
}
