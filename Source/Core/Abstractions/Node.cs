using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Pamola
{
    /// <summary>
    /// Represents an electrial circuit node.
    /// </summary>
    public sealed class Node :
        Component
    {
        /// <summary>
        /// The node voltage, as a <see cref="Complex"/> variable.
        /// </summary>
        public Complex Voltage { get; private set; }

        /// <summary>
        /// Represents a list of terminals currently connected to <see cref="Node"/>.
        /// </summary>
        /// <typeparam name="Terminal">An terminal belonging to an <see cref="Element"/>.</typeparam>
        /// <returns></returns>
        internal List<Terminal> terminals = new List<Terminal>();

        /// <summary>
        /// Creates a <see cref="Node"/> object.
        /// </summary>
        internal Node(){}

        /// <summary>
        /// Returns a read-only <see cref="Terminal"/> list of all terminals connected to this <see cref="Node"/>.
        /// </summary>
        /// <value></value>
        public IReadOnlyCollection<Terminal> Terminals { get => terminals; }

        /// <summary>
        /// <see cref="AdjacentComponents"/> will return its <see cref="Terminals"/>.  
        /// </summary>
        protected override IReadOnlyCollection<IComponent> AdjacentComponents => Terminals;

        /// <summary>
        /// A read-only collection of variables associated with <see cref="Node"/>.
        /// </summary>
        /// <returns><see cref="Voltage"/> as a <see cref="Variable"/>.</returns>    
        protected override IReadOnlyCollection<Variable> Variables { get => new[] { new Variable(() => Voltage, value => Voltage = value) }; }

        /// <summary>
        /// A <see cref="Node"/> applies Kirchhoff's Current Law (KCL) on all its <see cref="Terminals"/>.
        /// </summary>
        /// <returns><see cref="CurrentSum"/>.</returns>
        protected override IReadOnlyCollection<Func<Complex>> Equations => new List<Func<Complex>>() { CurrentSum };

        /// <summary>
        /// Kirchhoff's Current Law (KCL).
        /// </summary>
        /// <returns>The sum of all <see cref="Terminal.Current"/>.</returns>
        internal Complex CurrentSum()
        {
            return Terminals.Select(t => t.Current).Aggregate((l, r) => l + r);
        }
    }
}
