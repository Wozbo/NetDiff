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
    public class GetObjectInstanceFieldsTest
    {

        [Fact]
        public void GetObjectFields_YieldsOnlyObjects()
        {
            var baseObj = new GenericDynamicObject(subobj: new SubObject("An Object"));
            var result = baseObj.GetObjectInstanceFields();

            Assert.Equal(
                expected: 1,
                actual: result.Count());
        }
    }
}
