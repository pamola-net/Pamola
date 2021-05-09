using System;
using System.Numerics;
using System.Collections.Generic;

namespace Pamola.Components
{
    /// <summary>
    /// Describes a <see cref=" Jumper"/> component. It can be used for implementing 
    /// <see cref="Terminal"> connections to a <see cref="Circuit"/> operating as an 
    /// <see cref="element"/>, create connections, or even short-circuit terminals.
    /// </summary>
    public class Jumper : Dipole
    {
        /// <summary>
        /// Returns the electric behavior of a jumper.
        /// </summary>
        /// <returns>When both terminals are connected, and the nodes created, the voltage
        /// drop in a jumper is zero.</returns>
        private Complex JumperBehavior()
        {
            if (!Positive.IsConnected() || !Negative.IsConnected()) return new Complex();

            return Positive.Node.Voltage - Negative.Node.Voltage;
        }

        /// <summary>
        /// Implements <see cref="Element.Variables"/>.
        /// </summary>
        /// <typeparam name="Variable"></typeparam>
        /// <returns>A <see cref="Jumper"/> has no variables.</returns>
        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        /// <summary>
        /// Implements <see cref="Dipole.DipoleEquations"/>.
        /// </summary>
        /// <returns>A list containing <see cref="JumperBehavior"/>.</returns>
        protected override IReadOnlyCollection<Func<Complex>> DipoleEquations => 
            new List<Func<Complex>>() {JumperBehavior};
    }
}