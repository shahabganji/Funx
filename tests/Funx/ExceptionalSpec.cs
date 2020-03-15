using System;
using System.Threading.Tasks;
using FluentAssertions;
using Funx.Exceptional;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests
{
    public class ExceptionalSpec
    {
        [Fact]
        public void IsSuccess_Should_return_true_when_not_an_Exception()
        {
            Exceptional<string> exceptional = Success("success");

            Assert.True(exceptional.IsSuccess);
            Assert.False(exceptional.IsException);
        }

        [Fact]
        public void IsException_Should_return_true_when_not_a_Success()
        {
            Exceptional<int> exceptional = new InvalidCastException();

            Assert.False(exceptional.IsSuccess);
            Assert.True(exceptional.IsException);
        }

        [Fact]
        public void Exceptions_should_be_cast_to_Exceptional()
        {
            var exception = new InvalidCastException();
            Exceptional<int> exceptional = exception;

            Assert.True(exceptional.IsException);

            exceptional.OnException(ex => Assert.Equal(exception, ex));
        }

        [Fact]
        public void Success_should_throw_exception_when_value_is_null()
        {
            Action act = () => Success<string>(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Data_of_T_should_cast_to_Exceptional()
        {
            const string value = "sample";
            Exceptional<string> exceptional = value;

            Assert.True(exceptional.IsSuccess);

            exceptional.OnSuccess(data => Assert.Equal(value, data));
        }

        [Fact]
        public async Task OnExceptionAsync_should_be_called_when_on_it_is_on_exception_state()
        {
            var exception = new InvalidCastException();
            Exceptional<int> exceptional = exception;

            Assert.True(exceptional.IsException);

            Task ShouldBeCalledOnException(Exception ex)
            {
                Assert.Equal(exception, ex);
                return Task.CompletedTask;
            }

            await exceptional.OnExceptionAsync(ShouldBeCalledOnException).ConfigureAwait(false);
        }

        [Fact]
        public async Task OnSuccessAsync_should_be_called_when_on_it_is_on_success_state()
        {
            const string value = "sample";
            Exceptional<string> exceptional = value;

            Assert.True(exceptional.IsSuccess);

            Task ShouldBeCalledOnSuccess(string data)
            {
                Assert.Equal(value, data);
                return Task.CompletedTask;
            }

            await exceptional.OnSuccessAsync(ShouldBeCalledOnSuccess);
        }

        [Fact]
        public void Match_should_convert_the_underlying_type_to_R()
        {
            var data = Exceptional<int>.Success(1);

            var result =data.Match(
                _ => "no error",
                d => d.ToString()
            );

            result.Should().BeOfType<string>();
            result.Should().Be("1");

        }
        
        [Fact]
        public void Match_should_convert_the_underlying_exception_to_R()
        {
            Exceptional<int> data = new InvalidOperationException("error"); 

            var result =data.Match(
                ex => ex.Message,
                d => d.ToString()
            );

            result.Should().BeOfType<string>();
            result.Should().Be("error");

        }

        [Fact]
        public void Exceptional_should_be_cast_to_option_when_is_success()
        {
            Exceptional<int> exceptional = 1;

            var option = (Option<int>) exceptional;

            option.IsSome.Should().BeTrue();
            var data = option.Unwrap();
            data.Should().Be(1);
        }
        
        [Fact]
        public void Exceptional_should_cast_to_None_Option_when_it_is_exception()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var option = (Option<int>) exceptional;

            option.IsNone.Should().BeTrue();
            (option == None).Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_should_run_the_onSuccessAsync_when_success()
        {
            var exceptional = Success(11);

            var result = await exceptional.MatchAsync(
                 (ex) => Task.FromResult("no error"),
                 v => Task.FromResult(v.ToString())
            ).ConfigureAwait(false);

            result.Should().Be("11");
        }
        
        [Fact]
        public async Task MatchAsync_should_run_the_onExceptionAsync_when_exception()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var result = await exceptional.MatchAsync(
                (ex) => Task.FromResult(ex.Message),
                v => Task.FromResult(v.ToString())
            ).ConfigureAwait(false);

            result.Should().Be("invalid");
        }
        
        
        [Fact]
        public async Task MatchAsync_should_run_the_onSuccessAsync_For_onSuccessSync_overload_when_success()
        {
            var exceptional = Success(11);

            var result = await exceptional.MatchAsync(
                (ex) => "no error",
                v => Task.FromResult(v.ToString())
            ).ConfigureAwait(false);

            result.Should().Be("11");
        }
        
        [Fact]
        public async Task MatchAsync_should_run_the_onExceptionAsync_For_onSuccessSync_overload_when_exception()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var result = await exceptional.MatchAsync(
                (ex) => ex.Message,
                v => Task.FromResult(v.ToString())
            ).ConfigureAwait(false);

            result.Should().Be("invalid");
        }
        
        [Fact]
        public async Task MatchAsync_should_run_the_onSuccessAsync_For_onExceptionsSync_overload_when_success()
        {
            var exceptional = Success(11);

            var result = await exceptional.MatchAsync(
                (ex) => Task.FromResult("no error"),
                v => v.ToString()
            ).ConfigureAwait(false);

            result.Should().Be("11");
        }
        
        [Fact]
        public async Task MatchAsync_should_run_the_onExceptionAsync_For_onExceptionsSync_overload_when_exception()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var result = await exceptional.MatchAsync(
                (ex) => Task.FromResult(ex.Message),
                v => v.ToString()
            ).ConfigureAwait(false);

            result.Should().Be("invalid");
        }
        
    }
}
