using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Funx.Tests
{
    public class ValidationSpec
    {
        [Fact]
        public void Valid_factory_method_should_return_a_Validation_with_data()
        {
            Validation<int> validation = 1;

            validation.IsValid.Should().BeTrue();
            validation.OnValid(v => v.Should().Be(1));

        }
        
        [Fact]
        public void Invalid_factory_method_should_return_a_Validation_with_error()
        {
            Validation<int> validation = new Error("error");

            validation.IsValid.Should().BeFalse();
            validation.OnFailure(err => err.First().Message.Should().Be("error"));

        }

        [Fact]
        public void Match_should_map_to_another_type_when_valid()
        {
            Validation<int> validation = 1;

            var result = validation.Match(
                (Error[] err) => err.First().Message,
                (int v) => v.ToString());

            result.Should().Be("1");

        }
        
        [Fact]
        public void Match_should_map_to_another_type_when_Invalid()
        {
            Validation<int> validation = new Error("error");

            var result = validation.Match(
                (Error[] err) => err.First().Message,
                (int v) => v.ToString());

            result.Should().Be("error");

        }

        [Fact]
        public void Validation_should_be_able_to_cast_to_option_when_valid()
        {
            var validation = Helpers.Valid("test");

            var option = (Option<string>) validation;

            option.IsSome.Should().BeTrue();
            option.WhenSome(x=> x.Should().Be("test"));
        }
        
        [Fact]
        public void Validation_should_be_able_to_cast_to_Non_when_Invalid()
        {
            var validation = (Validation<string>)new Error("error");

            var option = (Option<string>) validation;

            option.IsNone.Should().BeTrue();
        }
        
        
        [Fact]
        public void An_object_should_be_converted_to_Validation_of_T()
        {
            Validation<string> result = "result";

            result.IsValid.Should().BeTrue();
            result.Data.Should().Be("result");
        }

        [Fact]
        public void An_Error_should_be_converted_to_Validation()
        {
            var err = new Error("error");
            Validation<string> result = err;
        
        
            result.IsValid.Should().BeFalse();
            Action act = () => _ = result.Data;
            act.Should().Throw<InvalidOperationException>();
            result.Errors.Should().NotBeEmpty();
            result.Errors.First().Message.Should().Be("error");
        }
        
        [Fact]
        public void An_Array_of_Errors_should_be_converted_to_Validation()
        {
            var err1 = new Error("error1");
            var err2 = new Error("error2");
        
            Validation<string> result = new Error[]{err1,err2};
        
            result.IsValid.Should().BeFalse();
            Action act = () => _ = result.Data;
            act.Should().Throw<InvalidOperationException>();
            
            result.Errors.Should().NotBeEmpty();
            result.Errors.Count.Should().Be(2);
        }
        
        [Fact]
        public void Either_should_be_converted_to_successful_validation_when_it_is_right()
        {
            var either = Either<Error, int>.Right(1);
            
            Validation<int> result = either;
        
            result.IsValid.Should().BeTrue();
            result.Data.Should().Be(1);
        }
        
        [Fact]
        public void Either_should_be_converted_to_failed_result_when_it_is_left()
        {
            var either = Either<Error, int>.Left(new Error("error"));
            
            Validation<int> result = either;
        
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.First().Message.Should().Be("error");
        }
        
        [Fact]
        public void Either_of_errors_should_be_converted_to_successful_validation_when_it_is_right()
        {
            var either = Either<IEnumerable<Error>, int>.Right(1);
            
            Validation<int> result = either;
        
            result.IsValid.Should().BeTrue();
            result.Data.Should().Be(1);
            result.Errors.Should().BeNullOrEmpty();
        }
        
        [Fact]
        public void Either_of_errors_should_be_converted_to_failed_result_when_it_is_left()
        {
            var either = Either<IEnumerable<Error>, int>.Left(new Error[] {
                new Error("error1") , new Error("error2")});
            
            Validation<int> result = either;
        
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Count.Should().Be(2);
        }
        
        [Fact]
        public void Exceptional_should_be_converted_to_failed_result_when_it_is_exception()
        {
            Exceptional<int> exceptional = new InvalidOperationException("exp");
            
            Validation<int> result = exceptional;
        
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.First().Message.Should().Be("exp");
        
        }
        
        [Fact]
        public void Exceptional_should_be_converted_to_failed_result_when_it_is_success()
        {
            Exceptional<int> exceptional = 11;
            
            Validation<int> result = exceptional;
        
            result.IsValid.Should().BeTrue();
            result.Data.Should().Be(11);
        }
    }
}
