using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Funx.Tests
{
    public class EitherTests
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

            var result  = either.Match(LeftFunc, RightFunc);
            
            Assert.Equal("1" , result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);
        }
        [Fact]
        public void Match_should_call_the_Left_function_if_either_is_left()
        {
            var either = Either<string, int>.Left("invalid");

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = either.Match(LeftFunc, RightFunc);
            
            Assert.Equal("invalid" , result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);
        }

        [Fact]
        public async Task MatchAsync_should_call_the_Right_function_if_either_is_right_both_async_overload()
        {
            Either<string, int> either = 1;

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = await either.MatchAsync(LeftFuncAsync, RightFuncAsync)
                .ConfigureAwait(false);
            
            Assert.Equal("1" , result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);

        }
        [Fact]
        public async Task MatchAsync_should_call_the_Left_function_if_either_is_left_both_async_overload()
        {
            Either<string, int> either = "invalid";

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = await either.MatchAsync(LeftFuncAsync, RightFuncAsync)
                .ConfigureAwait(false);
            
            Assert.Equal("invalid" , result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);

        }
        
        [Fact]
        public async Task MatchAsync_should_call_the_Right_function_if_either_is_right_right_async_overload()
        {
            Either<string, int> either = 1;

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = await either.MatchAsync(LeftFunc, RightFuncAsync)
                .ConfigureAwait(false);
            
            Assert.Equal("1" , result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);

        }
        [Fact]
        public async Task MatchAsync_should_call_the_Left_function_if_either_is_left_right_async_overload()
        {
            Either<string, int> either = "invalid";

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = await either.MatchAsync(LeftFunc, RightFuncAsync)
                .ConfigureAwait(false);
            
            Assert.Equal("invalid" , result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);

        }

        [Fact]
        public async Task MatchAsync_should_call_the_Right_function_if_either_is_right_left_async_overload()
        {
            Either<string, int> either = 1;

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = await either.MatchAsync(LeftFuncAsync, RightFunc)
                .ConfigureAwait(false);
            
            Assert.Equal("1" , result);
            Assert.True(rightCalled);
            Assert.False(leftCalled);

        }
        [Fact]
        public async Task MatchAsync_should_call_the_Left_function_if_either_is_left_left_async_overload()
        {
            Either<string, int> either = "invalid";

            var rightCalled = false;
            var leftCalled = false;
            
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

            var result  = await either.MatchAsync(LeftFuncAsync, RightFunc)
                .ConfigureAwait(false);
            
            Assert.Equal("invalid" , result);
            Assert.False(rightCalled);
            Assert.True(leftCalled);

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

    }
}
