using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Test.TestObjects
{
    public class SubObject: DynamicObject
    {
        public string Name;

        public SubObject(string name)
        {
            Name = name;
        }
    }
}
