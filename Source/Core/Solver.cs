﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Pamola
{
    public static class Solver
    {
        public static Circuit Solve(this Circuit circuit, ISolver solver)
        {
            var component = (IComponent)circuit;
            var equations = component.Equations;
            var variables = component.Variables;

            void SetState(IReadOnlyList<double> values)
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
                equations.Select(equation => new Func<IReadOnlyList<double>, double>(
                    values =>
                    {
                        SetState(values);
                        return equation();
                    })).ToList());

            SetState(solvedState);

            return circuit;

            //TODO: Insert a separate method to set all variables in a circuit.
            //TODO: Finish solver class/interface.
            //TODO: Create a ground component.
            //TODO: Create unit tests.

        }
    }
}
