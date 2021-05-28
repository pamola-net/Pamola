using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Pamola
{
    /// <summary>
    /// Provides extension methods for solving a <see cref="Circuit"/>.
    /// </summary>
    public static class Solver
    {
        /// <summary>
        /// Solves a <paramref name="circuit"/> using given <paramref name="solver"/>.
        /// </summary>
        /// <param name="circuit">An electric circuit.</param>
        /// <param name="solver">An user-defined <see cref="ISolver"/>.</param>
        /// <returns></returns>
        public static Circuit Solve(this Circuit circuit, ISolver solver)
        {
            var component = (IComponent)circuit;
            var equations = component.Equations;
            var variables = component.Variables;

            void SetState(IReadOnlyList<Complex> values)
            {
                variables.Zip(
                    values,
                    (variable, value) =>
                    {
                        variable.Setter(value);
                        return value;
                    }).ToList();
            }

            var solvedState = solver.Solve(
                equations.Select(equation => new Func<IReadOnlyList<Complex>, Complex>(
                    values =>
                    {
                        SetState(values);
                        return equation();
                    })).ToList());

            SetState(solvedState);

            return circuit;
        }
    }
}
