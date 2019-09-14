using Funx.Extensions;
using System;
using Xunit;

namespace Funx.Tests.Extensions
{
    public class ActionExtensionsTests
    {
        [Fact]
        public void ShouldToFuncCallTheAction()
        {
            var called = false;
            Action parameterLessAction = () => { called = true; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<ValueTuple>>(result);

            result();

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith1Parameter()
        {
            var called = false;
            Action<bool> parameterLessAction = (t) => { called = !t; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, ValueTuple>>(result);

            result(called);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith2Parameters()
        {
            var called = false;
            Action<bool, bool> parameterLessAction = (t1, t2) => { called = !t1 && t2; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, ValueTuple>>(result);

            result(called, true);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith3Parameters()
        {
            var called = false;
            Action<bool, bool, bool> parameterLessAction = (t1, t2, t3) => { called = !t1 && t2 && t3; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, bool, ValueTuple>>(result);

            result(called, true, true);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith4Parameters()
        {
            var called = false;
            Action<bool, bool, bool, bool> parameterLessAction = (t1, t2, t3, t4) =>
            {
                called = !t1 && t2 && t3 && t4;
            };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, bool, bool, ValueTuple>>(result);

            result(called, true, true, true);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith5Parameters()
        {
            var called = false;
            Action<bool, bool, bool, bool, bool> parameterLessAction =
                (t1, t2, t3, t4, t5) => { called = !t1 && t2 && t3 && t4 && t5; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, bool, bool, bool, ValueTuple>>(result);

            result(called, true, true, true, true);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith6Parameters()
        {
            var called = false;
            Action<bool, bool, bool, bool, bool, bool> parameterLessAction =
                (t1, t2, t3, t4, t5, t6) => { called = !t1 && t2 && t3 && t4 && t5 && t6; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, bool, bool, bool, bool, ValueTuple>>(result);

            result(called, true, true, true, true, true);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith7Parameters()
        {
            var called = false;
            Action<bool, bool, bool, bool, bool, bool, bool> parameterLessAction =
                (t1, t2, t3, t4, t5, t6, t7) => { called = !t1 && t2 && t3 && t4 && t5 && t6 && t7; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, bool, bool, bool, bool, bool, ValueTuple>>(result);

            result(called, true, true, true, true, true, true);

            Assert.True(called);
        }

        [Fact]
        public void ShouldToFuncCallTheActionWith8Parameters()
        {
            var called = false;
            Action<bool, bool, bool, bool, bool, bool, bool, bool> parameterLessAction =
                (t1, t2, t3, t4, t5, t6, t7, t8) => { called = !t1 && t2 && t3 && t4 && t5 && t6 && t7 && t8; };

            var result = parameterLessAction.ToFunc();

            Assert.IsAssignableFrom<Func<bool, bool, bool, bool, bool, bool, bool, bool, ValueTuple>>(result);

            result(called, true, true, true, true, true, true, true);

            Assert.True(called);
        }
    }
}