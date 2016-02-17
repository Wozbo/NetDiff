using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Extensions;
using NetDiff.Test.TestObjects;

namespace NetDiff.Test.Unit.Extensions
{
    [TestClass]
    public class IsObjectFieldTest
    {


        [TestMethod]
        public void IsObjectField_DiscernsObject()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("derr"));

            var fields = baseObj.GetInstanceFields();
            var field = fields.First(n => n.Name.Equals("SubObj"));

            var result = baseObj.IsObjectField(field);

            Assert.IsTrue(result);
        }

    }
}
