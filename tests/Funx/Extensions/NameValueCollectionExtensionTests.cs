using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using Funx.Extensions;
using Xunit;

namespace Funx.Tests.Extensions
{
    public class NameValueCollectionExtensionsTests
    {
        [Fact]
        public void ShouldReturnAnOptionWhenValueExists()
        {
            var dictionary = new NameValueCollection();

            dictionary.Add("One", "First");
            dictionary.Add("Two", "Second");

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
            var dictionary = new NameValueCollection();

            dictionary.Add("One", "First");
            dictionary.Add("Two", "Second");

            var result = dictionary.Lookup("Three");

            _ = result.Match(() =>
            {
                Assert.True(true);
                return true;
            }, null);
        }
        
        
    }
}
