using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDiff.Model
{
    public enum DiffValue
    {
        NotEqual = 0,
        Equal = 1,
        DiffersInLength = 2,
        DiffersInOrder = 3,
        TypesDiffer = 4
    }
}
