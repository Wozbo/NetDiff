using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Extensions;
using NetDiff.Model;
using NetDiff.Test.TestObjects;
using Xunit;

namespace NetDiff.Test.Unit.DiffCalculator
{    public class DiffObjectsTest
    {

        [Fact]
        public void ObjectsMayBeOfDifferentType()
        {
            var baseObject = 2;
            var antagonist = "hi";

            var result = baseObject.DiffAgainst(antagonist);

            Assert.Equal(false, result.ValuesMatch);
        }

        [Fact]
        public void AntagonistIsNotIterable()
        {
            var baseObject = new List<int> {1, 2, 3};
            var antagonist = 2;

            var result = baseObject.DiffAgainst(antagonist);

            Assert.Equal(DiffMessage.DiffersInType, result.Message);
        }
    }
}
