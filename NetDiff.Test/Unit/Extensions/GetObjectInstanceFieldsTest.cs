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
    public class GetObjectInstanceFieldsTest
    {

        [TestMethod]
        public void GetObjectFields_YieldsOnlyObjects()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("An Object"));
            var result = baseObj.GetObjectInstanceFields();

            Assert.AreEqual(
                expected: 1,
                actual: result.Count());
        }
    }
}
