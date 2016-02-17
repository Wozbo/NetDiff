using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDiff.Test.Unit.Model.DiffedObject
{
    [TestClass]
    public class WithoutMatchingTest
    {
        [TestMethod]
        public void EmptyIfMatching()
        {
            var obj = new NetDiff.DiffedObject()
                      {
                          BaseValue = 1,
                          EvaluatedValue = 1,
                          Items =
                              new List<DiffedItem>()
                              {
                                  new DiffedItem() {BaseValue = 2, EvaluatedValue = 2},
                                  new DiffedItem() {BaseValue = 3, EvaluatedValue = 3}
                              }
                      };

            var result = obj.WithoutMatching();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void OnlyMismatchedRemain()
        {
            var obj = new NetDiff.DiffedObject()
                      {
                          BaseValue = 1,
                          EvaluatedValue = 1,
                          Items =
                              new List<DiffedItem>()
                              {
                                  new DiffedItem() {BaseValue = 2, EvaluatedValue = 2},
                                  new DiffedItem() {BaseValue = 3, EvaluatedValue = 1}
                              }
                      };

            var result = obj.WithoutMatching();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(false, result.First().ValuesMatch);
        }

        [TestMethod]
        public void OnlyNestedMismatchedRemain()
        {
            var obj = new NetDiff.DiffedObject()
                      {
                          BaseValue = 1,
                          EvaluatedValue = 1,
                          Items =
                              new List<DiffedItem>()
                              {
                                  new DiffedItem() {BaseValue = 2, EvaluatedValue = 2},
                                  new NetDiff.DiffedObject()
                                  {
                                      BaseValue = 3,
                                      EvaluatedValue = 3,
                                      Items =
                                          new List<DiffedItem>()
                                          {
                                              new DiffedItem
                                                  ()
                                              {
                                                  BaseValue
                                                      =
                                                      2,
                                                  EvaluatedValue
                                                      =
                                                      2
                                              },
                                              new DiffedItem
                                                  ()
                                              {
                                                  BaseValue
                                                      =
                                                      3,
                                                  EvaluatedValue
                                                      =
                                                      1
                                              }
                                          }
                                  }
                              }
                      };

            var result = obj.WithoutMatching();

            var items = (List<DiffedItem>)result.First().Send("Items");

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(false, items.First().ValuesMatch);
        }
    }
}