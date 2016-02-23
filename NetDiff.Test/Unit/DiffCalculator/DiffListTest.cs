using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Model;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Unit.DiffCalculator
{
    public class DiffListTest
    {

        [Fact]
        public void TwoListsAreEqual()
        {
            var a = new List<int> {1, 2, 3};
            var b = new List<int> {1, 2, 3};

            var calculator = new NetDiff.DiffCalculator();
            var result = calculator.DiffList(a.Cast<object>(), b.Cast<object>());

            Assert.Equal(3, result.Count());

            foreach (var item in result)
            {
                Assert.True(item.ValuesMatch);
            }

        }

        [Fact]
        public void TwoListsHaveDifferentOrder()
        {
            var a = new List<int> { 1, 2, 3 };
            var b = new List<int> { 2, 3, 1 };
            var c = new List<int> { 1, 3, 2 };

            var calculator = new NetDiff.DiffCalculator();
            var result = calculator.DiffList(
                baseObj:    a.Cast<object>(),
                antagonist: b.Cast<object>());

            var secondResult = calculator.DiffList(
                baseObj:    a.Cast<object>(),
                antagonist: c.Cast<object>());

            Assert.Equal(
                expected: 3,
                actual: result.Count(n => n.Message.Equals(DiffMessage.DiffersInOrder)));

            Assert.Equal(
                expected: 2,
                actual: secondResult.Count(n => n.Message.Equals(DiffMessage.DiffersInOrder)));

            Assert.Equal(
                expected: 1,
                actual: secondResult.Count(n => n.Message.Equals(DiffMessage.NotApplicable)));
        }


        [Fact]
        public void TwoListsDemonstrateThatContentDiffers()
        {
            var a = new List<int> { 1, 2, 3 };
            var b = new List<int> { 2, 3, 1 };
            var c = new List<int> { 3, 3, 2 };

            var calculator = new NetDiff.DiffCalculator();
            var result = calculator.DiffList(
                baseObj: a.Cast<object>(),
                antagonist: b.Cast<object>());

            var secondResult = calculator.DiffList(
                baseObj: a.Cast<object>(),
                antagonist: c.Cast<object>());

            Assert.Equal(
                expected: 3,
                actual: result.Count(n => n.Message.Equals(DiffMessage.DiffersInOrder)));

            Assert.Equal(
                expected: 3,
                actual: secondResult.Count(n => n.Message.Equals(DiffMessage.DiffersInContent)));
        }

        [Fact]
        public void DiffListCannotSortListsOfObjects()
        {
            var a = new List<SubObject>
            {
                new SubObject("a"),
                new SubObject("b"),
                new SubObject("c")
            };
            var b = new List<SubObject>
            {
                new SubObject("c"),
                new SubObject("b"),
                new SubObject("a")
            };

            var calculator = new NetDiff.DiffCalculator();
            var result = calculator.DiffList(
                baseObj: a,
                antagonist: b);

            Assert.Equal(
                expected: 2,
                actual: result.Count(n => n.Message.Equals(DiffMessage.DiffersInContent)));
        }

    }
}