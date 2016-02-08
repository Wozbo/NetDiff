using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff
{
    public class DiffedItem
    {
        public FieldInfo Field;
        public dynamic BaseObjValue, EvaluatedValue;
        public double Tolerance;

        public bool ValuesMatch => Equals(BaseObjValue, EvaluatedValue);

        public bool Equals(dynamic baseObj, dynamic evaluatedObj)
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
