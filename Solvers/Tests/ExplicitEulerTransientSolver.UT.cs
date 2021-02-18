using Pamola.Components;
using Pamola.Solvers.UT.Components;
using System;
using System.Linq;
using System.Numerics;
using Xunit;

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
                    new Complex(1)
                },
                Time = 0
            };
            
            Complex derivative(Complex x) => 1 / x;

            var derivatives = new[]
            {
                derivative
            };

            var timeProvider = TimeProviderFactories.ConstantTimeProvider(0.01);
            var solver = new ExplicitEulerTransientSolver();

            var results = solver.Solve(
                x0,
                derivatives,
                timeProvider,
                x => { }
            ).Take(10);

            var errors = results.Zip(theoreticalResult,
                (s, t) => ((s.State.First() - t)/t)-1)
                .Average();
            
            

        }
    }
}
