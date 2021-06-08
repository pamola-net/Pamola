using System;
using System.Linq;
using Xunit;
using Pamola.Components;
using System.Numerics;

namespace Pamola.UT
{
    public class Solver
    {
        [Fact]
        public void SimpleMockedSolvedCircuit()
        {
            var R = new IdealResistor(2);
            var V = new IdealDCVoltageSource(12);

            R.Positive.ConnectTo(V.Positive);
            R.Negative.ConnectTo(V.Negative);

            var circuit = R.GetCircuit();

            var values = new[]
            {
                6.0,
                12.0,
                -6.0,
                6.0,
                0.0,
                -6.0
            }.ToList();

            circuit.Solve(new MockedSolver() { SolvedState = values });

            Assert.Equal(12.0, R.Positive.Node.Voltage);
            Assert.Equal(0.0, R.Negative.Node.Voltage);

            Assert.Equal(6.0, R.Positive.Current);
            Assert.Equal(-6.0, R.Negative.Current);

            Assert.Equal(-6.0, V.Positive.Current);
            Assert.Equal(6.0, V.Negative.Current);

            Assert.All(((IComponent)circuit).Equations.Select(equation => equation()), result => Assert.Equal(0.0, result));
        }
    }
}
