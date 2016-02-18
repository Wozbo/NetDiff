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
        public DiffMessage Message;

        public DiffedItem(
            object baseObj = null, 
            object eval = null, 
            DiffMessage message = DiffMessage.NotApplicable)
        {
            BaseValue = baseObj;
            EvaluatedValue = eval;
            Message = message;
        }

        public object BaseValue, EvaluatedValue;

        public bool ValuesMatch => Equals(BaseValue, EvaluatedValue);

        public virtual bool Equals(object baseObj, object evaluatedObj)
        {
            return baseObj.Equals(evaluatedObj);
        }
    }
}
