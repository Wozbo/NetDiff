using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;
using NetDiff.Extensions;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Unit.Extensions
{
    public class IsObjectFieldTest
    {


        [Fact]
        public void IsObjectField_DiscernsObject()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("derr"));

            var fields = baseObj.GetInstanceFields();
            var field = fields.First(n => n.Name.Equals("SubObj"));

            var result = baseObj.IsObjectField(field);

            Assert.True(result);
        }

    }
}
