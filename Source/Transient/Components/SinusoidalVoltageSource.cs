using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pamola.Transient
{
    /// <summary>
    /// Describes an <see cref="SinusoidalVoltageSource"/> and its properties.
    /// </summary>
    public class SinusoidalVoltageSource : TransientDipole
    {
        /// <summary>
        /// The electric angle of the source, equal to
        /// </summary>
        /// <value></value>
        public double ElectricAngle { get; set; }

        public double Frequency { get; }

        public double PeakVoltage { get; }

        public SinusoidalVoltageSource(double peakVoltage, double frequency)
        {
            PeakVoltage = peakVoltage;
            Frequency = frequency;
            TransientVariables = new List<TransientVariable>() 
            {
                new TransientVariable(new Variable(() => ElectricAngle, x => ElectricAngle = x.Real), AngleDerivative)
            };
        }

        protected override IReadOnlyCollection<Func<Complex>> DipoleEquations => 
            new List<Func<Complex>> { VoltageDrop };
        
        protected override IReadOnlyCollection<TransientVariable> TransientVariables { get; }


        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();


        private Complex VoltageDrop() =>
            PeakVoltage*Math.Sin(ElectricAngle) - Positive.Node.Voltage + Negative.Node.Voltage;

        private Complex AngleDerivative() => 
            2*Math.PI*Frequency;
        

    }
}