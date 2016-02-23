using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using csharp_extensions.Extensions;
using NetDiff.Extensions;
using NetDiff.Model;

namespace NetDiff
{
    public class DiffCalculator
    {
        private readonly double _tolerance;
        private readonly Type[] _ignoredClasses;
        private readonly bool _ignoreMatches;

        public DiffCalculator(double tolerance = 1e-6, Type[] ignoredClasses = null, bool ignoreMatches = false)
        {
            _tolerance = tolerance;
            _ignoredClasses = ignoredClasses ?? new Type[] {};
            _ignoreMatches = ignoreMatches;
        }

        #region Entry point
        public BaseDiff Diff(object baseObj, object evaluated)
        {
            var diffedData = DiffObjects(baseObj, evaluated);

            if (_ignoreMatches && diffedData.IsA(typeof(ObjectDiff)))
            {
                var objectDiff = diffedData as ObjectDiff;
                objectDiff.Items = objectDiff.WithoutMatching();

                return objectDiff;
            }


            return diffedData;
        }
        #endregion

        #region Get___ Helpers



        /// <summary>
        /// Finds a correlated FieldInfo in an object. Here, correlated refers to
        /// an identical name, but that criteria is TBD.
        /// </summary>
        /// <param name="matching">FieldInfo you're matching against</param>
        /// <param name="fromObject">The object from which the correlate will come</param>
        /// <returns>A Correlated FieldInfo (if it exists)</returns>
        internal FieldInfo GetFieldInfo(FieldInfo matching, object fromObject)
        {
            return fromObject
                .GetType()
                .GetField(matching.Name);
        }


        /// <summary>
        /// Gets a field value from an object. If a direct value exists, meaning
        /// that we don't need a correlate, then it is immediately returned. Otherwise,
        /// NetDiff tries to find a correlate and return it. This action defaults to null.
        /// </summary>
        /// <param name="forField">FieldInfo you're matching against</param>
        /// <param name="obj">The object you're checking</param>
        /// <returns>Value of the field for evaluated object</returns>
        internal dynamic GetValue(FieldInfo forField, object obj)
        {
            // Check if the field exists
            if (obj.GetInstanceFields().Contains(forField))
            {
                return forField.GetValue(obj);
            }

            // Check to see if it has a correlate
            if (obj.HasFieldMatching(forField))
            {
                var correlate = GetFieldInfo(forField, obj);
                return correlate.GetValue(obj);
            }

            // Null; this field doesn't exist, nor does it have a correlate.
            return null;
        }

        /// <summary>
        /// Find all fields which are exclusive to a specific object compared to
        /// another object.
        /// </summary>
        /// <param name="exclusiveTo">The object whose fields you're looking through</param>
        /// <param name="antagonist">The object whose fields we are de-intersecting</param>
        /// <returns>A list of exclusive fieldinfos</returns>
        internal List<FieldInfo> GetExclusiveFields(
            object exclusiveTo,
            object antagonist)
        {
            var fields = exclusiveTo.GetInstanceFields();

            // Find the fields where the antagonist doesn't have a correlate; return it
            return fields
                .Where(n => !antagonist.HasFieldMatching(n))
                .ToList();
        }

        internal List<FieldInfo> GetMutualObjectFields(
            object baseObj,
            object evaluated)
        {
            var baseObjectFields = baseObj.GetNonObjectInstanceFields();
            return baseObjectFields
                .Where(evaluated.HasFieldMatching) as List<FieldInfo>
                   ?? new List<FieldInfo>();
        }

        private static bool ContentIsEqual(IEnumerable<object> sortedBase, IEnumerable<object> sortedAntagonist)
        {
            var contentIsEqual = sortedBase
                .Zip(sortedAntagonist, (sbase, sant) => sbase.Equals(sant)).All(n => n);
            return contentIsEqual;
        }

        #endregion


        #region helpers

        /// <summary>
        /// Diff two objects
        /// </summary>
        /// <param name="baseObj"></param>
        /// <param name="evaluated"></param>
        /// <returns></returns>
        internal BaseDiff DiffObjects(
            object baseObj,
            object evaluated)
        {
            if (TypeIsIgnored(baseObj) || TypeIsIgnored(evaluated))
            {
                return new IgnoredDiff(baseObj: baseObj, eval: evaluated);
            }


            // Validate that the objects are not null
            if (baseObj == null || evaluated == null)
            {
                return new NullDiff
                {
                    BaseValue = baseObj,
                    EvaluatedValue = evaluated
                };
            }

            // Check to see if it's primitive
            if (baseObj.IsPrimitive())
            {
                return new PrimitiveDiff
                {
                    BaseValue = baseObj,
                    EvaluatedValue = evaluated,
                    Tolerance = _tolerance
                };
            }

            // Handle Intersected Fields
            var results = Intersect(baseObj, evaluated).ToList();

            // Handle Exclusive Fields for BaseObj
            results.AddRange(MutuallyExclusive(baseObj, evaluated));

            if (baseObj.IsIterable())
            {
                if (!evaluated.IsIterable())
                {
                    return new BaseDiff
                    {
                        BaseValue = baseObj,
                        EvaluatedValue = evaluated,
                        Message = DiffMessage.DiffersInType
                    };
                }

                var baseList = ((IEnumerable)baseObj).Cast<object>().ToList();
                var evalList = ((IEnumerable)evaluated).Cast<object>().ToList();
                var listResults = DiffList(baseList, evalList);

                results.AddRange(listResults);
            }
            else
            {
                // Recursively handle each intersected object field
                var fields = DiffFields(baseObj, evaluated);

                results.AddRange(fields);
            }

