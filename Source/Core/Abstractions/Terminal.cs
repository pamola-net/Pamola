using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Pamola
{
    /// <summary>
    /// Represents a terminal or pin of any <see cref="Element"/>.
    /// </summary>
    public sealed class Terminal : Component
    { 
        /// <summary>
        /// Electric current passing through this terminal (positive when entering the element, negative otherwise), in Amperes.
        /// </summary>
        public Complex Current { get; private set; }

        /// <summary>
        /// Terminal owner <see cref="Pamola.Element"/>.
        /// </summary>
        public Element Element { get; private set; }

        /// <summary>
        /// The <see cref="Pamola.Node"/> this termininal is currently connected to (null when disconnected).
        /// </summary>
        public Node Node { get; internal set; }

        /// <summary>
        /// Creates a terminal for a given owner <see cref="Pamola.Element"/>.
        /// </summary>
        /// <param name="ownerElement">Terminal owner Element.</param>
        internal Terminal(Element ownerElement)
        {
            Element = ownerElement;
        }

        /// <summary>
        /// <see cref="AdjacentComponents"/> returns <see cref="Terminal.Element"/> and <see cref="Terminal.Node"/> (if not null).
        /// </summary>
        /// <returns></returns>
        protected override IReadOnlyCollection<IComponent> AdjacentComponents => 
            (new[] { (IComponent)Element, Node }).Where(component => component != null).ToList();

        /// <summary>
        /// A read-only collection of variables associated with <see cref="Terminal"/>.
        /// </summary>
        /// <returns><see cref="Current"/> as a <see cref="Variable"/>.</returns>  
        protected override IReadOnlyCollection<Variable> Variables { get => new[] { new Variable(() => Current, value => Current = value) }; }
        
    }
}
