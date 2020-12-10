using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using Pamola.Transient;

namespace Pamola.Solvers
{
    public class ExplicitEulerTransientSolver : ITransientSolver
    {

        public IEnumerable<IReadOnlyList<Complex>> Solve(
            IReadOnlyList<Complex> initialState,
            IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> derivatives,
            Func<IReadOnlyList<Complex>, IReadOnlyList<Complex>, double> timeProvider,
            Action<IReadOnlyList<Complex>> solveSystem
        )
        {
            //TODO: Finish Transient Solver
            solveSystem(initialState);
            while (true)
            {
                var timeStep = timeProvider(state, stateDerivative)

            }
            
            return null;
        }
    }
}

