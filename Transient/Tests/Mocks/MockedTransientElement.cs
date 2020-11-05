using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Pamola.Transient.UT.Mocks
{
    class MockedTransientElement : TransientElement
    {
        public Complex MockedProperty { get; set; }

        public MockedTransientElement(int numberOfTerminals) : base(numberOfTerminals)
        {
        }

        protected override IReadOnlyCollection<IComponent> AdjacentComponents => base.AdjacentComponents;

        protected override IReadOnlyCollection<Variable> Variables => throw new NotImplementedException();

        protected override IReadOnlyCollection<Func<Complex>> Equations => throw new NotImplementedException();

        protected override IReadOnlyCollection<TransientVariable> TransientVariables =>
            new List<TransientVariable>() 
            {
                new TransientVariable(
                   new Variable(() => MockedProperty, value => MockedProperty = value),
                   () => 0.0)
            };
    }
}
