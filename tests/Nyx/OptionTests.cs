using System;
using Xunit;
using static Nyx.Helpers.OptionHelpers;

namespace Monad.Tests {

    public class OptionTests {
        [Fact]
        public void WhenValueIsNullSomeShouldRaiseError( )
        {
            Assert.Throws<ArgumentNullException>(() => { _ = Some<string>(null); });
        }
    }
}
