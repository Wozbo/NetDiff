using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Model;
using Xunit;

namespace NetDiff.Test.Integration
{
    public class IgnoredMatchesTest
    {
        [Fact]
        public void AllMatchesAreIgnored()
        {
            var calculator = new DiffCalculator(ignoreMatches: true);
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 5.1, 2.1, 4.1 };

            var result = calculator.Diff(a, b);
            var resultMatch = result.ValuesMatch;
            var items = ((ObjectDiff)result).Items;

            Assert.False(resultMatch);
            Assert.Equal(2, items.Count());
        }

        [Fact]
        public void ItemsFieldIsEmptySet()
        {
            var calculator = new DiffCalculator(ignoreMatches: true);
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 1.1, 2.1, 3.1 };

            var result = calculator.Diff(a, b);

            var items = ((ObjectDiff)result).Items;

            Assert.True(result.ValuesMatch);
            Assert.False(items.Any());
        }

        [Fact]
        public void MatchesAreNotIgnored()
        {
            var calculator = new DiffCalculator(ignoreMatches: false);
            var a = new List<double> { 1.1, 2.1, 3.1 };
            var b = new List<double> { 1.1, 2.1, 3.1 };

            var result = calculator.Diff(a, b);

            var items = ((ObjectDiff)result).Items;

            Assert.True(result.ValuesMatch);
            // Currently <= because there are some hidden fields included
            Assert.True(3 <= items.Count());
        }

    }
}
