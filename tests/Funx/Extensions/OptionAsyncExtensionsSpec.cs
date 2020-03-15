using System.Threading.Tasks;
using FluentAssertions;
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

            Assert.Equal($"VALUE", result);
        }

        [Fact]
        public async Task MatchAsync_should_call_provided_None_function_when_not_matched()
        {
            var result = await _taskOptionWithNone.MatchAsync(NoneFunc, SomeFunc).ConfigureAwait(false);

            Assert.Equal($"None", result);
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

        [Fact]
        public async Task BindAsync_should_stay_in_the_same_abstraction()
        {
            Task<Option<string>> ToUpper(string s) => Task.FromResult(Some(s.ToUpper()));

            var strOption = Task.FromResult(Some("value"));

            var result = await strOption.BindAsync(ToUpper).ConfigureAwait(false);

            Assert.NotEqual(result, None);
            result.ForEach(x => Assert.Equal(x, $"VALUE"));
        }

        [Fact]
        public async Task BindAsync_should_return_None_when_Not_matched()
        {
            Task<Option<string>> ToUpper(string s) => Task.FromResult(Some(s.ToUpper()));

            var strOption = Task.FromResult<Option<string>>(None);

            var result = await strOption.BindAsync(ToUpper).ConfigureAwait(false);

            Assert.Equal(result, None);
        }

        [Fact]
        public async Task MapAsync_should_map_Option_of_T_to_Option_of_TR_when_matched()
        {
            var result = await _taskOptionWithValue.MapAsync(value => Task.FromResult(value[0])).ConfigureAwait(false);

            Assert.True(result != None);
            Assert.Equal('v', result);
        }

        [Fact]
        public async Task MapAsync_should_map_Option_of_T_to_None_when_not_matched()
        {
            var result = await _taskOptionWithNone.MapAsync(value => Task.FromResult(value[0])).ConfigureAwait(false);

            Assert.True(result == None);
        }

        [Fact]
        public async Task Select_should_map_a_task_of_an_option_to_another_option()
        {
            var resultTask =
                from s in _taskOptionWithValue
                select Task.FromResult(s.ToUpper());

            var result = await resultTask.ConfigureAwait(false);

            Assert.NotEqual(result, None);
            Assert.Equal($"VALUE", result);
        }

        [Fact]
        public async Task Select_should_map_an_option_to_None_when_option_is_none()
        {
            var result =
                from s in await _taskOptionWithNone.ConfigureAwait(false)
                select s.ToUpper();

            Assert.True(result.IsNone);
            Assert.Equal(None, result);
        }


        [Fact]
        public async Task Where_should_apply_a_true_predicate_and_return_value_when_there_is_an_option()
        {
            var result =
                from s in await _taskOptionWithValue.ConfigureAwait(false)
                where s.StartsWith("v")
                select s.ToUpper();

            Assert.NotEqual(result, None);
            Assert.Equal($"VALUE", result);
        }
        
        [Fact]
        public void Where_should_return_None_when_the_option_is_none()
        {
            var option = Option<string>.None;
            
            var result =
                from s in option
                where s.StartsWith("v")
                select s.ToUpper();

            result.IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task Where_should_apply_a_false_predicate_and_return_None_when_there_is_an_option()
        {
            var result =
                (await _taskOptionWithNone
                    .Where(s => s.StartsWith("zzz")).ConfigureAwait(false))
                .Select(s =>
                    s.ToUpper());

            Assert.Equal(result, None);
        }

        [Fact]
        public async Task Where_should_return_none_when_there_is_None()
        {
            var result = await _taskOptionWithNone.Where(s => s.StartsWith("v")).ConfigureAwait(false);

            Assert.Equal(None, result);
        }
    }
}
