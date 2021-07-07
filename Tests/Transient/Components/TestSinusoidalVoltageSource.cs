using Xunit;
using System;
using System.Linq;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using Pamola.Components;
using Pamola.Solvers;
using Pamola.Solvers.UT.Components;
using System.Text.Json;

namespace Pamola.Transient.UT.Components
{
    public class TestSinusoidalVoltageSource
    {
        [Fact]
        public void TestRCCircuit()
        {
            var circuit = BuildRCCircuit();

            var tau = 4.7E-4;

            var solvedCircuit = circuit.SolveTransient(
                new ExplicitEulerTransientSolver(),
                TimeProviderFactories.ConstantTimeProvider(tau / 100),
                new AccordBaseSolver(circuit.Components
                    .SelectMany(c => c.Variables
                    .Select(v => v.Getter()))
                    .ToList()),
                new LinearInterpolator());

            var x = solvedCircuit(60E-3);

            var transientResponse = Enumerable.Range(0, 1000).Select(i => 1E-10 + i * tau / 100).Select(t =>
                    {
                    var c = solvedCircuit(t);
                    return new Dictionary<string, double>()
                    {
                        {"Time", t},
                        {"Voltage", c.Components.OfType<IdealCapacitor>().First().Charge},
                        {"Current", c.Components.OfType<IdealCapacitor>().First().Positive.Current}
                    };
            }).ToList();

            var serialiedSimulation = JsonSerializer.Serialize(transientResponse);  

            using (var f = new StreamWriter("RCtransientResponse.json")) 
            { 
                f.Write(serialiedSimulation);
            }  
        }

        private Circuit BuildRCCircuit()
        {
            var V = new SinusoidalVoltageSource(100, 60);
            var R = new IdealResistor(100);
            var C = new IdealCapacitor(4.7E-6);
            var G = new Ground();

            V.Positive.ConnectTo(R.Positive);
            R.Negative.ConnectTo(C.Positive);
            C.Negative.ConnectTo(V.Negative).ConnectTo(G.Terminal);   

            return V.GetCircuit(); 
        }
    }
}