using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Funx.Option;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests
{
    public class OptionTests
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

            var noneCalled = await noneOpt.MatchAsync(NoneFuncAsync, SomeFuncAsync);

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

            var noneCalled = await someOpt.MatchAsync(NoneFuncAsync, SomeFuncAsync);

            Assert.True(noneCalled);
        }

        [Fact]
        public async Task WhenIsSomeThenCallSomeAsyncAndNotNone()
        {
            var option = Some(11);

            bool NoneFunc() => false;
            Task<bool> SomeAsync(int value) => Task.FromResult(value == 11);

            var xxx = SynchronizationContext.Current;

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

    }
}