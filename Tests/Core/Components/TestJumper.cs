using Xunit;
using System.Linq;
using System.Collections.Generic;
using Pamola.Components;
using Pamola.Solvers;
using Pamola.Solvers.UT.Components;

namespace Pamola.UT.Components
{
    public class TestJumper
    {
        [Fact]
        public void TestJumperConnectOneSide()
        {
            var J = (Jumper)SampleCircuit().First();

            var C = J.GetCircuit();
            
            C.Solve(new AccordBaseSolver(((IComponent)C).Variables.Select(v => v.Getter()).ToList()));

            Assert.Equal(2, C.Terminals.Count);

            Assert.Equal(0.0, J.Positive.Node.Voltage.Real, 4);
            Assert.Equal(0.0, J.Positive.Node.Voltage.Imaginary, 4);
            Assert.Equal(0.0, J.Positive.Current.Real, 4);
            Assert.Equal(0.0, J.Positive.Current.Imaginary, 4);
        }

        [Fact]
        public void TestJumperFullyConnected()
        {
            var _sc = SampleCircuit();
            var J = (Dipole)_sc.First();
            var R = (Dipole)_sc.Last();

            J.Negative.ConnectTo(R.Negative);
            
            var C = J.GetCircuit();
            
            C.Solve(new AccordBaseSolver(((IComponent)C).Variables.Select(v => v.Getter()).ToList()));

            Assert.Equal(0.0, J.Positive.Node.Voltage.Real, 4);
            Assert.Equal(0.0, J.Positive.Node.Voltage.Imaginary, 4);
            Assert.Equal(0.0, J.Negative.Node.Voltage.Real, 4);
            Assert.Equal(0.0, J.Negative.Node.Voltage.Imaginary, 4);
            Assert.Equal(6.0, J.Negative.Current.Real, 4);
            Assert.Equal(0.0, J.Negative.Current.Imaginary, 4);
        }

        [Fact]
        public void TestShortCircuit()
        {
            var _sc = SampleCircuit();
            var J = (Dipole)_sc.First();
            var R = (Dipole)_sc.Last();

            J.Negative.ConnectTo(R.Negative);

            var closedCircuit = new Jumper();

            closedCircuit.Positive.ConnectTo(R.Positive);
            closedCircuit.Negative.ConnectTo(R.Negative);

            var C = J.GetCircuit();            
            C.Solve(new AccordBaseSolver(((IComponent)C).Variables.Select(v => v.Getter()).ToList()));

            Assert.Equal(0.0, (R.Positive.Node.Voltage - R.Negative.Node.Voltage).Real, 4);
            Assert.Equal(0.0, (R.Positive.Node.Voltage - R.Negative.Node.Voltage).Imaginary, 4);
            Assert.Equal(0.0, R.Positive.Current.Real, 4);
            Assert.Equal(0.0, R.Positive.Current.Imaginary, 4);  
        }

        private List<IComponent> SampleCircuit()
        {
            var V = new IdealDCVoltageSource(12.0);
            var R = new IdealResistor(2.0);
            var G = new Ground();
            var J = new Jumper();

            R.Positive.ConnectTo(V.Positive);
            V.Negative.ConnectTo(G.Terminal);
            G.Terminal.Node.ConnectTo(J.Positive);

            return new List<IComponent>(){J, V, R};
        }
    }
}