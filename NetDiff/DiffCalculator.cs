using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets all fields from an object
        /// </summary>
        /// <param name="obj">The object you want fields from</param>
        /// <returns>Collection of FieldInfos</returns>
        public FieldInfo[] GetObjectFields(DynamicObject obj)
        {
            return obj.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
        }

        /// <summary>
        /// Finds a correlated FieldInfo in an object. Here, correlated refers to
        /// an identical name, but that criteria is TBD.
        /// </summary>
        /// <param name="field">FieldInfo you're matching against</param>
        /// <param name="obj">The object from which the correlate will come</param>
        /// <returns>A Correlated FieldInfo (if it exists)</returns>
        public FieldInfo GetCorrelate(FieldInfo field, DynamicObject obj)
        {
            return obj
                .GetType()
                .GetField(field.Name);
        }

        /// <summary>
        /// Determines if a correlated FieldInfo exists for an object
        /// </summary>
        /// <param name="field">FieldInfo you're matching against</param>
        /// <param name="obj">The object you're checking</param>
        /// <returns>boolean indicating whether the correlate exists</returns>
        public bool HasCorrelate(FieldInfo field, DynamicObject obj)
        {
            return obj
                .GetType()
                .GetFields()
                .Any(n => string.Equals(n.Name, field.Name));
        }

        /// <summary>
        /// Gets a field value from an object. If a direct value exists, meaning 
        /// that we don't need a correlate, then it is immediately returned. Otherwise,
        /// NetDiff tries to find a correlate and return it. This action defaults to null.
        /// </summary>
        /// <param name="field">FieldInfo you're matching against</param>
        /// <param name="obj">The object you're checking</param>
        /// <returns>Value of the field for evaluated object</returns>
        public dynamic GetFieldValue(FieldInfo field, DynamicObject obj)
        {
            if (obj.GetType().GetFields().Contains(field))
            {
                return field.GetValue(obj);
            }

            if (HasCorrelate(field, obj))
            {
                var correlate = GetCorrelate(field, obj);
                return correlate.GetValue(obj);
            }

            return null;
        }

        /// <summary>
        /// Find all fields which are exclusive to a specific object compared to
        /// another object.
        /// </summary>
        /// <param name="exclusiveTo">The object whose fields you're looking through</param>
        /// <param name="antagonist">The object whose fields we are de-intersecting</param>
        /// <returns>A list of exclusive fieldinfos</returns>
        public List<FieldInfo> GetExclusiveFields(DynamicObject exclusiveTo, DynamicObject antagonist)
        {
            var fields = GetObjectFields(exclusiveTo);
            var exclusive = fields.Where(n => !HasCorrelate(n, antagonist));

            return exclusive.ToList();
        }

        #endregion

        /// <summary>
        /// Finds all fields which are included in both of two dynamic objects
        /// </summary>
        /// <param name="baseObj">Baseline Object</param>
        /// <param name="evaluated">The object you might be evaluating</param>
        /// <returns>Collection of Items which are in both objects</returns>
        public ICollection<DiffedItem> Intersect(DynamicObject baseObj, DynamicObject evaluated)
        {
            var baseFields = GetObjectFields(baseObj);
            var evaluatedFields = GetObjectFields(evaluated);
            var intersectedFields = baseFields.Intersect(evaluatedFields, new FieldInfoIntersector());

            var intersected = intersectedFields.Select(field => new DiffedItem()
            {
                Field = field,
                BaseObjValue = GetFieldValue(field, baseObj),
                EvaluatedValue = GetFieldValue(field, evaluated),
                Tolerance = _tolerance
            });

            return intersected.ToList();
        }

        /// <summary>
        /// Finds all fields which are mutually exclusive between two dynamic objects
        /// </summary>
        /// <param name="baseObj">Baseline Object</param>
        /// <param name="evaluated">The object you might be evaluating</param>
        /// <returns>Collection of Items which are mutually exclusive</returns>
        public ICollection<DiffedItem> MutuallyExclusive(DynamicObject baseObj, DynamicObject evaluated)
        {
            var evaluatedExclusives = GetExclusiveFields(evaluated, baseObj);
            var fullExclusives = GetExclusiveFields(baseObj, evaluated);
            fullExclusives.AddRange(evaluatedExclusives);

            var exclusives = fullExclusives.Select(field => new DiffedItem()
            {
                Field = field,
                BaseObjValue = GetFieldValue(field, baseObj),
                EvaluatedValue = GetFieldValue(field, evaluated),
                Tolerance = _tolerance
            });

            return exclusives.ToList();
        }
    }
}
