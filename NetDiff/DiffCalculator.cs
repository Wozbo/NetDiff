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

        #region Get___ Helpers
        public DiffCalculator(double tolerance=1e-6)
        {
            _tolerance = tolerance;
        }


        /// <summary>
        /// Finds a correlated FieldInfo in an object. Here, correlated refers to
        /// an identical name, but that criteria is TBD.
        /// </summary>
        /// <param name="matching">FieldInfo you're matching against</param>
        /// <param name="fromObject">The object from which the correlate will come</param>
        /// <returns>A Correlated FieldInfo (if it exists)</returns>
        public FieldInfo GetFieldInfo(FieldInfo matching, object fromObject)
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
        public dynamic GetValue(FieldInfo forField, object obj)
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
        public List<FieldInfo> GetExclusiveFields(
            object exclusiveTo,
            object antagonist)
        {
            var fields = exclusiveTo.GetInstanceFields();

            // Find the fields where the antagonist doesn't have a correlate; return it
            return fields
                .Where(n => !antagonist.HasFieldMatching(n))
                .ToList();
        }

        public List<FieldInfo> GetMutualObjectFields(
            object baseObj,
            object evaluated)
        {
            var baseObjectFields = baseObj.GetNonObjectInstanceFields();
            return baseObjectFields
                .Where(evaluated.HasFieldMatching) as List<FieldInfo>
                   ?? new List<FieldInfo>();
        }

        #endregion

        #region Aliases
        public DiffedItem Diff(object baseObj, object evaluated)
        {
            return DiffObjects(baseObj, evaluated);
        }
        #endregion

        /// <summary>
        /// Diff two objects
        /// </summary>
        /// <param name="baseObj"></param>
        /// <param name="evaluated"></param>
        /// <returns></returns>
        public DiffedItem DiffObjects(
            object baseObj,
            object evaluated)
        {
            if (baseObj == null || evaluated == null)
            {
                return new DiffedNull {BaseValue = baseObj, EvaluatedValue = evaluated};
            }

            if (baseObj.IsPrimitive())
            {
                return new DiffedPrimitive {BaseValue = baseObj, EvaluatedValue = evaluated, Tolerance = _tolerance};
            }

            // Handle Intersected Fields
            var results = Intersect(baseObj, evaluated).ToList();

            // Handle Exclusive Fields for BaseObj
            results.AddRange(MutuallyExclusive(baseObj, evaluated));

            if (baseObj.IsIterable())
            {
                if (!evaluated.IsIterable())
                {
                    return new DiffedItem
                           {
                               BaseValue = baseObj,
                               EvaluatedValue = evaluated,
                               Message = "Types are not both iterable"
                           };
                }
                var oe = ((IEnumerable)baseObj).Cast<object>().ToList();
                var ae = ((IEnumerable)evaluated).Cast<object>().ToList();
                var listResults = DiffList(oe, ae);

                results.AddRange(listResults);
            }
            else
            {
                // Recursively handle each intersected object field
                var fields = DiffFields(baseObj, evaluated);

                results.AddRange(fields);
            }

            // Handle each non-intersected object field...

            return new DiffedObject
            {
                BaseValue = baseObj,
                EvaluatedValue = evaluated,
                Items = results
            };
        }

        public IEnumerable<DiffedItem> DiffList(IEnumerable<object> baseObj, IEnumerable<object> antagonist)
        {
            var results = new List<DiffedItem>();

            // 1. Ensure the lists are the same size
            var baseSize = baseObj.Count();
            var antagonistSize = antagonist.Count();

            if (baseSize != antagonistSize)
            {
                var result = new DiffedItem
                       {
                           BaseValue = baseObj,
                           EvaluatedValue = antagonist,
                           Message = "Enumerables are not the same length"
                       };

                results.Add(result);
                return results;
            }

            foreach (var item in baseObj.Zip(antagonist))
            {
                var currentBase = item.Key;
                var currentAntagonist = item.Value;

                var result = DiffObjects(currentBase, currentAntagonist);
                results.Add(result);
            }

            // TODO: how to handle sorting issues -- write now sorting matters and will error if not the same

            return results;
        }

        private IEnumerable<DiffedItem> DiffFields(object baseObj, object antagonist)
        {
            var fields = baseObj.GetInstanceFields();
            var result = fields.Select(field => Diff(field, baseObj, antagonist));

            return result;
        }

        private DiffedItem Diff(FieldInfo forField, object baseObj, object antagonist)
        {
            var valueOfBase = GetValue(forField, baseObj);
            var otherFieldValue = GetFieldInfo(matching: forField, fromObject: antagonist);
            var valueOfEvaluated = GetValue(otherFieldValue, antagonist);

            return DiffObjects(baseObj: valueOfBase, evaluated: valueOfEvaluated);
        }


        /// <summary>
        /// Finds all fields which are included in both of two dynamic objects
        /// </summary>
        /// <param name="baseObj">Baseline Object</param>
        /// <param name="evaluated">The object you might be evaluating</param>
        /// <returns>Collection of Items which are in both objects</returns>
        public ICollection<DiffedItem> Intersect(
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
        public ICollection<DiffedItem> MutuallyExclusive(
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
        public ICollection<DiffedItem> Assemble(
            IEnumerable<FieldInfo> fields,
            object baseObj,
            object antagonist)
        {
            var exclusives = fields.Select(field => new DiffedField()
            {
                Field = field,
                BaseValue = GetValue(field, baseObj),
                EvaluatedValue = GetValue(field, antagonist),
                Tolerance = _tolerance
            });

            return exclusives
                .Cast<DiffedItem>()
                .ToList();
        }
    }
}
