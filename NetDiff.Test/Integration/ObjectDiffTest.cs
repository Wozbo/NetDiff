using System.Collections.Generic;
using System.Linq;
using NetDiff.Model;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Integration
{
    public class ObjectDiffTest
    {
        private DiffCalculator _calculator;

        public ObjectDiffTest()
        {
            _calculator = new DiffCalculator();
        }

        [Fact]
        public void ObjectWithListMatches()
        {

            var a =
                new ObjectWithList
                {
                    Number = 2,
                    String = "string",
                    Decimal = 20.000000000000000000001,
                    List = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }
                };

            var b =
                new ObjectWithList
                {
                    Number = 2,
                    String = "string",
                    Decimal = 20.000000000000000000001,
                    List = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }
                };


            var result = _calculator.Diff(a, b);

            Assert.True(result.ValuesMatch);
        }

        [Fact]
        public void ObjectWithListDoesNotMatch()
        {
            var a = new ObjectWithList
                    {
                        Number = 2,
                        String = "string",
                        Decimal = 20.000000000000000000001,
                        List = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9}
                    };

            var b = new ObjectWithList
                    {
                        Number = 2,
                        String = "string",
                        Decimal = 20.000000000000000000001,
                        List = new List<int> {1, 2, 3, 4, 5, 6, 7, 8}
                    };


            var result = _calculator.Diff(a, b) as ObjectDiff;

            result.WithoutMatching();

            Assert.False(result.ValuesMatch);
        }

        [Fact]
        public void ObjectWithPropertyMatches()
        {
            var a = new ObjectWithProperties
            {
                Number = 2,
                String = "string",
                SomeIntProperty = 2
            };

            var b = new ObjectWithProperties
            {
                Number = 2,
                String = "string"
            };


            var result = _calculator.Diff(a, b);

            Assert.True(result.ValuesMatch);
        }

        [Fact]
        public void ObjectWithNestedObjectMatches()
        {
            var a = new NestingObject
            {
                Number = 2,
                String = "string",
                SomeIntProperty = 2,
                SomeObject = new ObjectWithProperties
                {
                    Number = 2,
                    String = "string"
                }
            };

            var b = new NestingObject
            {
                Number = 2,
                String = "string",
                SomeObject = new ObjectWithProperties
                {
                    Number = 2,
                    String = "string"
                }
            };


            var result = _calculator.Diff(a, b);

            Assert.True(result.ValuesMatch);
        }

        [Fact]
        public void ObjectWithNestedObjectDoesNotMatch()
        {
            var a = new NestingObject
            {
                Number = 2,
                String = "string",
                SomeObject = new ObjectWithProperties
                {
                    Number = 2,
                    String = "string"
                }
            };

            var b = new NestingObject
            {
                Number = 2,
                String = "string",
                SomeObject = new ObjectWithProperties
                {
                    Number = 2,
                    String = "some other string"
                }
            };


            var result = _calculator.Diff(a, b);

            Assert.False(result.ValuesMatch);
        }

        [Fact]
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

            Assert.True(result.ValuesMatch);
        }

        [Fact]
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

            Assert.True(result.ValuesMatch);
        }

        [Fact]
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

            Assert.True(equalResult.ValuesMatch);
            Assert.False(notEqualResult.ValuesMatch);
        }
    }
}
