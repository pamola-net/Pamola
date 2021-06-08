using System;
using System.Collections.Generic;
using System.Numerics;
using Accord.Math;
using System.Linq;

namespace Pamola.Solvers
{
    public class AccordBaseSolver : ISolver
    {
        public AccordBaseSolver(IReadOnlyList<double> initialGuess) 
        {
            InitialGuess = initialGuess;
            Tolerance = 1e-8;
            StopCriteria = (Y, i) => i >= 100 || Y.All(y => y*y < Tolerance);
        }

        public double Tolerance { get; set; }

        public Func<IReadOnlyList<double>, int, bool> StopCriteria { get; set; }

        public IReadOnlyList<double> InitialGuess { get; set; }


        public IReadOnlyList<double> Solve(Func<IReadOnlyList<double>, IReadOnlyList<double>> equations)
        {
            return IterativeSolve(equations).
                Select((Xk, k) => (Xk, k)).
                First(itk => StopCriteria(equations(itk.Xk).ToList(), itk.k))
                .Xk;
        }

        private IEnumerable<IReadOnlyList<double>> IterativeSolve(Func<IReadOnlyList<double>, IReadOnlyList<double>> funcs)
        {
            var Xk = InitialGuess.ToArray();

            while (true)
            {
                yield return Xk;

                var J = funcs.Jacobian(Xk, Tolerance).Select(j => j.ToArray()).ToArray();
                var F = funcs(Xk).ToArray();

                var deltaX = Matrix.Solve(J, F, true);

                Xk = Xk.Subtract(deltaX);
            }
        }
    }
}
