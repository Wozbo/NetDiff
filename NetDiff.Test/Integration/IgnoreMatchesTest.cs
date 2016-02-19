using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Model;

namespace NetDiff.Test.Integration
{
    [TestClass]
    public class IgnoredMatchesTest
    {
        [TestMethod]
        public void AllMatchesAreIgnored()
        {
            var calculator = new DiffCalculator(ignoreMatches: true);
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 5.1, 2.1, 4.1 };

            var result = calculator.Diff(a, b);
            var resultMatch = result.ValuesMatch;
            var items = ((ObjectDiff)result).Items;

            Assert.IsFalse(resultMatch);
            Assert.AreEqual(2, items.Count());
        }

        [TestMethod]
        public void ItemsFieldIsEmptySet()
        {
            var calculator = new DiffCalculator(ignoreMatches: true);
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 1.1, 2.1, 3.1 };

            var result = calculator.Diff(a, b);

            var items = ((ObjectDiff)result).Items;

            Assert.IsTrue(result.ValuesMatch);
            Assert.IsFalse(items.Any());
        }

    }
}
