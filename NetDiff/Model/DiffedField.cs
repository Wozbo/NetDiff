using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff
{
    public class DiffedField : DiffedItem
    {
        public FieldInfo Field;
        public double Tolerance;

        public override bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            // Use tolerances for floating point equalities
            if (Field.FieldType == typeof (double))
                return Math.Abs(baseObj - evaluatedObj) < Tolerance;

            // Use string.Equals if applicable
            if (Field.FieldType == typeof (string))
                return string.Equals(baseObj, evaluatedObj);

            return baseObj == evaluatedObj;
        }
    }
}
