using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using Pamola.Components;

namespace Pamola.UT
{
    class MockedDipole : 
        Dipole
    {
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        protected override IReadOnlyCollection<Func<double>> DipoleEquations => new List<Func<double>>();
    }
}
