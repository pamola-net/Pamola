using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola
{
    public interface ISolver
    {
        IReadOnlyList<double> Solve(Func<IReadOnlyList<double>, IReadOnlyList<double>> equations);
    }
}