            return new ObjectDiff
            {
                BaseValue = baseObj,
                EvaluatedValue = evaluated,
                Items = results
            };
        }

        /// <summary>
        /// Currently there is no way to ignore null values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool TypeIsIgnored(object obj)
        {
            if (obj == null) return false;

            var type = obj.GetType();
            var result = _ignoredClasses.Contains(type);
            return result;
        }


        /// <summary>
        /// Diff two lists. Does not currently
        /// </summary>
        /// <param name="baseObj"></param>
        /// <param name="antagonist"></param>
        /// <returns></returns>
        internal IEnumerable<BaseDiff> DiffList(IEnumerable<object> baseObj, IEnumerable<object> antagonist)
        {
            var results = new List<BaseDiff>();

            // 1. Ensure the lists are the same size
            var baseSize = baseObj.Count();
            var antagonistSize = antagonist.Count();

            // If these aren't the same size, don't run the Diff, just mention that it differs in length.
            if (baseSize != antagonistSize)
            {
                var result = new BaseDiff
                {
                    BaseValue = baseObj,
                    EvaluatedValue = antagonist,
                    Message = DiffMessage.DiffersInLength
                };

                results.Add(result);
                return results;
            }

            // Determine the Error Message for mismatches in the list
            var errorMessage = DiffMessage.DiffersInContent;
            if(baseObj.All(n => n is IComparable) && antagonist.All(m => m is IComparable))
            {
                // Sort the Base
                var sortedBase = baseObj.ToList();
                sortedBase.Sort();

                // Sort the Eval
                var sortedAntagonist = antagonist.ToList();
                sortedAntagonist.Sort();

                // If the content is equal, then the order is the only thing that can differ
                if (ContentIsEqual(sortedBase, sortedAntagonist))
                {
                    errorMessage = DiffMessage.DiffersInOrder;
                }
            }

            // Check values
            foreach (var item in baseObj.Zip(antagonist))
            {
                var result = DiffObjects(
                    baseObj: item.Key,
                    evaluated: item.Value);

                // Change the error message if the values don't match
                result.Message = result.ValuesMatch
                    ? DiffMessage.NotApplicable
                    : errorMessage;

                results.Add(result);
            }

            return results;
        }

        private IEnumerable<BaseDiff> DiffFields(object baseObj, object antagonist)
        {
            var fields = baseObj.GetInstanceFields();
            var result = fields.Select(field => Diff(field, baseObj, antagonist));

            return result;
        }

        /// <summary>
        /// Diff a field
        /// </summary>
        /// <param name="forField"></param>
        /// <param name="baseObj"></param>
        /// <param name="antagonist"></param>
        /// <returns></returns>
        private BaseDiff Diff(FieldInfo forField, object baseObj, object antagonist)
        {
            var valueOfBase = GetValue(forField, baseObj);
            var otherFieldValue = GetFieldInfo(matching: forField, fromObject: antagonist);
            var valueOfEvaluated = GetValue(otherFieldValue, antagonist);

            return DiffObjects(
                baseObj: valueOfBase,
                evaluated: valueOfEvaluated);
        }


        /// <summary>
        /// Finds all fields which are included in both of two dynamic objects
        /// </summary>
        /// <param name="baseObj">Baseline Object</param>
        /// <param name="evaluated">The object you might be evaluating</param>
        /// <returns>Collection of Items which are in both objects</returns>
        internal ICollection<BaseDiff> Intersect(
            object baseObj,
            object evaluated)
        {
            // Get non-object fields for each object
            var baseFields = baseObj.GetNonObjectInstanceFields();
            var evaluatedFields = evaluated.GetNonObjectInstanceFields();

            // Perform Intersection using the FieldInfoIntersector
            var intersectedFields = baseFields.Intersect(evaluatedFields, new FieldInfoIntersector());

            // Create a summary, then return it
            var intersectionSummary = Assemble(intersectedFields, baseObj, evaluated);
            return intersectionSummary;
        }

        /// <summary>
        /// Finds all fields which are mutually exclusive between two dynamic objects
        /// </summary>
        /// <param name="baseObj">Baseline Object</param>
        /// <param name="evaluated">The object you might be evaluating</param>
        /// <returns>Collection of Items which are mutually exclusive</returns>
        internal ICollection<BaseDiff> MutuallyExclusive(
            object baseObj,
            object evaluated)
        {
            // Get fields which exist in one object but not the other
            var fullExclusives = GetExclusiveFields(baseObj, antagonist: evaluated);
            var evaluatedExclusives = GetExclusiveFields(evaluated, antagonist: baseObj);

            // Merge the two exclusives together
            fullExclusives.AddRange(evaluatedExclusives);

            var exclusionSummary = Assemble(fullExclusives, baseObj, evaluated);
            return exclusionSummary;
        }

        /// <summary>
        /// Assemble a summary of DiffedItems for this object
        /// </summary>
        /// <param name="fields">A collection of FieldInfos</param>
        /// <param name="baseObj">The baseline object</param>
        /// <param name="antagonist">The object you're evaluating against</param>
        /// <returns>Collection of Items which are mutually exclusive</returns>
        internal ICollection<BaseDiff> Assemble(
            IEnumerable<FieldInfo> fields,
            object baseObj,
            object antagonist)
        {
            var exclusives = fields.Select(field => new FieldDiff()
            {
                Field = field,
                BaseValue = GetValue(field, baseObj),
                EvaluatedValue = GetValue(field, antagonist),
                Tolerance = _tolerance
            });

            return exclusives
                .Cast<BaseDiff>()
                .ToList();
        }

        #endregion
    }
}
