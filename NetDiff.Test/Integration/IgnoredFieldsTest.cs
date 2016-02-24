using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Model;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Integration
{
    public class IgnoredFieldsTest
    {

        [Fact]
        public void IgnoresFieldsNamed()
        {
            var calculator = new DiffCalculator(
                ignoreFieldsNamed: new[] {"SecondString"});

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.True(resultObject.ValuesMatch);
        }


        [Fact]
        public void IgnoresFieldsContaining()
        {
            var calculator = new DiffCalculator(
    ignoreFieldsContaining: new[] { "String" });

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.True(resultObject.ValuesMatch);
        }



        [Fact]
        public void IgnoresFieldsNamedByType()
        {
            var calculator = new DiffCalculator(ignoreFieldsNamedByType: new Dictionary<Type, string[]> {{typeof (SlightlyDifferentObject), new[] {"SecondString"}}});

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.True(resultObject.ValuesMatch);
        }


        [Fact]
        public void IgnoresFieldsContainingByType()
        {
            var calculator = new DiffCalculator(ignoreFieldsContainingByType: new Dictionary<Type, string[]> { { typeof(SlightlyDifferentObject), new[] { "String" } } });

            var a = new SlightlyDifferentObject(secondString: "hi");
            var b = new SlightlyDifferentObject(secondString: "there");

            var resultObject = calculator.Diff(a, b);

            Assert.True(resultObject.ValuesMatch);
        }


    }
}
