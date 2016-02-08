using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff
{
    public class DiffCalculator
    {
        private double _tolerance;

        public DiffCalculator(double tolerance=1e-6)
        {
            _tolerance = tolerance;
        }

        public ICollection<DiffedItem> Diff(DynamicObject baseObj, DynamicObject evaluated)
        {
            var fields = baseObj.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);

            var diffed = fields.Select(field => new DiffedItem()
            {
                Field = field,
                BaseObjValue = field.GetValue(baseObj),
                EvaluatedValue = field.GetValue(evaluated),
                Tolerance = _tolerance
            });

            return diffed.ToList();
        }
    }
}
