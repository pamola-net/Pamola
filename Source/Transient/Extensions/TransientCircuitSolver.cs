using System;
using Pamola.Solvers;

namespace Pamola.Transient
{
    public static class TransientCircuitSolverExtensions
    {
          public static Func<double,Circuit> SolveTransient(this Circuit circuit)
          {
                return new Func<double, Circuit>(x => circuit);
          }


    }
}