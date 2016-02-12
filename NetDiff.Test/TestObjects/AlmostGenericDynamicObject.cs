using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class AlmostGenericDynamicObject
    {
        public string PublicString;
        public double SecondaryString;
        public double PublicNum;
        public SubObject SubObj;

        public AlmostGenericDynamicObject(
            double num=0.0, 
            string pubString="", 
            double secondString=0.0,
            SubObject subobj=null)
        {
            PublicString = pubString;
            SecondaryString = secondString;
            PublicNum = num;
            SubObj = subobj;
        }
    }
}
