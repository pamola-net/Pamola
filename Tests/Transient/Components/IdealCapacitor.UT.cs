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
                13.0,
                -2.0,
                0.0,
                15.0
            },
            new object[]
            {
                2.0,
                -2.0,
                0.0,
                4.0
            },
            new object[] {
                2.0,
                -1.0,
                4.0,
                -1.0,
            },
            new object[] {
                -2.5,
                -2.5,
                -2.5,
                2.5
            }
        };

        public static IEnumerable<object[]> equationData { get; } = new List<object[]>()
        {
            new object[]
            {
                1.0,
                -2.0,
                -2.0
            },
            new object[]
            {
                1e3,
                -2.0,
                -0.002
            },
            new object[] {
                12e-9,
                12.0,
                1.0e9
            },
            new object[] {
                7e-6,
                -3.5,
                -0.5e6
            }
        };


        [Theory]
        [MemberData(nameof(chargeData))]
        public void CapacitorReturnsCharge(
            double v1,
            double v2,
            double vc,
            double chargeBalance)
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
            double ic,
            double capacitorEquationValue)
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
