using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDiff.Test.Integration
{
    [TestClass]
    public class ListDiffTest
    {
        private DiffCalculator _calculator;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new DiffCalculator();
        }

        [TestMethod]
        public void ListIsTheSame()
        {
            var a = new List<string> {"hi", "there"};
            var b = new List<string> {"hi", "there"};

            var result = _calculator.Diff(a, b);

            Assert.IsTrue(result.ValuesMatch);
        }

        [TestMethod]
        public void ListIsDifferent()
        {
            var a = new List<string> { "hi", "there" };
            var b = new List<string> { "Hi", "there" };

            var result = _calculator.Diff(a, b);

            Assert.IsFalse(result.ValuesMatch);

        }

        [TestMethod]
        public void ListHasAllTheSameElementsButInADifferentOrder()
        {
            var a = new List<string> { "hi", "there" };
            var b = new List<string> { "there", "hi"};

            var result = _calculator.Diff(a, b);

            Assert.IsFalse(result.ValuesMatch);

        }
    }
}
