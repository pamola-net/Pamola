using Xunit;
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using Pamola.Components;
using Pamola.Solvers;

namespace Pamola.Transient.UT.Components
{
    public class TestIdealInductor
    {
        public static IEnumerable<object[]> chargeData { get; } = new List<object[]>()
        {
            new object[] {
                10.0,
                10.0,
                0.0, 
            },
            new object[]
            {
                10.0,
                0.0,
                10.0
            }
        }; 

        public static IEnumerable<object[]> equationData {get;} = new List<Object[]>
        {
            new object[]
            {
                10.0,
                200.0
            },
            new object[]
            {
                -10.0,
                -200.0
            }
        };

        [Theory]
        [MemberData(nameof(chargeData))]
        public void TestInductorCharge(
            double ip, 
            double iL, 
            double chargeBalance)
        {
            var L = new IdealInductor(0.05);
            
            ((IComponent)L.Positive).Variables.First().Setter(ip);

            L.Charge = iL;
            
            var chargeEquation = ((ITransientComponent)L).Equations.First();
            
            Assert.Equal(chargeBalance, chargeEquation());
        }

        [Theory]
        [MemberData(nameof(equationData))]
        public void TestInductorEquation(
            double vl,
            double inductorEquationValue
        )
        {
            var L = BuildRLCircuit();

            ((IComponent)L.Positive.Node).Variables.First().Setter(vl); 
            ((IComponent)L.Negative.Node).Variables.First().Setter(default(double));   

            var equation = ((ITransientComponent)L).TransientVariables.First().Equation;

            Assert.Equal(inductorEquationValue, equation());
        }

        [Fact]
        public void TestTransientBehavior()
        {
            var circuit = BuildRLCircuit().GetCircuit();
            var tau = 0.005;

            var transientResponse = circuit.SolveTransient(
                new ExplicitEulerTransientSolver(),
                TimeProviderFactories.ConstantTimeProvider(tau/100),
                new AccordBaseSolver(circuit.Components
                    .SelectMany(c => c.Variables
                    .Select(v => v.Getter()))
                    .ToList()),
                new LinearInterpolator());

            Assert.InRange(transientResponse(1*tau).GetTransientVariables().First().Variable.Getter(), 0.63, 0.64);
            Assert.InRange(transientResponse(2*tau).GetTransientVariables().First().Variable.Getter(), 0.86, 0.87);
            Assert.InRange(transientResponse(3*tau).GetTransientVariables().First().Variable.Getter(), 0.95, 0.96);
            Assert.InRange(transientResponse(4*tau).GetTransientVariables().First().Variable.Getter(), 0.98, 0.99);
            Assert.InRange(transientResponse(5*tau).GetTransientVariables().First().Variable.Getter(), 0.99, 1.00);
        }

        private IdealInductor BuildRLCircuit(double inductance = 0.05)
        {
            var V = new IdealDCVoltageSource(10.0);
            var L = new IdealInductor(inductance);
            var R = new IdealResistor(10.0);

            V.Positive.ConnectTo(L.Positive);
            L.Negative.ConnectTo(R.Positive);
            R.Negative.ConnectTo(V.Negative);

            return L;
        }
    }
}