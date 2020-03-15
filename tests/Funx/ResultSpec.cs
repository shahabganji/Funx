using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Funx.Tests
{
    public class ResultSpec
    {
        [Fact]
        public void An_object_should_be_converted_to_Result_of_T()
        {
            Result<string> result = "result";

            result.Failed.Should().BeFalse();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void An_Error_should_be_converted_to_Result()
        {
            var err = new Error("error");

            Result<string> result = err;


            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            
            result.Failed.Should().BeTrue();
            result.Errors.Should().NotBeEmpty();
            result.Errors.First().Message.Should().Be("error");
        }
        
        [Fact]
        public void An_Array_of_Errors_should_be_converted_to_Result()
        {
            var err1 = new Error("error1");
            var err2 = new Error("error2");

            Result<string> result = new Error[]{err1,err2};

            result.Succeeded.Should().BeFalse();
            result.Data.Should().BeNull();
            
            result.Failed.Should().BeTrue();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Count.Should().Be(2);
        }

        [Fact]
        public void Either_should_be_converted_to_successful_result_when_it_is_right()
        {
            var either = Either<Error, int>.Right(1);
            
            Result<int> result = either;

            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(1);
        }
        
        [Fact]
        public void Either_should_be_converted_to_failed_result_when_it_is_left()
        {
            var either = Either<Error, int>.Left(new Error("error"));
            
            Result<int> result = either;

            result.Failed.Should().BeTrue();
            result.Errors.Should().NotBeEmpty();
            result.Errors.First().Message.Should().Be("error");
        }
        
        [Fact]
        public void Either_of_errors_should_be_converted_to_failed_result_when_it_is_left()
        {
            var either = Either<IEnumerable<Error>, int>.Right(1);
            
            Result<int> result = either;

            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(1);
            result.Errors.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void Either_of_errors_should_be_converted_to_failed_result_when_it_is_right()
        {
            var either = Either<IEnumerable<Error>, int>.Left(new Error[] {
                new Error("error1") , new Error("error2")});
            
            Result<int> result = either;

            result.Failed.Should().BeTrue();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Count.Should().Be(2);
        }
        
        [Fact]
        public void Exceptional_should_be_converted_to_failed_result_when_it_is_exception()
        {
            Exceptional<int> exceptional = new InvalidOperationException("exp");
            
            Result<int> result = exceptional;

            result.Failed.Should().BeTrue();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.First().Message.Should().Be("exp");

        }
        
        [Fact]
        public void Exceptional_should_be_converted_to_failed_result_when_it_is_success()
        {
            Exceptional<int> exceptional = 11;
            
            Result<int> result = exceptional;

            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(11);
        }
        
    }
}
