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
        public dynamic BaseValue, EvaluatedValue;

        public bool ValuesMatch => Equals(BaseValue, EvaluatedValue);

        public virtual bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            return false;
        }
    }
}
