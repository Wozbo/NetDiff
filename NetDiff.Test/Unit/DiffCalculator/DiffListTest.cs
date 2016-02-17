using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    }
}