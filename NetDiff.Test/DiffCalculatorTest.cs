﻿using System;
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
        public void Intersect_ProducesAList()
        {
            var result = _calculator.Intersect(
                baseObj: new GenericDynamicObject(), 
                evaluated: new GenericDynamicObject());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Intersect_ListHasFieldsSet()
        {
            var basePublicString = "This is the base object";
            var evaluatedPublicString = "This is the evaluated object";

            var baseObj = new GenericDynamicObject(
                num: 10.0,
                pubString: basePublicString);

            var evaluatedObject = new GenericDynamicObject(
                num: 0.0,
                pubString: evaluatedPublicString);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.IsTrue(result.Any(
                  n => string.Equals(n.BaseObjValue, basePublicString)
                    && string.Equals(n.EvaluatedValue, evaluatedPublicString)));
        }

        [TestMethod]
        public void Intersect_ListDisplaysAppropriateNumberOfEqualFields()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new GenericDynamicObject(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalStrings,
                secondString: notIdenticalString);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.AreEqual(
                expected: 2,
                actual: result.Count(n => n.ValuesMatch));

            Assert.AreEqual(
                expected: 1,
                actual: result.Count(n => !n.ValuesMatch));
        }

        [TestMethod]
        public void Intersection_YieldsOnlyCommonFields()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new SlightlyDifferentObject(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalStrings,
                secondString: notIdenticalString);

            var result = _calculator.Intersect(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.AreEqual(
                expected: 2,
                actual: result.Count());
        }
    }
}
