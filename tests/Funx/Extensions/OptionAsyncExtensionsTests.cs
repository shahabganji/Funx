using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Funx.Extensions;
using static Funx.Helpers;
using Xunit;

namespace Funx.Tests.Extensions
{
    public class OptionAsyncExtensionsTests
    {
        private Task<string> NoneFuncAsync() => Task.FromResult("None");
        private Task<string> SomeFuncAsync(string value) => Task.FromResult(value.ToUpper());

        private string NoneFunc() => "None";
        private string SomeFunc(string value) => value.ToUpper();

        private readonly Task<Option<string>> _taskOptionWithValue;
        private readonly Task<Option<string>> _taskOptionWithNone;

        public OptionAsyncExtensionsTests()
        {
            this._taskOptionWithNone = Task.FromResult<Option<string>>(None);

            _taskOptionWithValue = Task.FromResult<Option<string>>("value");
        }

        [Fact]
        public async Task MatchAsync_should_call_provided_Some_function_when_matched()
        {
            var result = await _taskOptionWithValue.MatchAsync(NoneFunc, SomeFunc).ConfigureAwait(false);

            Assert.Equal($"VALUE", result );

        }
        [Fact]
        public async Task MatchAsync_should_call_provided_None_function_when_not_matched()
        {
            var result = await _taskOptionWithNone.MatchAsync(NoneFunc, SomeFunc).ConfigureAwait(false);

            Assert.Equal($"None", result );
        }


        [Fact]
        public async Task MatchAsync_should_call_provided_Some_when_matched_overload_noneAsync()
        {
            var result = await _taskOptionWithValue.MatchAsync(NoneFuncAsync, SomeFunc).ConfigureAwait(false);
            
            Assert.Equal("VALUE", result);
        }
        [Fact]
        public async Task MatchAsync_should_call_provided_NoneAsync_when_matched_overload_noneAsync()
        {
            var result = await _taskOptionWithNone.MatchAsync(NoneFuncAsync, SomeFunc).ConfigureAwait(false);

            Assert.Equal("None", result);
        }

        [Fact]
        public async Task MatchAsync_should_call_provided_SomeAsync_when_matched_overload_someAsync()
        {
            var result = await _taskOptionWithValue.MatchAsync(NoneFunc, SomeFuncAsync).ConfigureAwait(false);
            
            Assert.Equal("VALUE", result);
        }
        [Fact]
        public async Task MatchAsync_should_call_provided_None_when_matched_overload_someAsync()
        {
            var result = await _taskOptionWithNone.MatchAsync(NoneFunc, SomeFuncAsync).ConfigureAwait(false);

            Assert.Equal("None", result);
        }


        [Fact]
        public async Task MatchAsync_should_call_provided_SomeAsync_when_matched_overload_someAsync_noneAsync()
        {
            var result = await _taskOptionWithValue.MatchAsync(NoneFuncAsync, SomeFuncAsync).ConfigureAwait(false);
            
            Assert.Equal("VALUE", result);
        }
        [Fact]
        public async Task MatchAsync_should_call_provided_NoneAsync_when_matched_overload_someAsync_noneAsync()
        {
            var result = await _taskOptionWithNone.MatchAsync(NoneFuncAsync, SomeFuncAsync).ConfigureAwait(false);

            Assert.Equal("None", result);
        }

    }
}
