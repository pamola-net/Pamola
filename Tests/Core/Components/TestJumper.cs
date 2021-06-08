using Xunit;
using System.Linq;
using System.Collections.Generic;
using Pamola.Components;
using Pamola.Solvers;
using Pamola.Solvers.UT.Components;
using System;

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

            Assert.Equal(2, C.Components.Where(c => c.GetType() == typeof(Node)).ToList().Count);
            Assert.Equal(0.0, Math.Abs(J.Positive.Node.Voltage), 4);
            Assert.Equal(0.0, Math.Abs(J.Positive.Current), 4);
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

            Assert.Equal(3, C.Components.Where(c => c.GetType() == typeof(Node)).ToList().Count);
            Assert.Equal(0.0, Math.Abs(J.Positive.Node.Voltage), 4);
            Assert.Equal(0.0, Math.Abs(J.Negative.Node.Voltage), 4);
            Assert.Equal(6.0, Math.Abs(J.Negative.Current), 4);
            Assert.Equal(12.0, Math.Abs(R.Positive.Node.Voltage), 4);
        }

        [Fact]
        public void TestShortCircuit()
        {
            var _sc = SampleCircuit();
            var J = (Dipole)_sc.First();
            var R = (Dipole)_sc.Last();

            J.Negative.ConnectTo(R.Negative);

            var closedCircuit = new Jumper();

            closedCircuit.Positive.ConnectTo(R.Positive.Node);
            closedCircuit.Negative.ConnectTo(R.Negative.Node);

            var C = J.GetCircuit();            
            C.Solve(new AccordBaseSolver(((IComponent)C).Variables.Select(v => v.Getter()).ToList()));

            Assert.Equal(3, C.Components.Where(c => c.GetType() == typeof(Node)).ToList().Count);
            Assert.NotEqual(12.0, R.Positive.Node.Voltage, 4);
            Assert.NotEqual(0.0, J.Negative.Node.Voltage, 4);
            Assert.NotEqual(6.0, R.Positive.Current, 4);
            Assert.NotEqual(0.0, closedCircuit.Positive.Current, 4);
        }

        private List<IComponent> SampleCircuit()
        {
            var V = new IdealDCVoltageSource(12.0);
            var R = new IdealResistor(2.0);
            var G = new Ground();
            var J = new Jumper();

            R.Positive.ConnectTo(V.Positive);

            new List<Terminal>(){V.Negative, G.Terminal, J.Positive }.ConnectAll();

            return new List<IComponent>(){J, V, R};
        }
    }
}