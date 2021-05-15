using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola.Components
{
    /// <summary>
    /// Describes an Ideal Resistor.
    /// </summary>
    public class IdealResistor : 
        Dipole
    {
        /// <summary>
        /// The <see cref="IdealResistor"/> resistance in Ohms.
        /// </summary>
        /// <value></value>
        public double Resistance { get; private set; }

        /// <summary>
        /// Creates an instance of <see cref="IdealResistor"/> with given <paramref name="resistance"/>.
        /// </summary>
        /// <param name="resistance">The resistance in Ohms.</param>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when <paramref name="resistance"/> is
        /// negative.</exception>
        public IdealResistor(double resistance)
        {
            if (resistance < 0.0)
                throw new ArgumentOutOfRangeException(
                    nameof(resistance),
                    resistance,
                    "Negative resistance values are phisically impossible.");

            Resistance = resistance; 
        }

        /// <summary>
        /// Describes Ohm's Law applied to this.
        /// </summary>
        /// <returns></returns>
        private Complex OhmsLaw()
        {
            if (!Positive.IsConnected() || !Negative.IsConnected()) return new Complex();

            var V = Positive.Node.Voltage - Negative.Node.Voltage;
            var I = Positive.Current;
            var R = Resistance;

            return V - R * I;
        }   

        /// <summary>
        /// An <see cref="IdealResistor"/> has no variables.
        /// </summary>
        /// <typeparam name="Variable"><see cref="Variable"/>.</typeparam>
        /// <returns>An empty collection of <typeparamref name="Variable"/>.</returns>
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        /// <summary>
        /// The only equation associated with an <see cref="IdealResistor"/> is
        /// <see cref="OhmsLaw"/>.
        /// </summary>
        /// <returns>A read-only collection of functions describing the <see cref="IdealResistor"/> behaviour.</returns>
        protected override IReadOnlyCollection<Func<Complex>> DipoleEquations => new List<Func<Complex>>() { OhmsLaw };
        
    }
}
