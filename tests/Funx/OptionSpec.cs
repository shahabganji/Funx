using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Funx.Extensions;
using Funx.Option;
using Xunit;
using FluentAssertions;
using static Funx.Helpers;

namespace Funx.Tests
{
    public class OptionSpec
    {
        [Fact]
        public void NoneShouldBeAnOptionOfT()
        {
            Option<bool> option = Helpers.None;

            Assert.IsType<Option<bool>>(option);
        }

        [Fact]
        public void SomeOfTShouldBeAnOptionOfT()
        {
            Option<bool> option = Some(true);

            Assert.IsType<Option<bool>>(option);
        }

        [Fact]
        public async Task WhenNoneMatchAsyncMethodShouldCallNoneFuncAsync()
        {
            Option<string> noneOpt = Helpers.None;

            Task<bool> NoneFuncAsync()
            {
                return Task.FromResult(true);
            }

            Task<bool> SomeFuncAsync(string vs)
            {
                return Task.FromResult(false);
            }

            var noneCalled = await noneOpt.MatchAsync(NoneFuncAsync, SomeFuncAsync).ConfigureAwait(false);

            Assert.True(noneCalled);
        }

        [Fact]
        public void WhenNoneMatchMethodShouldCallNoneFunc()
        {
            Option<string> noneOpt = Helpers.None;

            var noneCalled = noneOpt.Match(() => true, v => false);

            Assert.True(noneCalled);
        }

        [Fact]
        public async Task WhenSomeMatchAsyncMethodShouldNotCallNoneFuncAsync()
        {
            var someOpt = Some("fake");

            Task<bool> NoneFuncAsync()
            {
                return Task.FromResult(false);
            }

            Task<bool> SomeFuncAsync(string vs)
            {
                return Task.FromResult(true);
            }

            var noneCalled = await someOpt.MatchAsync(NoneFuncAsync, SomeFuncAsync).ConfigureAwait(false);

            Assert.True(noneCalled);
        }

        [Fact]
        public async Task WhenIsSomeThenCallSomeAsyncAndNotNone()
        {
            var option = Some(11);

            bool NoneFunc() => false;
            Task<bool> SomeAsync(int value) => Task.FromResult(value == 11);

            var hasCalledSome = await option.MatchAsync(NoneFunc, SomeAsync)
                .ConfigureAwait(false);

            Assert.True(hasCalledSome);

        }

        [Fact]
        public async Task WhenIsNoneThenCallNoneAndNotSomeAsync()
        {
            var option = (Option<int>)Helpers.None;

            bool NoneFunc() => true;
            Task<bool> SomeAsync(int value) => Task.FromResult(false);

            var hasCalledNone = await option.MatchAsync(NoneFunc, SomeAsync)
                .ConfigureAwait(false);

            Assert.True(hasCalledNone);

        }

        [Fact]
        public async Task WhenIsSomeThenCallSomeAndNotNoneAsync()
        {
            var option = Some(11);

            Task<bool> NoneFuncAsync() => Task.FromResult(false);
            bool SomeFunc(int value) => value == 11;

            var xxx = SynchronizationContext.Current;

            var hasCalledSome = await option.MatchAsync(NoneFuncAsync, SomeFunc)
                .ConfigureAwait(false);

            Assert.True(hasCalledSome);

        }

        [Fact]
        public async Task WhenIsNoneThenCallNoneAsyncAndNotSome()
        {
            var option = (Option<int>)Helpers.None;

            Task<bool> NoneFuncAsync() => Task.FromResult(true);
            bool SomeFunc(int value) => false;

            var hasCalledNone = await option.MatchAsync(NoneFuncAsync, SomeFunc)
                .ConfigureAwait(false);

            Assert.True(hasCalledNone);

        }

        [Fact]
        public void WhenSomeMatchMethodShouldNotCallNoneFunc()
        {
            Option<string> someOpt = Some("fake");

            var someCalled = someOpt.Match(() => false, v => true);

            Assert.True(someCalled);
        }

        [Fact]
        public void WhenValueIsNullSomeShouldRaiseError()
        {
            Assert.Throws<ArgumentNullException>(() => { _ = Some<string>(null); });
        }

        [Fact]
        public void ShouldReturnIEnumerableWithValuesWhenOptionIsSome()
        {
            Option<string> someOption = "fake";

            var enumerable = someOption.AsEnumerable();

            var count = enumerable.Count();

            Assert.Equal(1, count);
            Assert.IsAssignableFrom<IEnumerable<string>>(enumerable);
        }


        [Fact]
        public void ShouldNullObjectBeNotEqualToSomeButEqualToNone()
        {
            object obj1 = Some(1);
            object obj2 = Some(2);

            object objNone = (Option<int>)Helpers.None;
            object objNoneStr = (Option<string>)Helpers.None;

            var some = Some(1);
            Option<int> none = Helpers.None;

            Assert.True(some.Equals(obj1));
            Assert.False(some.Equals(obj2));
            Assert.False(some.Equals(objNone));

            Assert.False(none.Equals(obj1));
            Assert.True(none.Equals(objNone));
            Assert.False(none.Equals(objNoneStr));
        }

