using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Model
{
    public class DiffedIgnored : DiffedItem
    {
        public DiffedIgnored(object baseObj = null,
            object eval = null)
        {
            BaseValue = baseObj;
            EvaluatedValue = eval;
            Message = DiffMessage.Ignored;
        }

        public override bool ValuesMatch => true;
        public override bool Equals(object baseObj, object evaluatedObj)
        {
            return true;
        }

    }
}
