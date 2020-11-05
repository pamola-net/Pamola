using System;
using System.Linq;
using Xunit;
using System.Numerics;
using System.Collections.Generic;


namespace Pamola.Transient.UT.Components
{
    public class CapacitorUT
    {
        public static IEnumerable<object[]> chargeData { get; } = new List<object[]>()
        {
            new object[]
            {
                new Complex(13.0,5.0),
                new Complex(-2.0,2.0),
                new Complex(0.0,4.0),
                new Complex(15.0,-1.0)
            },
            new object[]
            {
                new Complex(2.0,-2.0),
                new Complex(-2.0,2.0),
                new Complex(0.0,0.0),
                new Complex(4.0,-4.0)
            },
            new object[] {
                new Complex(2.0,2.0),
                new Complex(-1.0,2.0),
                new Complex(4.0,-3.0),
                new Complex(-1.0,3.0)
            },
            new object[] {
                new Complex(-2.5,2.0),
                new Complex(-2.5,2.0),
                new Complex(-2.5,2.0),
                new Complex(2.5,-2.0)
            }
        };

        public static IEnumerable<object[]> equationData { get; } = new List<object[]>()
        {
            new object[]
            {
                1.0,
                new Complex(-2.0,2.0),
                new Complex(-2.0,2.0)
            },
            new object[]
            {
                1e3,
                new Complex(-2.0,2.0),
                new Complex(-0.002,0.002)
            },
            new object[] {
                12e-9,
                new Complex(12.0,0.0),
                new Complex(1.0e9,0.0)
            },
            new object[] {
                7e-6,
                new Complex(-3.5,7),
                new Complex(-0.5e6,1e6)
            }
        };


        [Theory]
        [MemberData(nameof(chargeData))]
        public void CapacitorReturnsCharge(
            Complex v1,
            Complex v2,
            Complex vc,
            Complex chargeBalance)
        {
            var capacitor = new IdealCapacitor(10.0);
            var resistor = new Pamola.Components.IdealResistor(10e9);

            ((IComponent)capacitor.Positive.ConnectTo(resistor.Positive)).Variables.First().Setter(v1);
            ((IComponent)capacitor.Negative.ConnectTo(resistor.Negative)).Variables.First().Setter(v2);

            capacitor.Charge = vc;

            var chargeEquation = ((ITransientComponent)capacitor).Equations.First();

            Assert.Equal(chargeBalance, chargeEquation());
        }

        [Theory]
        [MemberData(nameof(equationData))]
        public void CapacitorEquationReturnsValue(
            double c,
            Complex ic,
            Complex capacitorEquationValue)
        {
            var capacitor = new IdealCapacitor(c);
            var resistor = new Pamola.Components.IdealResistor(10e9);

            capacitor.Positive.ConnectTo(resistor.Positive);
            capacitor.Negative.ConnectTo(resistor.Negative);

            ((IComponent)capacitor.Positive).Variables.First().Setter(ic);

            var equation = ((ITransientComponent)capacitor).TransientVariables.First().Equation;

            Assert.Equal(capacitorEquationValue, equation());

        }
    }
}
