using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola
{
    public interface ISolver
    {
        IReadOnlyList<double> Solve(IReadOnlyList<Func<IReadOnlyList<double>, double>> equations);
    }
}
