using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using Pamola.Transient;

namespace Pamola.Solvers
{
    public class ExplicitEulerTransientSolver : ITransientSolver
    {
        public IEnumerable<TransientState> Solve(
            TransientState initialState,
            IReadOnlyList<Func<Complex>> derivatives,
            Func<IReadOnlyList<Complex>, IReadOnlyList<Complex>, double> timeProvider,
            Action<IReadOnlyList<Complex>> solveSystem
        )
        {
            var state = initialState;
            while (true)
            {
               
                yield return state;    

                solveSystem(state.State);

                var stateDerivative = derivatives.Select(x => x()).ToList();
                var timeStep = timeProvider(state.State, stateDerivative);

                state = new TransientState() {
                    Time = state.Time + timeStep,
                    State = state.State.Zip(stateDerivative, (x,dx) => x + dx*timeStep).ToList()
                };
            }
        }
    }
}

