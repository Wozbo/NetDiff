using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff
{
    public class DiffedObject : DiffedItem
    {
        public IEnumerable<DiffedItem> Items;

        public override bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            return Items.All(d => d.ValuesMatch);
        }
    }
}
