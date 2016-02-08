using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class SlightlyDifferentObject : DynamicObject
    {
        public string PublicString;
        public string SecondString;
        public double PublicNum = 0.00;

        public SlightlyDifferentObject(double num=0.0, string pubString="", string secondString="")
        {
            PublicString = pubString;
            SecondString = secondString;
            PublicNum = num;
        }
    }
}
