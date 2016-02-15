using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public FieldInfo[] GetFields(object obj)
        {
            return obj.GetType().GetFields(
                  BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Instance);
        }

        /// <summary>
        /// Gets all fields from an object which are objects
        /// </summary>
        /// <param name="obj">The object you want fields from</param>
        /// <returns>Collection of FieldInfos</returns>
        public FieldInfo[] GetOjbectFields(object obj)
        {
            return GetFields(obj)
                .Where(n => IsObjectField(n, obj))
                .ToArray();
        }

        /// <summary>
        /// Gets all non-object fields from an object
        /// </summary>
        /// <param name="obj">The object you want fields from</param>
        /// <returns>Collection of FieldInfos</returns>
        public FieldInfo[] GetNonOjbectFields(object obj)
        {
            var results = GetFields(obj)
                .Where(n => !IsObjectField(n, obj))
                .ToArray();
            return results;
        }

        /// <summary>
        /// Checks to see if a field's type is an object.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsObjectField(FieldInfo field, object obj)
        {
            var fieldValue = field.GetValue(obj);

            return fieldValue != null
                && !fieldValue.GetType().IsPrimitive
                && fieldValue.GetType() != typeof(decimal)
                && fieldValue.GetType() != typeof(string);
        }

        /// <summary>
        /// Finds a correlated FieldInfo in an object. Here, correlated refers to
        /// an identical name, but that criteria is TBD.
        /// </summary>
        /// <param name="field">FieldInfo you're matching against</param>
        /// <param name="obj">The object from which the correlate will come</param>
        /// <returns>A Correlated FieldInfo (if it exists)</returns>
        public FieldInfo GetCorrelate(FieldInfo field, object obj)
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
        public bool HasCorrelate(FieldInfo field, object obj)
        {
            return GetFields(obj)
                .Any(n => string.Equals(n.Name, field.Name)
                       && n.FieldType == field.FieldType);
        }

        /// <summary>
        /// Gets a field value from an object. If a direct value exists, meaning 
        /// that we don't need a correlate, then it is immediately returned. Otherwise,
        /// NetDiff tries to find a correlate and return it. This action defaults to null.
        /// </summary>
        /// <param name="field">FieldInfo you're matching against</param>
        /// <param name="obj">The object you're checking</param>
        /// <returns>Value of the field for evaluated object</returns>
        public dynamic GetFieldValue(FieldInfo field, object obj)
        {
            // Check if the field exists 
            if (obj.GetType().GetFields().Contains(field))
            {
                return field.GetValue(obj);
            }

            // Check to see if it has a correlate
            if (HasCorrelate(field, obj))
            {
                var correlate = GetCorrelate(field, obj);
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
            var fields = GetFields(exclusiveTo);

            // Find the fields where the antagonist doesn't have a correlate; return it
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
        public ICollection<DiffedItem> Intersect(
            object baseObj, 
            object evaluated)
        {
            // Get non-object fields for each object
            var baseFields = GetFields(baseObj);
            var evaluatedFields = GetFields(evaluated);

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
        /// <param name="evaluated">The object you're evaluating against</param>
        /// <returns>Collection of Items which are mutually exclusive</returns>
        public ICollection<DiffedItem> Assemble(
            IEnumerable<FieldInfo> fields, 
            object baseObj, 
            object evaluated)
        {
            var exclusives = fields.Select(field => new DiffedField()
            {
                Field = field,
                BaseValue = GetFieldValue(field, baseObj),
                EvaluatedValue = GetFieldValue(field, evaluated),
                Tolerance = _tolerance
            });

            return exclusives
                .Cast<DiffedItem>()
                .ToList();
        } 
    }
}
