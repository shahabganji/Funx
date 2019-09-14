using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests
{
    public class OptionTests
    {
        [Fact]
        public void NoneShouldBeAnOptionOfT()
        {
            Option<bool> option = None;

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
            Option<string> noneOpt = None;

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
            Option<string> noneOpt = None;

            var noneCalled = noneOpt.Match(() => true, v => false);

            Assert.True(noneCalled);
        }

        [Fact]
        public async Task WhenSomeMatchAsyncMethodShouldNotCallNoneFuncAsync()
        {
            Option<string> someOpt = Some("fake");

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
        public void ShouldTwoNoneValuesOrNullBeEqual()
        {
            var first = (Option<int>) None;
            var other = (Option<int>) None;

            var actual = first.Equals(other);

            Assert.True(actual);
        }

        [Fact]
        public void ShouldNoneAndSomeNotBeEqual()
        {
            var first = (Option<int>) None;
            var other = Some(1);

            var actual = first.Equals(other);

            Assert.False(actual);
        }

        [Fact]
        public void ShouldEqualityOperatorReturnTrueOnOptionTypes()
        {
            var firstNone = (Option<int>) None;
            var otherNone = (Option<int>) None;

            var actualNone = firstNone == otherNone;

            var firstSome = Some(1);
            var otherSome = Some(1);

            var actualSome = firstSome == otherSome;

            Assert.True(actualNone);
            Assert.True(actualSome);
        }

        [Fact]
        public void ShouldEqualityOperatorReturnTrueOnOptionAndPrimitiveTypes()
        {
            var firstSome = Some(1);
            var other = 1;

            var actualSome = firstSome == other;

            Assert.True(actualSome);
        }
    }
}