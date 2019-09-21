using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funx.Extensions;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests.Extensions
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void Where_ShouldReturnNoneWhenPredicateReturnsFalse()
        {
            bool IsOdd(int v) => v % 2 != 0;

            var evenNumber = Some(2);

            var actual = evenNumber.Where(IsOdd);

            Assert.Equal(actual, Helpers.None);
        }

        [Fact]
        public void Where_ShouldReturnSomeWhenPredicateReturnsFalse()
        {
            bool IsOdd(int v) => v % 2 == 0;

            var evenNumber = Some(2);

            var actual = evenNumber.Where(IsOdd);

            Assert.Equal(actual, evenNumber);
        }

        [Fact]
        public void Map_Should_Map_Option_Of_T_To_Option_Of_TR()
        {
            var lower = Some("test");

            var upper = lower.Map(t => t.ToUpper());

            Assert.True(upper != None);
            upper.ForEach(value => Assert.Equal(value, $"TEST"));
        }
        [Fact]
        public void Map_Should_Map_Option_Of_T_To_None()
        {
            Option<string> lower = None;

            var upper = lower.Map(t => t.ToUpper());

            Assert.True(upper == None);
        }

        [Fact]
        public async Task MapAsync_Should_Map_Option_Of_T_To_Option_Of_TR()
        {
            var lower = Some("test");

            Task<string> ToUpperAsync(string value) => Task.FromResult(value.ToUpper());

            var upper = await  lower.MapAsync(ToUpperAsync).ConfigureAwait(false);

            Assert.True(upper != None);
            upper.ForEach(value => Assert.Equal(value, $"TEST"));
        }

        [Fact]
        public async Task MapAsync_Should_Map_Option_Of_T_To_None()
        {
            Option<string> lower = None;

            Task<string> ToUpperAsync(string value) => Task.FromResult(value.ToUpper());

            var upper = await  lower.MapAsync(ToUpperAsync).ConfigureAwait(false);

            Assert.True(upper == None);
        }

        [Fact]
        public async Task MapAsync_Should_ThrowException()
        {
            Option<string> lower = Some("value");

            Task<string> ToUpperAsync(string value) => throw new InvalidOperationException("invalid");

            await Assert.ThrowsAsync<InvalidOperationException>(() => lower.MapAsync(ToUpperAsync)).ConfigureAwait(false);
        }

        [Fact]
        public void Bind_Should_Bind_Between_IEnumerable_Of_Option()
        {
            var name = Some(new[]{"John" , "Jane"});
            
            IEnumerable<char> ToCharacters(string[] names) => names.SelectMany(n => n.ToCharArray());

            var result = name.Bind(ToCharacters);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<char>>(result);
        }

    }
}