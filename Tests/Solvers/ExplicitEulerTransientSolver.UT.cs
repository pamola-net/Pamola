using Pamola.Components;
using Pamola.Solvers.UT.Components;
using Pamola.Transient;
using System;
using System.Linq;
using System.Numerics;
using Xunit;
using System.Collections.Generic;

namespace Pamola.Solvers.UT
{
    public class ExplicitEulerTransientSolverUT
    {
        [Fact]
        public void SolveExplicitEuler()
        {
            var theoreticalResult = Enumerable.Range(0, 10)
                .Select(i => 0.01 * i)
                .Select(t => Math.Sqrt(2 * t + 1));

            var x0 = new TransientState()
            {
                State = new[] {
                    1.0
                },
                Time = 0
            };
            var x = 1 / x0.State.First();

            double derivative() => 1 / x;

            var derivatives = new List<Func<double>>()
            {
                derivative
            };

            var timeProvider = TimeProviderFactories.ConstantTimeProvider(0.01);
            var solver = new ExplicitEulerTransientSolver();

            var results = solver.Solve(
                x0,
                derivatives,
                timeProvider,
                s => 
                {
                    x = s.First();
                }
            ).Take(10);

            var errors = results.Zip(theoreticalResult,
                (s, t) => (s.State.First() - t)/t)
                .Select(e => Math.Abs(e))
                .Average(); 
            
            Assert.InRange(errors, 0, 0.005);
        }
    }
}
