using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Model
{
    public class DiffedNull : DiffedItem
    {
        public override bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            return baseObj == evaluatedObj;
        }
    }
}
