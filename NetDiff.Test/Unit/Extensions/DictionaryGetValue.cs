using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Extensions;

namespace NetDiff.Test.Unit.Extensions
{
    [TestClass]
    public class DictionaryGetValue
    {
        [TestMethod]
        public void ReturnsNullableValue()
        {
            var dict = new Dictionary<Type, string>() {{typeof(int), "1"}, {typeof(double), "2.0"}};

            Assert.AreEqual("1", dict.GetValue(typeof(int)));
            Assert.AreEqual(null, dict.GetValue(typeof(string)));

        }
    }
}
