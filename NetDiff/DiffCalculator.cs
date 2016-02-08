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


        public FieldInfo[] GetObjectFields(DynamicObject obj)
        {
            return obj.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
        }

        public FieldInfo GetCorrelate(FieldInfo info, DynamicObject evaluated)
        {
            return evaluated
                .GetType()
                .GetField(info.Name);
        }

        public bool HasCorrelate(FieldInfo info, DynamicObject evaluated)
        {
            return evaluated
                .GetType()
                .GetFields()
                .Any(n => string.Equals(n.Name, info.Name));
        }

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
