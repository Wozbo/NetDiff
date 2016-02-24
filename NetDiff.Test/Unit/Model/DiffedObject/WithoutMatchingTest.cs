using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;
using NetDiff.Model;
using Xunit;

namespace NetDiff.Test.Unit.Model.DiffedObject
{
    public class WithoutMatchingTest
    {
        [Fact]
        public void EmptyIfMatching()
        {
            var obj = new ObjectDiff()
                      {
                          BaseValue = 1,
                          EvaluatedValue = 1,
                          Items =
                              new List<BaseDiff>()
                              {
                                  new BaseDiff() {BaseValue = 2, EvaluatedValue = 2},
                                  new BaseDiff() {BaseValue = 3, EvaluatedValue = 3}
                              }
                      };

            var result = obj.WithoutMatching();

            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void OnlyMismatchedRemain()
        {
            var obj = new ObjectDiff()
                      {
                          BaseValue = 1,
                          EvaluatedValue = 1,
                          Items =
                              new List<BaseDiff>()
                              {
                                  new BaseDiff() {BaseValue = 2, EvaluatedValue = 2},
                                  new BaseDiff() {BaseValue = 3, EvaluatedValue = 1}
                              }
                      };

            var result = obj.WithoutMatching();

            Assert.Equal(1, result.Count);
            Assert.Equal(false, result.First().ValuesMatch);
        }

        [Fact]
        public void OnlyNestedMismatchedRemain()
        {
            var obj = new ObjectDiff()
            {
                BaseValue = 1,
                EvaluatedValue = 1,
                Items = new List<BaseDiff>()
                    {
                        new BaseDiff() {BaseValue = 2, EvaluatedValue = 2},
                        new ObjectDiff()
                        {
                            BaseValue = 3,
                            EvaluatedValue = 3,
                            Items =
                                new List<BaseDiff>()
                                {
                                    new BaseDiff()
                                    {
                                        BaseValue=2,
                                        EvaluatedValue=2
                                    },
                                    new BaseDiff()
                                    {
                                        BaseValue=3,
                                        EvaluatedValue=1
                                    }
                                }
                        }
                    }
            };

            var result = obj.WithoutMatching();

            var items = (List<BaseDiff>)result.First().Send("Items");

            Assert.Equal(1, items.Count);
            Assert.Equal(false, items.First().ValuesMatch);
        }
    }
}