using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pamola.Transient 
{
    public interface ITransientSolver
    {
        IEnumerable<IReadOnlyList<Complex>> Solve(
            IReadOnlyList<Complex> initialState,
            IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> derivatives
        );
    }

}