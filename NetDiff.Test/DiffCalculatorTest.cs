using System;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Test.TestObjects;

namespace NetDiff.Test
{
    [TestClass]
    public class DiffCalculatorTest
    {
        private DiffCalculator _calculator;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new DiffCalculator();
        }

        [TestMethod]
        public void Diff_ProducesAList()
        {
            var result = _calculator.Diff(
                baseObj: new GenericDynamicObject(), 
                evaluated: new GenericDynamicObject());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Diff_ListHasFieldsSet()
        {
            var basePublicString = "This is the base object";
            var evaluatedPublicString = "This is the evaluated object";

            var baseObj = new GenericDynamicObject(
                num: 10.0,
                pubString: basePublicString);

            var evaluatedObject = new GenericDynamicObject(
                num: 0.0,
                pubString: evaluatedPublicString);

            var result = _calculator.Diff(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.IsTrue(result.Any(
                  n => string.Equals(n.BaseObjValue, basePublicString)
                    && string.Equals(n.EvaluatedValue, evaluatedPublicString)));
        }
    }
}
