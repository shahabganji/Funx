using System;
using Funx.Exceptional;
using Funx.Option;
using Xunit;
using Xunit.Abstractions;
using static Funx.Helpers;

namespace Funx.Tests
{
    public class ExceptionalTests
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

            exceptional.OnException(ex => { Assert.Equal(ex, exception); });
        }

        [Fact]
        public void Data_of_T_should_be_cast_to_Exceptional()
        {
            const string value = "sample";
            Exceptional<string> exceptional = value;

            Assert.True(exceptional.IsSuccess);

            exceptional.OnSuccess(data => { Assert.Equal(data, value); });
        }

       
        
    }
}
