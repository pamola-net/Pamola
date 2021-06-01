using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola.Components
{
    public class IdealDCVoltageSource:
        Dipole
    {
        public double Voltage { get; private set; }

        public IdealDCVoltageSource(double voltage)
        {
            Voltage = voltage;
        }

        private double VoltageDrop()
        {
            return (!Positive.IsConnected() || !Negative.IsConnected()) ?
                0.0 :
                Positive.Node.Voltage - Negative.Node.Voltage - Voltage;

        }

        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        protected override IReadOnlyCollection<Func<double>> DipoleEquations => new List<Func<double>>() { VoltageDrop };

    }
}
