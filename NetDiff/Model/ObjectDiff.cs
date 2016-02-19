using System.Collections.Generic;
using System.Linq;
using csharp_extensions.Extensions;

namespace NetDiff.Model
{
    public class ObjectDiff : BaseDiff
    {
        public IEnumerable<BaseDiff> Items;

        public ObjectDiff(
            object baseObj = null,
            object eval = null,
            IEnumerable<BaseDiff> items = null,
            DiffMessage message = DiffMessage.NotApplicable)
            : base(baseObj, eval, message)
        {
            Items = items ?? new List<BaseDiff>();
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
        public List<BaseDiff> WithoutMatching()
        {
            var result = new List<BaseDiff>();

            var nonMatching = Items.Where(item => !item.ValuesMatch);
            foreach (var item in nonMatching)
            {
                if (item.IsA(typeof (ObjectDiff)))
                {
                    var obj = item as ObjectDiff;
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
