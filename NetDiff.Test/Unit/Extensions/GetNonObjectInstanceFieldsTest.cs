using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Extensions;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Unit.Extensions
{
    public class GetNonObjectInstanceFieldsTest
    {

        [Fact]
        public void GetNonObjectFields_YieldsOnlyNonObjects()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("An Object"));
            var result = baseObj.GetNonObjectInstanceFields();

            Assert.Equal(
                expected: 3,
                actual: result.Count());
        }
    }
}
