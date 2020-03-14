using System;
using FluentAssertions;
using Funx.Extensions;
using Xunit;

namespace Funx.Tests.Extensions
{
    public class EitherExtensionsSpec
    {

        [Fact]
        public void Map_should_apply_the_map_func_on_the_right_side_of_an_either()
        {
            Either<bool, int> either = 1;

            // select calls the map internally, so both are tested with less unit tests
            var mapped = either.Select(i => i.ToString());

            mapped.Should().BeAssignableTo<Either<bool, string>>();
            mapped.IsRight.Should().BeTrue();
            mapped.WhenRight(x=> x.Should().Be("1"));

        }
        
        [Fact]
        public void Map_should_not_apply_the_map_func_on_the_right_side_if_an_either_is_left_and_should_return_left()
        {
            Either<bool, int> either = false;
            
            // select calls the map internally, so both are tested with less unit tests
            var mapped = either.Select(i => i.ToString());

            mapped.Should().BeAssignableTo<Either<bool, string>>();
            mapped.IsLeft.Should().BeTrue();
            mapped.WhenLeft(v=> v.Should().BeFalse());
        }
        
        [Fact]
        public void MapLeft_should_apply_the_map_func_on_the_left_side_if_an_either_is_left()
        {
            Either<bool, int> either = false;
            var mapped = either.MapLeft(i => i.ToString());

            mapped.Should().BeAssignableTo<Either<string,int>>();
            mapped.IsLeft.Should().BeTrue();
            mapped.WhenLeft(v=> v.Should().Be("False"));
        }
        
        [Fact]
        public void MapLeft_should_not_apply_the_map_func_ont_the_left_side_when_either_is_right()
        {
            Either<bool, int> either = 1;
            var mapped = either.MapLeft(i => i.ToString());

            mapped.Should().BeAssignableTo<Either<string,int>>();
            mapped.IsRight.Should().BeTrue();
            mapped.WhenRight(v=> v.Should().Be(1));
        }

        [Fact]
        public void ForEach_should_only_call_when_either_is_right()
        {
            Either<string, int> rightEither = 1;
            Either<string, int> leftEither = "left";

            rightEither.ForEach(v => v.Should().Be(1));

            var leftCalled = false;
            leftEither.ForEach(_ => leftCalled = true);

            leftCalled.Should().BeFalse();
        }

        [Fact]
        public void Bind_from_either_to_either()
        {
            Either<string, int> First() => 1;
            Either<string, string> Second(int value) => Helpers.Right( value.ToString());

            var result = First().Bind(Second);

            result.Should().BeAssignableTo<Either<string, string>>();
            result.IsRight.Should().BeTrue();
            result.WhenRight(value=> value.Should().Be("1"));
        }
        
        [Fact]
        public void Bind_from_either_to_either_returns_left_when_first_either_is_left()
        {
            Either<string, int> First() => "left";
            Either<string, string> Second(int value) => Helpers.Right( value.ToString());

            var result = First().Bind(Second);

            result.Should().BeAssignableTo<Either<string, string>>();
            result.IsLeft.Should().BeTrue();
            result.WhenLeft(value=> value.Should().Be("left"));
        }
        
        
        [Fact]
        public void Bind_from_either_to_option()
        {
            Either<string,int> FromInt() => 1;
            Option<string> FromString(int i) => i.ToString();

            var x = FromInt().Bind(FromString);

            x.IsSome.Should().BeTrue();
            x.Should().BeAssignableTo<Option<string>>();
            x.WhenSome(s => s.Should().Be("1"));

        }
        
        [Fact]
        public void Bind_from_either_to_option_returns_None_when_either_is_left()
        {
            Either<string,int> FromInt() => "left value";
            Option<string> FromString(int i) => i.ToString();

            var x = FromInt().Bind(FromString);

            x.IsNone.Should().BeTrue();
            x.Should().BeAssignableTo<Option<string>>();
            
        }
    }
}
