using Xunit;
using Pamola.Components;
using Pamola.Transient;
using System.Linq;
using System;
using Pamola.Solvers;
using System.Text.Json;

namespace Pamola.UnitTests.Transient.Extensions
{
    // TODO: Rename UT classes and namespaces.
    public class TestTransientCircuitSolverExtensions
    {
        [Fact]
        public void TestGetTransientVariables()
        {
            var circuit = GetSimpleTransientCircuit();

            var transientVariables = circuit.GetTransientVariables();

            var C = circuit.Components.OfType<ITransientComponent>().First();

            Assert.Single(transientVariables);
            Assert.Equal(C.TransientVariables.Last(), transientVariables.Last());
        }

        [Fact]
        public void TestStateIterator()
        {
            var circuit = GetSimpleTransientCircuit();

            var stateIterator = circuit.StateIterator(
                new ExplicitEulerTransientSolver(), 
                TimeProviderFactories.ConstantTimeProvider(0.0047), 
                new AccordBaseSolver(circuit.Components.SelectMany(c => c.Variables.Select(v => v.Getter())).ToList()));

            var RC = 0.47;

            var transientResponse = stateIterator.Take(501).ToList();    

            var teoricResponse = transientResponse.Select(s => 10*(1-Math.Exp(-s.Time/RC)));

            var simulatedResponse = transientResponse.Select(s => s.State.First());

            var norm2 = teoricResponse.Zip(simulatedResponse, (t,s) => ((t-s)*(t-s)).Real).Sum();

            var serialiedSimulation = JsonSerializer.Serialize(simulatedResponse);
            var serialiedTeoretic = JsonSerializer.Serialize(teoricResponse);

            // throw new Exception(serialiedSimulation);
        }

        [Fact]
        public void TestSolveTransient()
        {
            var circuit = GetSimpleTransientCircuit();

            var transientResponse = circuit.SolveTransient(new ExplicitEulerTransientSolver(),
                TimeProviderFactories.ConstantTimeProvider(0.0047),
                new AccordBaseSolver(circuit.Components.SelectMany(c => c.Variables.Select(v => v.Getter())).ToList()),
                new LinearInterpolator());

            var tau = 0.47;

            Assert.InRange(transientResponse(1*tau).GetTransientVariables().First().Variable.Getter().Real, 6.3, 6.4);
            Assert.InRange(transientResponse(2*tau).GetTransientVariables().First().Variable.Getter().Real, 8.6, 8.7);
            Assert.InRange(transientResponse(3*tau).GetTransientVariables().First().Variable.Getter().Real, 9.5, 9.6);
            Assert.InRange(transientResponse(4*tau).GetTransientVariables().First().Variable.Getter().Real, 9.8, 9.9);
            Assert.InRange(transientResponse(5*tau).GetTransientVariables().First().Variable.Getter().Real, 9.9, 10.0);
        }

        private Circuit GetSimpleTransientCircuit()
        {
            var R = new IdealResistor(10.0);
            var C = new IdealCapacitor(0.047);
            var V = new IdealDCVoltageSource(10.0);

            R.Positive.ConnectTo(V.Positive);
            C.Positive.ConnectTo(R.Negative);
            C.Negative.ConnectTo(V.Negative);

            return R.GetCircuit();   
        }

    }
}