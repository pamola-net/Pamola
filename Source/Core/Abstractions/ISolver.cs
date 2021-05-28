using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pamola
{
    /// <summary>
    /// Interface for numerical solvers, used to simulate circuits.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Solve a set of <paramref name="equations"/>.
        /// </summary>
        /// <param name="equations">A read-only list of functions to solve, f(x), where x is a 
        /// read-only list of <see cref="Complex"/> variables.</param>
        /// <returns>A read-only list of solved variables.</returns>
        IReadOnlyList<Complex> Solve(IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> equations);
    }
}
