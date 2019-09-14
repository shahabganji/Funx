using Funx.Extensions;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests.Extensions
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void ShouldReturnNoneWhenPredicateReturnsFalse()
        {
            bool IsOdd(int v) => v % 2 != 0;

            var evenNumber = Some(2);

            var actual = evenNumber.Where(IsOdd);

            Assert.Equal(actual, None);
        }

        [Fact]
        public void ShouldReturnSomeWhenPredicateReturnsFalse()
        {
            bool IsOdd(int v) => v % 2 == 0;

            var evenNumber = Some(2);

            var actual = evenNumber.Where(IsOdd);

            Assert.Equal(actual, evenNumber);
        }
    }
}