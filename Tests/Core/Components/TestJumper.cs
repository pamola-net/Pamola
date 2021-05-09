using Xunit;
using System.Linq;
using Pamola.Components;
using Pamola.Solvers;

namespace Pamola.UT.Components
{
    public class TestJumper
    {
        [Fact]
        public void TestJumperConnectPositive()
        {
            var C = SampleCircuit();
            var J = new Jumper();

            C.Terminals.ConnectAll();
            
            var circuit = (J.Positive.ConnectTo(C.Terminals.First())).GetCircuit();

            circuit.Solve(new AccordBaseSolver(((IComponent)circuit).Variables.Select(v => v.Getter()).ToList()));

            /*Assert.Equal(12.0, J.Positive.Node.Voltage.Real);
            Assert.Equal(0.0, J.Positive.Node.Voltage.Imaginary);
            Assert.Equal(0.0, J.Positive.Current.Real);
            Assert.Equal(0.0, J.Positive.Current.Imaginary);*/

            Assert.InRange(C.Terminals.First().Current.Real, 5.99, 6.01);
            Assert.InRange(C.Terminals.First().Current.Imaginary, -0.01, 0.01);
            Assert.InRange(C.Terminals.Last().Current.Real,-6.01,-5.99);
            Assert.InRange(C.Terminals.Last().Current.Imaginary,-0.01, 0.01);
        }

        private Circuit SampleCircuit()
        {
            var V = new IdealDCVoltageSource(12);
            var R = new IdealResistor(2);
            
            R.Positive.ConnectTo(V.Positive);

            return R.GetCircuit();
        }
    }
}