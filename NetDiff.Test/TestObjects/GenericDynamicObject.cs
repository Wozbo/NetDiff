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
        public string SecondaryString;
        public double PublicNum = 0.00;

        public GenericDynamicObject(double num=0.0, string pubString="", string secondString="")
        {
            PublicString = pubString;
            SecondaryString = secondString;
            PublicNum = num;
        }
    }
}
