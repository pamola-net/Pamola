using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Pamola
{
    /// <summary>
    /// Proxies <see cref="IComponent"/> Interface and hides its members.
    /// </summary>
    public abstract class Component
        : IComponent
    {
        /// <summary>
        /// Returns the components adjacent to this.
        /// </summary>
        protected virtual IReadOnlyCollection<IComponent> AdjacentComponents { get; } = Enumerable.Empty<IComponent>().ToArray();

        /// <summary>
        /// A collection of appropriate variables relative to the nature of this component.
        /// </summary>
        protected virtual IReadOnlyCollection<Variable> Variables { get; } = Enumerable.Empty<Variable>().ToArray();

        /// <summary>
        /// A collection of appropriate equations relative to the nature of this component.
        /// </summary>
        protected virtual IReadOnlyCollection<Func<Complex>> Equations { get; } = Enumerable.Empty<Func<Complex>>().ToArray();

        /// <summary>
        /// Proxies <see cref="IComponent.AdjacentComponents"/>.
        /// </summary>
        IReadOnlyCollection<IComponent> IComponent.AdjacentComponents => AdjacentComponents;
        
        /// <summary>
        /// Proxies <see cref="IComponent.Variables"/>.
        /// </summary>
        IReadOnlyCollection<Variable> IComponent.Variables => Variables;

        /// <summary>
        /// Proxies <see cref="IComponent.Equations"/>.
        /// </summary>
        IReadOnlyCollection<Func<Complex>> IComponent.Equations => Equations;

    }
}
