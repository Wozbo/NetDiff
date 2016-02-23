using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Model;
using NetDiff.Test.TestObjects;

namespace NetDiff.Test.Integration
{
    [TestClass]
    public class IgnoredFieldsTest
    {

        [TestMethod]
        public void IgnoresFieldsNamed()
        {
            var calculator = new DiffCalculator(
                ignoreFieldsNamed: new[] {"SecondString"});

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.IsTrue(resultObject.ValuesMatch);
        }


        [TestMethod]
        public void IgnoresFieldsContaining()
        {
            var calculator = new DiffCalculator(
    ignoreFieldsContaining: new[] { "String" });

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.IsTrue(resultObject.ValuesMatch);
        }



        [TestMethod]
        public void IgnoresFieldsNamedByType()
        {
            var calculator = new DiffCalculator(ignoreFieldsNamedByType: new Dictionary<Type, string[]> {{typeof (SlightlyDifferentObject), new[] {"SecondString"}}});

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.IsTrue(resultObject.ValuesMatch);
        }


        [TestMethod]
        public void IgnoresFieldsContainingByType()
        {
            var calculator = new DiffCalculator(ignoreFieldsContainingByType: new Dictionary<Type, string[]> { { typeof(SlightlyDifferentObject), new[] { "String" } } });

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.IsTrue(resultObject.ValuesMatch);
        }


    }
}
