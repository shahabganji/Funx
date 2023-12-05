using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funx.Extensions;
using Xunit;
using static Funx.Factories;

namespace Funx.Tests.Extensions
{
    public class OptionExtensionsSpec
    {
        [Fact]
        public void Where_ShouldReturnNoneWhenPredicateReturnsFalse()
        {
            var evenNumber = Some(2);

            var actual = evenNumber.Where(IsOdd);

            Assert.Equal(actual, None);
            return;

            bool IsOdd(int v) => v % 2 != 0;
        }

        [Fact]
        public void Where_ShouldReturnSomeWhenPredicateReturnsFalse()
        {
            var evenNumber = Some(2);

            var actual = evenNumber.Where(IsOdd);

            Assert.Equal(actual, evenNumber);
            return;

            bool IsOdd(int v) => v % 2 == 0;
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

            var upper = await  lower.MapAsync(ToUpperAsync).ConfigureAwait(false);

            Assert.True(upper != None);
            upper.ForEach(value => Assert.Equal(value, $"TEST"));
            return;

            Task<string> ToUpperAsync(string value) => Task.FromResult(value.ToUpper());
        }

        [Fact]
        public async Task MapAsync_Should_Map_Option_Of_T_To_None()
        {
            Option<string> lower = None;

            var upper = await  lower.MapAsync(ToUpperAsync).ConfigureAwait(false);

            Assert.True(upper == None);
            return;

            Task<string> ToUpperAsync(string value) => Task.FromResult(value.ToUpper());
        }

        [Fact]
        public async Task MapAsync_Should_ThrowException()
        {
            Option<string> lower = Some("value");

            await Assert.ThrowsAsync<InvalidOperationException>(() => lower.MapAsync(ToUpperAsync)).ConfigureAwait(false);
            return;

            Task<string> ToUpperAsync(string value) => throw new InvalidOperationException("invalid");
        }

        [Fact]
        public void Bind_Should_Bind_Between_IEnumerable_Of_Option()
        {
            var name = Some(new[]{"John" , "Jane"});

            var result = name.Bind(ToCharacters);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<char>>(result);
            return;

            IEnumerable<char> ToCharacters(string[] names) => names.SelectMany(n => n.ToCharArray());
        }

        [Fact]
        public void Bind_ShouldReturnTheValueOfTheProvidedFunctionWithNoWrapper()
        {
            var lower = Some("test");

            var upper = lower.Bind(ToUpper);

            Assert.IsAssignableFrom<Option<string>>(upper);

            upper.ForEach((value) => { Assert.Equal(value, $"TEST"); });
            return;

            Option<string> ToUpper(string value) => value.ToUpper();
        }

        [Fact]
        public void Bind_ShouldReturnNoneWhenTheMainOptionIsNoneAndNotCallSomeFunc()
        {
            Option<string> lower = None;

            var upper = lower.Bind(ToUpper);

            Assert.IsAssignableFrom<Option<bool>>(upper);
            Assert.True(upper == None);
            return;

            Option<bool> ToUpper(string value) => false;
        }

        [Fact]
        public async Task BindAsync_ShouldReturnTheValueOfTheProvidedFunctionWithNoWrapper()
        {
            var lower = Some("test");

            var upperTask = lower.BindAsync(ToUpperAsync);

            await Assert.IsAssignableFrom<Task<Option<string>>>(upperTask).ConfigureAwait(false);

            var upper = await upperTask.ConfigureAwait(false);;

            Assert.IsAssignableFrom<Option<string>>(upper);

            upper.ForEach((value) => { Assert.Equal(value, $"TEST"); });
            return;

            Task<Option<string>> ToUpperAsync(string value) => Task.FromResult(Some(value.ToUpper()));
        }

        [Fact]
        public async Task BindAsyncShouldReturnNoneWhenTheMainOptionIsNoneAndNotCallSomeFunc()
        {
            Option<string> lower = None;

            var upperTask = lower.BindAsync(ToUpperAsync);

            await Assert.IsAssignableFrom<Task<Option<bool>>>(upperTask).ConfigureAwait(false);

            var upper = await upperTask.ConfigureAwait(false);

            Assert.IsAssignableFrom<Option<bool>>(upper);
            Assert.True(upper == None);
            return;

            Task<Option<bool>> ToUpperAsync(string value) => Task.FromResult(Some(false));
        }

    }
}
