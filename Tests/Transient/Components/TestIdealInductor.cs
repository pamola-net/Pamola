using Xunit;
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using Pamola.Components;

namespace Pamola.Transient.UT.Components
{
    public class TestIdealInductor
    {
        public static IEnumerable<object[]> chargeData { get; } = new List<object[]>()
        {
            new object[] {
                new Complex(10.0, 0.0),
                new Complex(10.0, 0.0),
                new Complex(0.0, 0.0) 
            },
            new object[]
            {
                new Complex(10.0, 0.0),
                new Complex(0.0, -10.0),
                new Complex(10.0, 10.0)
            }
        }; 

        public static IEnumerable<object[]> equationData {get;} = new List<Object[]>
        {
            new object[]
            {
                new Complex(10.0, 0.0),
                new Complex(200.0, 0.0)
            },
            new object[]
            {
                new Complex(0.0, -10.0),
                new Complex(0.0, -200.0)
            }
        };

        [Theory]
        [MemberData(nameof(chargeData))]
        public void TestInductorCharge(
            Complex ip, 
            Complex iL, 
            Complex chargeBalance)
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
            Complex vl,
            Complex inductorEquationValue
        )
        {
            var L = BuildRLCircuit();

            ((IComponent)L.Positive.Node).Variables.First().Setter(vl); 
            ((IComponent)L.Negative.Node).Variables.First().Setter(new Complex(0.0, 0.0));   

            var equation = ((ITransientComponent)L).TransientVariables.First().Equation;

            Assert.Equal(inductorEquationValue, equation());
        }

        private IdealInductor BuildRLCircuit(double inductance = 0.05)
        {
            var L = new IdealInductor(inductance);
            var R = new IdealResistor(10.0);

            L.Positive.ConnectTo(R.Positive);
            L.Negative.ConnectTo(R.Negative);

            return L;
        }
    }
}