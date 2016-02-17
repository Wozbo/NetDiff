using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using csharp_extensions.Extensions;

namespace NetDiff
{
    public class DiffedObject : DiffedItem
    {
        public IEnumerable<DiffedItem> Items;

        public DiffedObject(
            object baseObj = null,
            object eval = null,
            IEnumerable<DiffedItem> items = null,
            string message = "") : base(baseObj, eval, message)
        {
            Items = items ?? new List<DiffedItem>();
        }

        public override bool Equals(dynamic baseObj, dynamic evaluatedObj)
        {
            return Items.All(d => d.ValuesMatch);
        }

        /// <summary>
        /// This will remove any Diffed items that have matching a/b values.
        /// So only what is incorrect will remain.
        /// This will make hunting down problems much quicker.
        /// </summary>
        public List<DiffedItem> WithoutMatching()
        {
            var result = new List<DiffedItem>();

            var nonMatching = Items.Where(item => !item.ValuesMatch);
            foreach (var item in nonMatching)
            {
                if (item.IsA(typeof (DiffedObject)))
                {
                    var obj = item as DiffedObject;
                    obj.Items = obj.WithoutMatching();

                    result.Add(obj);
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
