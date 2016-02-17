using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Extensions;
using NetDiff.Test.TestObjects;

namespace NetDiff.Test.Unit.Extensions
{
    [TestClass]
    public class GetNonObjectInstanceFieldsTest
    {

        [TestMethod]
        public void GetNonObjectFields_YieldsOnlyNonObjects()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("An Object"));
            var result = baseObj.GetNonObjectInstanceFields();

            Assert.AreEqual(
                expected: 3,
                actual: result.Count());
        }
    }
}
