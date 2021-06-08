using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Pamola.UT
{
    public class MockedSolver : ISolver
    {
        public IReadOnlyList<double> SolvedState { get; set; }

        public IReadOnlyList<double> Solve(IReadOnlyList<Func<IReadOnlyList<double>, double>> equations)
        {
            return SolvedState;
        }
    }
}
