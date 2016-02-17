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
        public string Message;

        public DiffedItem(object baseObj=null, object eval=null, string message = "")
        {
            BaseValue = baseObj;
            EvaluatedValue = eval;
            Message = message;

        }

        public object BaseValue, EvaluatedValue;

        public bool ValuesMatch => Equals(BaseValue, EvaluatedValue);

        public virtual bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            return baseObj.Equals(evaluatedObj);
        }
    }
}
