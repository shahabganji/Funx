using System;
using System.Threading.Tasks;
using Xunit;
using static Nyx.Helpers.OptionHelpers;

namespace Nyx.Tests
{
    public class OptionTests
    {
        [Fact]
        public void NoneShouldBeAnOptionOfT()
        {
            Option<bool> option = None;

            Assert.NotNull(option);
            Assert.IsType<Option<bool>>(option);
        }

        [Fact]
        public void SomeOfTShouldBeAnOptionOfT()
        {
            Option<bool> option = Some(true);

            Assert.NotNull(option);
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
    }
}