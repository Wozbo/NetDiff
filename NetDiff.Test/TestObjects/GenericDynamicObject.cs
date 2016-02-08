using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class GenericDynamicObject : DynamicObject
    {
        public string PublicString;
        public double PublicNum = 0.00;
        private string _privateString = "";

        public GenericDynamicObject(double num=0.0, string pubString="")
        {
            PublicString = pubString;
            PublicNum = num;
        }
    }
}
