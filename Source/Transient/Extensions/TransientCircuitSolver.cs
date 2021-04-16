using System;
using Pamola.Solvers;
using System.Linq;
using System.Collections.Generic;

namespace Pamola.Transient
{
    public static class TransientCircuitSolverExtensions
    {
        public static Func<double,Circuit> SolveTransient(
            this Circuit circuit, 
            ITransientSolver transientSolver, 
            TimeProvider timeProvider, 
            ISolver stateSolver,
            IInterpolator interpolator)
        {

            var stateIterator = circuit.StateIterator(transientSolver, timeProvider, stateSolver).ToCachedEnumerable();

            return new Func<double, Circuit>(t =>
            {
                var state = interpolator.Interpolate(stateIterator, t);
                state.State.Zip(
                    circuit.GetTransientVariables(),
                    (s, v) => {
                        v.Variable.Setter(s);
                        return 0;
                    }).ToList();
                circuit.Solve(stateSolver);
                return circuit;
            });
        }

        public static IEnumerable<TransientVariable> GetTransientVariables(this Circuit circuit) => 
            circuit
            .Components
            .OfType<ITransientComponent>()
            .SelectMany(component => component.TransientVariables); 

        public static IEnumerable<TransientState> StateIterator(
            this Circuit circuit, 
            ITransientSolver transientSolver, 
            TimeProvider timeProvider, 
            ISolver stateSolver)
        {
            var transientVariables = circuit.GetTransientVariables();

            var initialState = new TransientState(0, transientVariables.Select(v => v.Variable.Getter()).ToList());
            var derivatives = transientVariables.Select(v => v.Equation).ToList();

            return transientSolver.Solve(
                initialState,
                derivatives,
                timeProvider,
                s => {
                    transientVariables.Zip(s,(v, ns) => 
                        {
                            v.Variable.Setter(ns);
                            return ns;
                        }).ToList();
                    circuit.Solve(stateSolver);
                });

        }    


    }
}