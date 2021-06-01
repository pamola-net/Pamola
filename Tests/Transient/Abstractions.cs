using System;
using System.Linq;
using Xunit;
using System.Numerics;
using System.Collections.Generic;

namespace Pamola.Transient.UT
{
    public class Abstractions
    {
        [Fact]
        public void TransientVariableReturnsValue()
        {
            ITransientComponent element = new Mocks.MockedTransientElement(1)
            {
                MockedProperty = 2.0
            };

            var transientVariable = element.TransientVariables.First();

            Assert.Equal(2.0, transientVariable.Variable.Getter());
        }

        public static IEnumerable<object[]> ValueData { get; } = new List<object[]>()
        {
            new object[]
            {
                2.0,
                -2.0,
                0.0
            },
            new object[] {
                2.0,
                2.0,
                4.0
            },
            new object[] {
                -2.0,
                -2.0,
                -4.0
            }
        };

        [Theory]
        [MemberData(nameof(ValueData))]
        public void MockedDipoleCurrentsBehaveProperly(double positiveCurrent, double negativeCurrent, double currentSum)
        {
            var dipole = new Mocks.MockedTransientDipole();

            IComponent positive = dipole.Positive;
            IComponent negative = dipole.Negative;
            positive.Variables.First().Setter(positiveCurrent);
            negative.Variables.First().Setter(negativeCurrent);

            Assert.Equal(currentSum, ((IComponent)dipole).Equations.First()());
        }

    }
}
