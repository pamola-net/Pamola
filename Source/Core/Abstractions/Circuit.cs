using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Pamola
{
    /// <summary>
    /// Defines a <see cref="Circuit"/> and its properties.
    /// </summary>
    public class Circuit : Element
    {
        /// <summary>
        /// A read-only <see cref="IComponent"/> collection, present in the the <see cref="Circuit"/> topology.
        /// </summary>
        /// <value>All componentes within the circuit  as an <see cref="IReadOnlyCollection"/>.</value>
        public IReadOnlyCollection<IComponent> Components { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Circuit"/>, from a list of <paramref name="components"/>.
        /// </summary>
        /// <param name="components">An <see cref="IComponent"/> IEnumerable.</param>
        /// <typeparam name="Terminal">Circuit inherits from <see cref="Element"/>. 
        /// All <see cref="Terminal"/> objects contained within <paramref name="components"/> 
        /// are added as possible conections to <see cref="Circuit"/>.</typeparam>
        /// <returns></returns>
        internal Circuit(IEnumerable<IComponent> components) : 
            base(components.
                OfType<Terminal>().
                Where((terminal) => !terminal.IsConnected()))
        {
            Components = components.ToList();
        }
        
        /// <summary>
        /// Return all <see cref="Variable"/> objects of <see cref="Components"/>.
        /// </summary>
        /// <returns>A read-only <see cref="Variable"/> collection.</returns>
        protected override IReadOnlyCollection<Variable> Variables => Components.SelectMany(component => component.Variables).ToList();

        /// <summary>
        /// Return all equations, of all <see cref="Components"/>, functions 
        /// of a <see cref="Variable"/> list of <see cref="Complex"/> values.
        /// </summary>
        /// <returns>A read-only <see cref="Function"/> collection.</returns>
        protected override IReadOnlyCollection<Func<Complex>> Equations => Components.SelectMany(component => component.Equations).ToList();
    }
}
