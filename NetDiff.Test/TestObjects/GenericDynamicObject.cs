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
        protected string ProtectedString = "";
        private string _privateString = "";
    }
}
