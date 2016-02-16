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
        public DiffedItem(dynamic baseObj=null, dynamic eval=null)
        {
            BaseValue = baseObj;
            EvaluatedValue = eval;
        }

        public dynamic BaseValue, EvaluatedValue;

        public bool ValuesMatch => Equals(BaseValue, EvaluatedValue);

        public virtual bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            return false;
        }
    }
}
