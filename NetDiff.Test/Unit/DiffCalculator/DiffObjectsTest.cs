using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Extensions;

namespace NetDiff.Test.Unit.DiffCalculator
{
    [TestClass]
    public class DiffObjectsTest
    {

        [TestMethod]
        public void ObjectsMayBeOfDifferentType()
        {
            var baseObject = 2;
            var antagonist = "hi";

            var result = baseObject.DiffAgainst(antagonist);

            Assert.AreEqual(false, result.ValuesMatch);
        }

        [TestMethod]
        public void AntagonistIsNotIterable()
        {
            var baseObject = new List<int> {1, 2, 3};
            var antagonist = 2;

            var result = baseObject.DiffAgainst(antagonist);

            Assert.AreEqual("Types are not both iterable", result.Message);
        }
    }
}
