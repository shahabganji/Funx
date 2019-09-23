using System;
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
        public void ShouldMapAndSelectBeIdentical()
        {
            var numbers = Enumerable.Range(0, 10).ToArray();

            bool LessThanFive(int i) => i < 5;

            var selectResults = numbers.Select(LessThanFive).ToArray();
            var mapResult = numbers.Map(LessThanFive).ToArray();

            Assert.Equal(selectResults.Count(), mapResult.Count());

            selectResults.ForEach((item, index) => Assert.Equal(item, mapResult[index]));
        }

        [Fact]
        public void ShouldBindAndSelectManyBeIdentical()
        {
            IEnumerable<Subject> population = new List<Subject>()
            {
                new Subject() {Age = Age.Of(20)},
                new Subject() { },
                new Subject() {Age = Age.Of(30)}
            };

            var manyItems = population.SelectMany(p => p.Age.AsEnumerable()).ToArray();
            var bindItems = population.Bind(x => x.Age).ToArray();

            Assert.Equal(manyItems.Count(), bindItems.Count());

            manyItems.ForEach((item, index) => Assert.Equal(item, bindItems[index]));
        }

        [Fact]
        public void ShouldReturnAnIEnumerable()
        {
            var empty = Return<string>();
            var list = Return("A", "B");

            var emptyCount = empty.Count();
            var listCount = list.Count();

            Assert.NotNull(empty);
            Assert.IsAssignableFrom<IEnumerable<string>>(empty);
            Assert.Equal(0, emptyCount);

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

        [Fact]
        public void ShouldSafeAnyReturnFalseOnNull()
        {
            IEnumerable<int> integers = null;

            var hasAnyElement = integers.SafeAny();

            Assert.False(hasAnyElement);
        }
        
        [Fact]
        public void ShouldSafeAnyReturnFalseOnEmpty()
        {
            IEnumerable<int> integers = new List<int>();

            var hasAnyElement = integers.SafeAny();

            Assert.False(hasAnyElement);
        }
        
        [Fact]
        public void ShouldSafeAnyReturnTrueWhenHavingElement()
        {
            IEnumerable<int> integers = new []{1,2,3};

            var hasAnyElement = integers.SafeAny();

            Assert.True(hasAnyElement);
        }

        [Fact]
        public void ShouldSafeAnyReturnFalseOnFailingPredicate()
        {
            IEnumerable<int> integers = new[] {1, 2, 3, 4};

            var hasAnyElement = integers.SafeAny(x => x > 5);

            Assert.False(hasAnyElement);
        }

        [Fact]
        public void ShouldSafeAnyReturnTrueOnPassingPredicate()
        {
            IEnumerable<int> integers = new[] {1, 2, 3, 4};

            var hasAnyElement = integers.SafeAny(x => x % 2 == 0);

            Assert.True(hasAnyElement);
        }

        [Fact]
        public void ForEach_ShouldRunForAllElements()
        {
            var numbers = new[] {1, 3, 5};

            var numbersList = new List<int>();

            numbers.ForEach( n => numbersList.Add(n) );

            numbersList.ForEach((value, index) => { Assert.Equal(value, numbers[index]); });

        }

    }
}
