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
        public FieldInfo Field;
        public dynamic baseObjValue, evaluatedValue;
        //Todo tolerance

        public bool ValuesMatch => baseObjValue == evaluatedValue;
    }
}
