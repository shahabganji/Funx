using System;
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
        public void WhenNoneMatchMethodShouldCallNoneFunc()
        {
            Option<string> noneOpt = None;

            var noneCalled = noneOpt.Match(() => true, v => false);

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
