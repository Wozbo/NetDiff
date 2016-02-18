using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetDiff.Model;

namespace NetDiff
{
    public class DiffedItem
    {
        public DiffValue Message;

        public DiffedItem(
            object baseObj=null, 
            object eval=null, 
            DiffValue message = DiffValue.NotEvaluated)
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
