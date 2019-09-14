using Funx.Extensions;
using System.Collections.Specialized;
using Xunit;

namespace Funx.Tests.Extensions
{
    public class NameValueCollectionExtensionsTests
    {
        [Fact]
        public void ShouldReturnAnOptionWhenValueExists()
        {
            var dictionary = new NameValueCollection
            {
                {"One", "First"},
                {"Two", "Second"}
            };


            var result = dictionary.Lookup("One");

            _ = result.Match(null, value =>
            {
                Assert.Equal("First", value);
                return true;
            });
        }

        [Fact]
        public void ShouldReturnNoneWhenValueDoesNotExists()
        {
            var dictionary = new NameValueCollection
            {
                {"One", "First"},
                {"Two", "Second"}
            };


            var result = dictionary.Lookup("Three");

            _ = result.Match(() =>
            {
                Assert.True(true);
                return true;
            }, null);
        }
    }
}