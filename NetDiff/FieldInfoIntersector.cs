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
        public bool Equals(FieldInfo baseField, FieldInfo evalField)
        {
            return baseField.FieldType == evalField.FieldType
                && baseField.Name == evalField.Name;
        }

        public int GetHashCode(FieldInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}
