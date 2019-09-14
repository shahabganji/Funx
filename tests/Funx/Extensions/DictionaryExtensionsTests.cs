using System.Collections.Concurrent;
using System.Collections.Generic;
using Funx.Extensions;
using Xunit;

namespace Funx.Tests.Extensions
{
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void ShouldReturnAnOptionWhenValueExists()
        {
            var dictionary = new Dictionary<int, string>();

            dictionary.TryAdd(1, "First");
            dictionary.TryAdd(2, "Second");

            var result = dictionary.Lookup(1);

            _ = result.Match(null, value =>
            {
                Assert.Equal("First", value);
                return true;
            });
        }

        [Fact]
        public void ShouldReturnNoneWhenValueDoesNotExists()
        {
            var dictionary = new Dictionary<int, string>();

            dictionary.TryAdd(1, "First");
            dictionary.TryAdd(2, "Second");

            var result = dictionary.Lookup(3);

            _ = result.Match(() =>
            {
                Assert.True(true);
                return true;
            }, null);
        }
        
        [Fact]
        public void ShouldReturnAnOptionWhenValueExistsForConcurrentDictionary()
        {
            var dictionary = new ConcurrentDictionary<int, string>();

            dictionary.TryAdd(1, "First");
            dictionary.TryAdd(2, "Second");

            var result = dictionary.Lookup(1);

            _ = result.Match(null, value =>
            {
                Assert.Equal("First", value);
                return true;
            });
        }

        [Fact]
        public void ShouldReturnNoneWhenValueDoesNotExistsForConcurrentDictionary()
        {
            var dictionary = new ConcurrentDictionary<int, string>();

            dictionary.TryAdd(1, "First");
            dictionary.TryAdd(2, "Second");

            var result = dictionary.Lookup(3);

            _ = result.Match(() =>
            {
                Assert.True(true);
                return true;
            }, null);
        }
    }
}
