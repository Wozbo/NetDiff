using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff
{
    class FieldInfoIntersector : IEqualityComparer<FieldInfo>
    {
        /// <summary>
        /// Equals means that the name is identical and types are equal
        /// </summary>
        /// <param name="baseField">FieldInfo in base object</param>
        /// <param name="evalField">FieldInfo in evaluated obj</param>
        /// <returns>Boolean indicating equality</returns>
        public bool Equals(FieldInfo baseField, FieldInfo evalField)
        {
            return baseField.FieldType == evalField.FieldType
                && string.Equals(baseField.Name, evalField.Name);
        }

        /// <summary>
        /// Returns a hashcode for a specific FieldInfo. Functionally
        /// identical to normal behavior, just a mandatory declaration.
        /// </summary>
        /// <param name="obj">FieldInfo Object in question</param>
        /// <returns>Hash Code for that object</returns>
        public int GetHashCode(FieldInfo obj)
        {
            return obj.FieldType.GetHashCode();
        }
    }
}
