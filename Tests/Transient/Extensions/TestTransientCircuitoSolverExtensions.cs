using Xunit;
using Pamola.Components;
using Pamola.Transient;
using System.Linq;

namespace Pamola.UnitTests.Transient.Extensions
{
    // TODO: Rename UT classes and namespaces.
    public class TestTransientCircuitoSolverExtensions
    {
        [Fact]
        public void TestGetTransientVariables()
        {
            var R = new IdealResistor(10.0);
            var C = new IdealCapacitor(0.047);
            var V = new IdealDCVoltageSource(10.0);

            R.Positive.ConnectTo(V.Positive);
            C.Positive.ConnectTo(R.Negative);
            C.Negative.ConnectTo(V.Negative);

            var transientVariables = R.GetCircuit().GetTransientVariables();

            Assert.Single(transientVariables);
            Assert.Equal(((ITransientComponent)C).TransientVariables.Last(), transientVariables.Last());
        }

    }
}