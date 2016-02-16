using System;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Test.TestObjects;

namespace NetDiff.Test
{
    [TestClass]
    public class ObjectDiffTest
    {
        private DiffCalculator _calculator;

        [TestInitialize]
        public void Initialize()
        {
            _calculator = new DiffCalculator();
        }
        
        [TestMethod]
        public void DiffObjects_ShowsObjectsAreTrueIfTheyMatch()
        {
            var identicalString = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalString,
                secondString: identicalString);

            var evaluatedObject = new GenericDynamicObject(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalString,
                secondString: identicalString);

            var result = _calculator.DiffObjects(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.IsTrue(result.ValuesMatch);
        }

        [TestMethod]
        public void DiffObjects_EqualityReflectedAcrossDifferentObjects()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var evaluatedObject = new GenericDynamicObjectCopy(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalStrings,
                secondString: identicalStrings);

            var result = _calculator.DiffObjects(
                baseObj: baseObj,
                evaluated: evaluatedObject);

            Assert.IsTrue(result.ValuesMatch);
        }

        [TestMethod]
        public void DiffObjects_RecursesAppropriately()
        {
            var identicalStrings = "These strings are identical";
            var notIdenticalString = "These strings are not identical";
            var identicalNumber = 0.000089;
            var identicalNumberOffset = 0.000000000000123;

            var baseObj = new GenericDynamicObject(
                num: identicalNumber,
                pubString: identicalStrings,
                secondString: identicalStrings,
                subobj: new SubObject("I am equal"));

            var equalObj = new GenericDynamicObjectCopy(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalStrings,
                secondString: identicalStrings,
                subobj: new SubObject("I am equal"));

            var notEqualObj = new GenericDynamicObjectCopy(
                num: identicalNumber + identicalNumberOffset,
                pubString: identicalStrings,
                secondString: identicalStrings,
                subobj: new SubObject("I am not equal"));

            var equalResult = _calculator.DiffObjects(
                baseObj: baseObj,
                evaluated: equalObj);

            var notEqualResult = _calculator.DiffObjects(
                baseObj: baseObj,
                evaluated: notEqualObj);

            Assert.IsTrue(equalResult.ValuesMatch);
            Assert.IsFalse(notEqualResult.ValuesMatch);
        }
    }
}
