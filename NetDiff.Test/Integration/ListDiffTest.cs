using System.Collections.Generic;
using Xunit;

namespace NetDiff.Test.Integration
{
    public class ListDiffTest
    {
        private DiffCalculator _calculator;

        public ListDiffTest()
        {
            _calculator = new DiffCalculator();
        }

        [Fact]
        public void ListIsTheSame()
        {
            var a = new List<string> {"hi", "there"};
            var b = new List<string> {"hi", "there"};

            var result = _calculator.Diff(a, b);

            Assert.True(result.ValuesMatch);
        }

        [Fact]
        public void ListIsDifferent()
        {
            var a = new List<string> { "hi", "there" };
            var b = new List<string> { "Hi", "there" };

            var result = _calculator.Diff(a, b);

            Assert.False(result.ValuesMatch);

        }

        [Fact]
        public void ListHasAllTheSameElementsButInADifferentOrder()
        {
            var a = new List<string> { "hi", "there" };
            var b = new List<string> { "there", "hi"};

            var result = _calculator.Diff(a, b);

            Assert.False(result.ValuesMatch);

        }
    }
}
