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
            // dx/dt = 1/x
            // x(t) = sqrt(2*t+1) quando x(0) = 1

            var theoreticalResult = Enumerable.Range(0, 10)
                .Select(i => 0.01 * i)
                .Select(t => Math.Sqrt(2 * t + 1));

            var x0 = new TransientState()
            {
                State = new[] {
                    new Complex(1, 0.0)
                },
                Time = 0
            };
            var x = 1 / x0.State.First();

            Complex derivative() => 1 / x;

            var derivatives = new List<Func<Complex>>()
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

            var errorsReal = results.Zip(theoreticalResult,
                (s, t) => (s.State.First() - t)/t)
                .Select(e => Math.Abs(e.Real))
                .Average(); // RME
            
            var errorsImag = results.Zip(theoreticalResult,
                (s, t) => (s.State.First() - t)/t)
                .Select(e => Math.Abs(e.Imaginary))
                .Average();
            
            Assert.InRange(errorsReal, 0, 0.005);
            Assert.InRange(errorsImag, 0, 0.00000000001);

        }
    }
}
