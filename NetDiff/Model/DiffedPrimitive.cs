using csharp_extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Model
{
    public class DiffedPrimitive : DiffedItem
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