        [Fact]
        public void ShouldTwoNoneValuesOrNullBeEqual()
        {
            var first = (Option<int>) Helpers.None;
            var other = (Option<int>) Helpers.None;

            var actual = first.Equals(other);

            Assert.True(actual);
        }

        [Fact]
        public void ShouldNoneAndSomeNotBeEqual()
        {
            var first = (Option<int>) Helpers.None;
            var other = Some(1);

            var actual = first.Equals(other);

            Assert.False(actual);
        }

        [Fact]
        public void ShouldEqualityOperatorReturnCorrectOnOptionTypes()
        {
            var firstNone = (Option<int>) Helpers.None;
            var otherNone = (Option<int>) Helpers.None;

            var firstSome = Some(1);
            var otherSome = Some(1);

            var secondSome = Some(2);


            Assert.True(firstSome == otherSome);
            Assert.True(firstNone == otherNone);

            Assert.False(firstNone == firstSome);
            Assert.False(otherNone == otherSome);

            Assert.False(secondSome == firstSome);
            Assert.False(secondSome == firstNone);
        }

        [Fact]
        public void ShouldInEqualityOperatorReturnCorrectOnOptionTypes()
        {
            var firstNone = (Option<int>) Helpers.None;
            var otherNone = (Option<int>) Helpers.None;

            var firstSome = Some(1);
            var otherSome = Some(1);

            var secondSome = Some(2);


            Assert.False(firstSome != otherSome);
            Assert.False(firstNone != otherNone);

            Assert.True(firstNone != firstSome);
            Assert.True(otherNone != otherSome);

            Assert.True(secondSome != firstSome);
            Assert.True(secondSome != firstNone);
        }

        [Fact]
        public void ShouldEqualityOperatorReturnTrueOnOptionAndPrimitiveTypes()
        {
            Option<string> none = Helpers.None;
            var firstSome = Some("1");
            string str = null; // equals to None

            
            Assert.True( firstSome == "1");
            Assert.True(none == str);

            Assert.False(firstSome == "2");
            Assert.False(firstSome == str);


            Assert.True( "1"== firstSome );
            Assert.True(str== none );

            Assert.False("2"== firstSome );
            Assert.False(str== firstSome );

        }

        [Fact]
        public void ShouldInEqualityOperatorReturnTrueOnOptionAndPrimitiveTypes()
        {
            Option<string> none = Helpers.None;
            var firstSome = Some("1");
            string str = null; // equals to None

            
            Assert.False( firstSome != "1");
            Assert.False(none != str);

            Assert.True(firstSome != "2");
            Assert.True(firstSome != str);


            Assert.False( "1"!= firstSome );
            Assert.False(str!= none );

            Assert.True("2"!= firstSome );
            Assert.True(str!= firstSome );

        }

        [Fact]
        public void ToString_ShouldPrintNoneWhenNone()
        {
            var none = (Option<int>)Helpers.None;
            var nonePure = Helpers.None;

            Assert.Equal(none.ToString() , $"None");
            Assert.Equal(nonePure.ToString() , $"None");

        }

        [Fact]
        public void ToString_ShouldPrintSomeWhenHasValues()
        {
            var none = Some(1);
            
            Assert.Equal(none.ToString() , $"Some(1)");
        }

        [Fact]
        public void Equals_ShouldBeTrueWhenNoneIsPassedAndOptionIsAlsoNone()
        {
            var none = new None();

            Option<int> optStr = Helpers.None;

            var isEqual = optStr.Equals(none);

            Assert.True(isEqual);

        }

        [Fact]
        public void Equals_ShouldBeFalseWhenNoneIsPassedAndOptionsIsNotNone()
        {
            var none = new None();

            Option<int> optStr = Some(1);

            var isEqual = optStr.Equals(none);

            Assert.False(isEqual);

        }


        [Fact]
        public void Select_should_map_an_option_to_another_option()
        {
            var someStr = Some("value");

            var result = from s in someStr select s.ToUpper();

            Assert.False(result == Funx.Helpers.None);
            Assert.Equal("VALUE", result);

        }

        [Fact]
        public void Select_should_map_an_option_to_None_when_option_is_none()
        {
            Option<string> none = Helpers.None;

            var result = from s in none select s.ToUpper();

            Assert.True(result == Funx.Helpers.None);
        }

