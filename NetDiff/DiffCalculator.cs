using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
            return new List<DiffedItem>();
        }
    }
}
