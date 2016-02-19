using csharp_extensions.Extensions;
using System;

namespace NetDiff.Model
{
    public class PrimitiveDiff : BaseDiff
    {
        public double Tolerance;

        public override bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            var matchingType = ((object)baseObj).IsSameTypeAs((object)evaluatedObj);

            if (matchingType && baseObj.GetType() ==  typeof(double))
                return Math.Abs(baseObj - evaluatedObj) < Tolerance;

            return matchingType && baseObj == evaluatedObj;
        }
    }
}
