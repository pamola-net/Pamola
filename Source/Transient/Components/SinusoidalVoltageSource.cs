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
        /// The electric angle of the source, in radians.
        /// </summary>
        /// <value></value>
        public double ElectricAngle { get; set; }

        /// <summary>
        /// The electrical frequency of the source, in Hertz.
        /// </summary>
        /// <value></value>
        public double Frequency { get; }

        /// <summary>
        /// Peak voltage of the source, in Volts.
        /// </summary>
        /// <value></value>
        public double PeakVoltage { get; }

        /// <summary>
        /// Create an instance of <see cref="SinusoidalVoltageSource"/>, with given 
        /// <paramref name="peakVoltage"/> and <paramref name="frequency"/>.
        /// </summary>
        /// <param name="peakVoltage">Peak voltage of the source, in Volts.</param>
        /// <param name="frequency">The electrical frequency of the source, in Hertz.</param>
        public SinusoidalVoltageSource(double peakVoltage, double frequency)
        {
            PeakVoltage = peakVoltage;
            Frequency = frequency;
            TransientVariables = new List<TransientVariable>() 
            {
                new TransientVariable(new Variable(() => ElectricAngle, x => ElectricAngle = x), AngleDerivative)
            };
        }

        /// <summary>
        /// Overrides <see cref="TransientDipole.DipoleEquations"/> with a new list, 
        /// containg the <see cref="VoltageDrop"/>
        /// </summary>
        /// <returns>A read-only collection of functions of <see cref="double"/>.</returns>
        protected override IReadOnlyCollection<Func<double>> DipoleEquations => 
            new List<Func<double>> { VoltageDrop };
        
        /// <summary>
        /// Overrides <see cref="TransientDipole.TransientVariables"/>, with a list containg 
        /// the <see cref="ElectricAngle"/>, and its <see cref="AngleDerivative"/>.
        /// </summary>
        /// <value>A read-only <see cref="TransientVariable"/> collection.</value>
        protected override IReadOnlyCollection<TransientVariable> TransientVariables { get; }

        /// <summary>
        /// Overrides <see cref="Element.Variables"/> with an empty list.
        /// </summary>
        /// <typeparam name="Variable"></typeparam>
        /// <returns></returns>
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        /// <summary>
        /// Calculates the voltage drop in between its terminals as the sine of the 
        /// <see cref="ElectricalAngle"/>.
        /// </summary>
        /// <returns></returns>
        private double VoltageDrop() =>
            PeakVoltage*Math.Sin(ElectricAngle) - Positive.Node.Voltage + Negative.Node.Voltage;

        /// <summary>
        /// The analitical derivative of the <see cref="ElectricalAngle"/>.
        /// </summary>
        /// <returns></returns>
        private double AngleDerivative() => 
            2*Math.PI*Frequency;
        

    }
}