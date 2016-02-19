using System;
using System.Reflection;

namespace NetDiff.Model
{
    public class FieldDiff : BaseDiff
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
