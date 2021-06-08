using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola.Components
{
    public class IdealResistor : 
        Dipole
    {
        public double Resistance { get; private set; }

        public IdealResistor(double resistance)
        {
            if (resistance < 0.0)
                throw new ArgumentOutOfRangeException(
                    nameof(resistance),
                    resistance,
                    "Negative resistance values are phisically impossible.");

            Resistance = resistance; 
        }

        private double OhmsLaw() =>
            (!Positive.IsConnected() || !Negative.IsConnected()) ? 
            0.0 : 
            Positive.Node.Voltage - Negative.Node.Voltage - (Resistance * Positive.Current);

           

        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        protected override IReadOnlyCollection<Func<double>> DipoleEquations => new List<Func<double>>() { OhmsLaw };
        
        //TODO: Create an Ideal Voltage Source.
    }
}
