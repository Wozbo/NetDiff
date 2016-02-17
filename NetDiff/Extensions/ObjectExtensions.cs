using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;

namespace NetDiff.Extensions
{
    public static class ObjectExtensions
    {
        public static DiffedItem DiffAgainst(this object baseObject, object antagonist)
        {
            var calculator = new DiffCalculator();
            return calculator.Diff(baseObject, antagonist);
        }


        /// <summary>
        /// Gets all fields from an object which are objects
        /// </summary>
        /// <param name="obj">The object you want fields from</param>
        /// <returns>Collection of FieldInfos</returns>
        public static FieldInfo[] GetObjectInstanceFields(this object obj)
        {
            return obj.GetInstanceFields()
                .Where(n => IsObjectField(n, obj))
                .ToArray();
        }


        /// <summary>
        /// Gets all non-object fields from an object
        /// </summary>
        /// <param name="obj">The object you want fields from</param>
        /// <returns>Collection of FieldInfos</returns>
        public static FieldInfo[] GetNonObjectInstanceFields(this object obj)
        {
            var results = obj.GetInstanceFields()
                .Where(n => !IsObjectField(n, obj))
                .ToArray();
            return results;
        }

        /// <summary>
        /// Public access for testing...
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsObjectField(this object obj, FieldInfo field)
        {
            return IsObjectField(field, obj);
        }

        /// <summary>
        /// Checks to see if a field's type is an object.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static bool IsObjectField(FieldInfo field, object obj)
        {
            var fieldValue = field.GetValue(obj);

            return fieldValue != null
                && !fieldValue.IsPrimitive();
        }


    }
}
