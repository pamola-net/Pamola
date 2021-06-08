using System;
using System.Numerics;
using System.Collections.Generic;

namespace Pamola.Components.Phasors
{
    public class VoltageSource : Dipole
    {
        /// <summary>
        /// Frequency domain voltage.
        /// </summary>
        /// <value><see cref="Complex.Magnitude"/> is the peak value.</value>
        public double Voltage { get; private set; }

        //TODO: Lacks constructors
        //TODO: Implement extensions to phasors.

        /// <summary>
        /// The voltage drop accross <see cref="VoltageSource"/> must be equal to <see cref="Voltage"/>.
        /// </summary>
        /// <returns></returns>
        private double VoltageDrop() => 
            (!Positive.IsConnected() || !Negative.IsConnected()) ? 
            default(double) : 
            Positive.Node.Voltage - Negative.Node.Voltage - Voltage;

        /// <summary>
        /// <see cref="VoltageSource"/> has no variables.
        /// </summary>
        /// <typeparam name="Variable">Returns a read-only empty list.</typeparam>
        /// <returns></returns>
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        /// <summary>
        /// Applies <see cref="Voltage"/> to its connected nodes.
        /// </summary>
        /// <returns><see cref="VoltageDrop"/>.</returns>
        protected override IReadOnlyCollection<Func<double>> DipoleEquations => new List<Func<double>>() { VoltageDrop };

    }    
}