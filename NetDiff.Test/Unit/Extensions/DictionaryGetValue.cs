using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Extensions;
using Xunit;

namespace NetDiff.Test.Unit.Extensions
{
    public class DictionaryGetValue
    {
        [Fact]
        public void ReturnsNullableValue()
        {
            var dict = new Dictionary<Type, string>() {{typeof(int), "1"}, {typeof(double), "2.0"}};

            Assert.Equal("1", dict.GetValue(typeof(int)));
            Assert.Equal(null, dict.GetValue(typeof(string)));

        }
    }
}
