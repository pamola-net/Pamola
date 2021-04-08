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
    public class TestTransientCircuitoSolverExtensions
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

            var stateiterator = circuit.StateIterator(
                new ExplicitEulerTransientSolver(), 
                TimeProviderFactories.ConstantTimeProvider(0.0047), 
                new AccordBaseSolver(circuit.Components.SelectMany(c => c.Variables.Select(v => v.Getter())).ToList()));

            var RC = 0.47;

            var transientResponse = stateiterator.Take(501).ToList();    

            var teoricResponse = transientResponse.Select(s => 10*(1-Math.Exp(-s.Time/RC)));

            var simulatedResponse = transientResponse.Select(s => s.State.First());

            var norm2 = teoricResponse.Zip(simulatedResponse, (t,s) => ((t-s)*(t-s)).Real).Sum();

            var serialiedSimulation = JsonSerializer.Serialize(simulatedResponse);
            var serialiedTeoretic = JsonSerializer.Serialize(teoricResponse);

            // throw new Exception(serialiedSimulation);
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