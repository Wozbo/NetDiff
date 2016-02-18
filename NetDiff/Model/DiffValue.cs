using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Model
{
    public enum DiffValue
    {
        NotEvaluated = -1,
        NotApplicable = 0,
        DiffersInLength = 1,
        DiffersInOrder = 2,
        DiffersInType = 3
    }
}
