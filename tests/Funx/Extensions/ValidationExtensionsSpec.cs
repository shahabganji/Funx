using System;
using System.Linq;
using FluentAssertions;
using Funx.Extensions;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests.Extensions
{
    public class ValidationExtensionsSpec
    {

        [Fact]
        public void Map_should_apply_the_map_func_on_the_right_side_of_an_either()
        {
            Validation<int> validation = 1;

            // select calls the map internally, so both are tested with less unit tests
            var mapped = validation.Select(i => i.ToString());

            mapped.IsValid.Should().BeTrue();
            mapped.OnValid(x=> x.Should().Be("1"));

        }
        
        [Fact]
        public void Map_should_not_apply_the_map_func_on_the_right_side_if_an_either_is_left_and_should_return_left()
        {
            Validation<int> error= new Error("error");
            
            // select calls the map internally, so both are tested with less unit tests
            var mapped = error.Select(i => i.ToString());
            
            mapped.IsValid.Should().BeFalse();
            mapped.OnFailure(v =>
            {
                v.Should().NotBeNullOrEmpty();
                v.Single().Message.Should().Be("error");
            });
        }
        
        [Fact]
        public void ForEach_should_only_call_when_either_is_right()
        {
            Validation<int> valid = 1;
            Validation<int> invalid = new Error("left");

            valid.ForEach(v => v.Should().Be(1));

            var leftCalled = false;
            invalid.ForEach(_ => leftCalled = true);

            leftCalled.Should().BeFalse();
        }

        [Fact]
        public void Bind_from_either_to_either()
        {
            Validation<int> First() => 1;
            Validation<string> Second(int value) => Valid( value.ToString());

            var result = First().Bind(Second);

            result.Should().BeAssignableTo<Validation<string>>();
            result.IsValid.Should().BeTrue();
            result.OnValid(value=> value.Should().Be("1"));
        }
        
        [Fact]
        public void Bind_from_either_to_either_returns_left_when_first_either_is_left()
        {
            Validation<int> First() => new Error("error");
            Validation<string> Second(int value) => Valid( value.ToString());

            var result = First().Bind(Second);
            
            result.IsValid.Should().BeFalse();
            result.OnFailure(v =>
            {
                v.Should().NotBeNullOrEmpty();
                v.Single().Message.Should().Be("error");
            });
        }
        
        
        [Fact]
        public void Bind_from_either_to_option()
        {
            Validation<int> FromInt() => 1;
            Option<string> FromString(int i) => i.ToString();

            var x = FromInt().Bind(FromString);

            x.IsSome.Should().BeTrue();
            x.Should().BeAssignableTo<Option<string>>();
            x.WhenSome(s => s.Should().Be("1"));

        }
        
        [Fact]
        public void Bind_from_either_to_option_returns_None_when_either_is_left()
        {
            Validation<int> FromInt() => new Error("error");
            Option<string> FromString(int i) => i.ToString();

            var x = FromInt().Bind(FromString);

            x.IsNone.Should().BeTrue();
            x.Should().BeAssignableTo<Option<string>>();
            
        }
    }
}
