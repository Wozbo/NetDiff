using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Model;

namespace NetDiff.Test.Unit.DiffCalculator
{
    [TestClass]
    public class DiffListTest
    {

        [TestMethod]
        public void TwoListsAreEqual()
        {
            var a = new List<int> {1, 2, 3};
            var b = new List<int> {1, 2, 3};

            var calculator = new NetDiff.DiffCalculator();
            var result = calculator.DiffList(a.Cast<object>(), b.Cast<object>());

            Assert.AreEqual(3, result.Count());

            foreach (var item in result)
            {
                Assert.IsTrue(item.ValuesMatch);
            }

        }

        [TestMethod]
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

            Assert.AreEqual(
                expected: 3,
                actual: result.Count(n => n.Message.Equals(DiffMessage.DiffersInOrder)) );

            Assert.AreEqual(
                expected: 2,
                actual: secondResult.Count(n => n.Message.Equals(DiffMessage.DiffersInOrder)) );
        }


        [TestMethod]
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

            Assert.AreEqual(
                expected: 3,
                actual: result.Count(n => n.Message.Equals(DiffMessage.DiffersInOrder)));

            Assert.AreEqual(
                expected: 3,
                actual: secondResult.Count(n => n.Message.Equals(DiffMessage.DiffersInContent)));
        }

    }
}