using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDiff.Model;

namespace NetDiff.Test.Unit.Model.DiffedObject
{
    [TestClass]
    public class WithoutMatchingTest
    {
        [TestMethod]
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

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
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

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(false, result.First().ValuesMatch);
        }

        [TestMethod]
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

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(false, items.First().ValuesMatch);
        }
    }
}