using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;

namespace Pamola.Transient.UT.Mocks
{
    class MockedTransientDipole : TransientDipole
    {
        protected override IReadOnlyCollection<IComponent> AdjacentComponents => base.AdjacentComponents;

        protected override IReadOnlyCollection<Variable> Variables => throw new NotImplementedException();

        protected override IReadOnlyCollection<TransientVariable> TransientVariables => base.TransientVariables;

        protected override IReadOnlyCollection<Func<Complex>> Equations => base.Equations;

        protected override IReadOnlyCollection<Func<Complex>> DipoleEquations => Enumerable.Empty<Func<Complex>>().ToArray();
    }
}
