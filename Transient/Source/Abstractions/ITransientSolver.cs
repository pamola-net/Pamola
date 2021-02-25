using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pamola.Transient 
{
    /// <summary>
    /// Interface for Transient Solvers.
    /// </summary>
    public interface ITransientSolver
    {
        /// <summary>
        /// Yields current variable state for a given circuit.
        /// </summary>
        /// <param name="initialState">Transient variables current state.</param>
        /// <param name="derivatives">Transient variables time derivatives.</param>
        /// <param name="timeProvider">Dinamically chooses time step for each iteration.</param>
        /// <param name="solveSystem">Solves the circuit state for non-transient variables.</param>
        IEnumerable<TransientState> Solve(
            TransientState initialState,
            IReadOnlyList<Func<Complex>> derivatives,
            TimeProvider timeProvider,
            Action<IReadOnlyList<Complex>> solveSystem
        );
    }

}