        [Fact]
        public void IfNone_should_call_the_none_action_passed_when_there_is_Nothing()
        {
            var none = Option<int>.None;

            var isCalled = false;

            none
                .WhenNone(() => isCalled = true)
                .WhenSome(_ => isCalled = false);
            

            Assert.True(isCalled);
        }
        [Fact]
        public void IfNone_should_not_call_the_none_action_passed_when_there_is_a_value()
        {
            Option<int> iSome = 1;

            var isCalled = false;

            iSome
                .WhenNone(() => isCalled = true)
                .WhenSome(_ => isCalled = false);

            Assert.False(isCalled);
        }
        [Fact]
        public async Task IfNoneAsync_should_call_the_noneAsync_action_passed_when_there_is_Nothing()
        {
            var none = Option<int>.None;

            var isCalled = false;

            await none.WhenNoneAsync(() =>
            {
                isCalled = true;
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            Assert.True(isCalled);
        }
        [Fact]
        public async Task IfNoneAsync_should_not_call_the_noneAsync_action_passed_when_there_is_a_value()
        {
            Option<int> iSome = 1;

            var isCalled = false;

            await iSome.WhenNoneAsync(() =>
            {
                isCalled = true;
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            Assert.False(isCalled);
        }


        [Fact]
        public void IfSome_should_call_the_some_action_passed_when_there_is_a_value()
        {
            Option<int> iSome = 1;

            var isCalled = false;

            iSome.WhenSome((v) =>
            {
                isCalled = true;
                Assert.Equal(1,v);
            }); 

            Assert.True(isCalled);
        }
        [Fact]
        public void IfSome_should_not_call_the_some_action_passed_when_there_is_Nothing()
        {
            var none = Option<int>.None;

            var isCalled = false;

            none.WhenSome((v) =>
            {
                isCalled = true;
            });

            Assert.False(isCalled);
        }

        [Fact]
        public async  Task IfSomeAsync_should_call_the_someAsync_action_passed_when_there_is_a_value()
        {
            Option<int> iSome = 1;

            var isCalled = false;

            await iSome.WhenSomeAsync((v) =>
            {
                isCalled = true;
                Assert.Equal(1,v);
                return Task.CompletedTask;
            }).ConfigureAwait(false); 

            Assert.True(isCalled);
        }
        [Fact]
        public async Task IfSomeAsync_should_not_call_the_someAsync_action_passed_when_there_is_Nothing()
        {
            var none = Option<int>.None;

            var isCalled = false;

            await none.WhenSomeAsync((v) =>
            {
                isCalled = true;
                return Task.CompletedTask;
            }).ConfigureAwait(false);

            Assert.False(isCalled);
        }

        [Fact]
        public void ToEither_should_return_either_with_right_side_if_options_is_not_none()
        {
            var options = Option<int>.Some(11);

            var either = options.ToEither<string>(() => "invalid number");

            either.WhenRight(v => Assert.Equal(11, v));
            either.WhenLeft(_ => Assert.False(true));
        }

        [Fact]
        public void ToEither_should_call_the_leftFactory_method_when_none()
        {
            var options = Option<int>.None;

            var either = options.ToEither<string>(() => "invalid number");

            either.WhenRight(v => Assert.True(false));
            either.WhenLeft(msg => Assert.Equal("invalid number" , msg));
        }

        [Fact]
        public void Unwrap_should_return_the_value()
        {
            Option<int> option = 11;

            var value = option.Unwrap();

            Assert.Equal(11,value);

        }

        [Fact]
        public void Unwrap_should_return_the_provided_default_when_none()
        {
            Option<int> option = Helpers.None;

            var value = option.Unwrap(11);

            Assert.Equal(11,value);
        }
        [Fact]
        public void Unwrap_should_return_the_value_when_in_some_state()
        {
            Option<int> option = 11;

            var value = option.Unwrap(12);

            Assert.Equal(11,value);
        }

        [Fact]
        public void Unwrap_should_call_the_default_factory_when_none()
        {
            Option<int> option = Helpers.None;

            var value = option.Unwrap(() => 11);

            Assert.Equal(11,value);
        }
        
        [Fact]
        public void Unwrap_should_return_the_value_and_not_call_the_default_factory_when_some()
        {
            Option<int> option = 11; 
            var value = option.Unwrap(() => 0);

            value.Should().Be(11);
        }

        [Fact]
        public async Task UnwrapAsync_should_call_the_default_factory_when_none()
        {
            Option<int> option = Helpers.None;

            var value = await option.UnwrapAsync(() => Task.FromResult(11))
                .ConfigureAwait(false);

            Assert.Equal(11,value);
        }
        [Fact]
        public async Task UnwrapAsync_should_return_value_and_not_call_the_default_factory_when_some()
        {
            Option<int> option = 11;

            var value = await option.UnwrapAsync(() => Task.FromResult(0))
                .ConfigureAwait(false);

            value.Should().Be(11);
        }

        [Fact]
        public void ToEither_should_return_an_either_with_right_value_as_value_of_option()
        {
            var option = Option<int>.Some(11);

            var either = option.ToEither(() => "invalid value");

            either.Should().BeAssignableTo<Either<string, int>>();
            either.IsRight.Should().BeTrue();
            either.WhenRight(v => v.Should().Be(11));
            
        }

        [Fact]
        public void ToEither_should_return_an_either_with_left_value()
        {
            var option = Option<int>.None;

            var either = option.ToEither(() => "invalid value");

            either.Should().BeAssignableTo<Either<string, int>>();
            either.IsLeft.Should().BeTrue();
            either.WhenLeft(v => v.Should().Be("invalid value"));

        }

    }
}
