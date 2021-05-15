using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola.Components
{
    /// <summary>
    /// Describes an Ideal DC voltage source.
    /// </summary>
    public class IdealDCVoltageSource:
        Dipole
    {
        /// <summary>
        /// The <see cref="IdealDCVoltageSource"/> voltage in Volts.
        /// </summary>
        /// <value></value>
        public double Voltage { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="IdealDCVoltageSource"/>, with given <paramref name="voltage"/>.
        /// </summary>
        /// <param name="voltage">The source's voltage.</param>
        public IdealDCVoltageSource(double voltage)
        {
            Voltage = voltage;
        }

        /// <summary>
        /// The voltage drop through an <see cref="IdealDCVoltageSource"/> is its <see cref="Voltage"/>.
        /// </summary>
        /// <returns>The difference between the voltage drop in through its nodes, and its internal <see cref="Voltage"/>.</returns>
        private Complex VoltageDrop()
        {
            if (!Positive.IsConnected() || !Negative.IsConnected()) return new Complex();

            var V = Positive.Node.Voltage - Negative.Node.Voltage;

            return V - Voltage;

        }

        /// <summary>
        /// An <see cref="IdealDCVoltageSource"/> has no variables.
        /// </summary>
        /// <typeparam name="Variable"><see cref="Variable"/>.</typeparam>
        /// <returns>An empty collection of <typeparamref name="Variable"/>.</returns>
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        /// <summary>
        /// The only equation associated with an <see cref="IdealDCVoltageSource"/> is its 
        /// <see cref="VoltageDrop"/>.
        /// </summary>
        /// <returns>A read-only collection of functions describing the <see cref="IdealDCVoltageSource"/> behaviour.</returns>
        protected override IReadOnlyCollection<Func<Complex>> DipoleEquations => new List<Func<Complex>>() { VoltageDrop };

    }
}
