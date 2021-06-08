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


        public IReadOnlyList<double> Solve(IReadOnlyList<Func<IReadOnlyList<double>, double>> equations)
        {
            return IterativeSolve(equations).
                Select((Xk, k) => (Xk, k)).
                First(itk => StopCriteria(equations.Select(equation => equation(itk.Xk)).ToList(), itk.k)).
                Xk;
        }

        private IEnumerable<IReadOnlyList<double>> IterativeSolve(IReadOnlyList<Func<IReadOnlyList<double>, double>> funcs)
        {
            var Xk = InitialGuess.ToArray();

            while (true)
            {
                yield return Xk;

                var J = funcs.Jacobian(Xk, Tolerance).Select(j => j.ToArray()).ToArray();
                var F = funcs.Select(func => func(Xk)).ToArray();

                var deltaX = Matrix.Solve(J, F, true);

                Xk = Xk.Subtract(deltaX);
            }
        }
    }
}
