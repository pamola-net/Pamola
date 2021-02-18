using System.Numerics;
using System.Collections.Generic;

namespace Pamola.Solvers
{
    public delegate double TimeProvider(
        IReadOnlyList<Complex> state,
        IReadOnlyList<Complex> derivatives);

}