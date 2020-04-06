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
        
    }
}
