using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Funx.Extensions;
using Funx.Tests.Mocks;
using Xunit;
using static Funx.Extensions.EnumerableExtensions;

namespace Funx.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ShouldBindAndSelectManyBeIdentical()
        {
            IEnumerable<Subject> population = new List<Subject>()
            {
                new Subject(){Age = Age.Of(20)},
                new Subject(){},
                new Subject(){Age = Age.Of(30)}
            };

            var manyItems = population.SelectMany(p => p.Age.AsEnumerable()).ToArray();
            var bindItems = population.Bind(x => x.Age).ToArray();

            Assert.Equal(manyItems.Count(), bindItems.Count());
            for (int i = 0; i < manyItems.Count(); i++)
            {
                Assert.Equal(manyItems[i].Value, bindItems[i].Value);
            }
        }

        [Fact]
        public void Should_Return_An_IEnumerable()
        {
            var empty = Return<string>();
            var list = Return("A", "B");

            var emptyCount = empty.Count();
            var listCount = list.Count();

            Assert.NotNull(empty);
            Assert.IsAssignableFrom<IEnumerable<string>>(empty);
            Assert.Equal(0 , emptyCount);

            Assert.NotNull(list);
            Assert.IsAssignableFrom<IEnumerable<string>>(list);
            Assert.Equal(2, listCount);
        }

        [Fact]
        public void ShouldReturnAnEmptyList()
        {
            var actual = Return<string>();
            
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ShouldReturnAListOfString()
        {
            var actual = Return("first", "second");
            
            Assert.NotNull(actual);
            
            var collection = actual.ToList();
            Assert.NotEmpty(collection);

            var listCount = collection.Count();
            
            Assert.Equal(2, listCount);
        }

    }
}
