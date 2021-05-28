﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Accord.Math;
using System.Linq;

namespace Pamola.Solvers
{
    /// <summary>
    /// Implements an <see cref="ISolver"/> using <see href="http://accord-framework.net/">
    /// Accord.NET Framework</see>.
    /// </summary>
    public class AccordBaseSolver : ISolver
    {
        /// <summary>
        /// Creates an <see cref="AccordBaseSolver"/> while setting values 
        /// from an <paramref name="initialGuess"/>.
        /// </summary>
        /// <param name="initialGuess">An initial guess for all variables.</param>
        public AccordBaseSolver(IReadOnlyList<Complex> initialGuess) 
        {
            InitialGuess = initialGuess;
            Tolerance = new Complex(1e-8, 1e-8);
            StopCriteria = (Y, i) => i >= 100 || Y.All(y => y.Magnitude < Tolerance.Magnitude);
        }

        /// <summary>
        /// Tolerance for the solver.
        /// </summary>
        /// <value></value>
        public Complex Tolerance { get; set; }

        /// <summary>
        /// A function describing the stop criteria for the solver.
        /// </summary>
        /// <value></value>
        public Func<IReadOnlyList<Complex>, int, bool> StopCriteria { get; set; }

        /// <summary>
        /// The initial guess for the solver.
        /// </summary>
        /// <value></value>
        public IReadOnlyList<Complex> InitialGuess { get; set; }

        /// <summary>
        /// Solves a given set of <paramref name="equations"/>, until <see cref="StopCriteria"/> is met.
        /// </summary>
        /// <param name="equations">Given equations.</param>
        /// <returns>A list of updates values for all variables.</returns>        
        public IReadOnlyList<Complex> Solve(IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> equations)
        {
            return IterativeSolve(equations).
                Select((Xk, k) => (Xk, k)).
                First(itk => StopCriteria(equations.Select(equation => equation(itk.Xk)).ToList(), itk.k)).
                Xk;
        }

        /// <summary>
        /// Solves and returns a single iteration of the solver.
        /// </summary>
        /// <param name="funcs">Given equations.</param>
        /// <returns>A list of updates values for all variables.</returns>
        private IEnumerable<IReadOnlyList<Complex>> IterativeSolve(IReadOnlyList<Func<IReadOnlyList<Complex>, Complex>> funcs)
        {
            var Xk = InitialGuess.ToArray();

            while (true)
            {
                yield return Xk;

                var J = funcs.Jacobian(Xk, Tolerance).Select(j => j.ToArray()).ToArray();
                var F = funcs.Select(func => func(Xk)).ToArray();

                var Jreal = J.Re();
                var Jimag = J.Im();

                var Freal = F.Re();
                var Fimag = F.Im();

                var deltaXreal = Matrix.Solve(Jreal, Freal, true);
                var deltaXimag = Matrix.Solve(Jimag, Fimag, true);

                var XK1real = Xk.Re().Subtract(deltaXreal);
                var XK1imag = Xk.Im().Subtract(deltaXimag);

                Xk = XK1real.Zip(XK1imag, (real, imag) => new Complex(real, imag)).ToArray();
            }
        }
    }
}
