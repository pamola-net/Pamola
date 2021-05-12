using System;
using System.Collections.Generic;
using System.Numerics;


namespace Pamola
{
    /// <summary>
    /// Basic Interface for all Pamola circuit components.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Returns the components adjacent to <see cref="IComponent"/>.
        /// </summary>
        IReadOnlyCollection<IComponent> AdjacentComponents { get; }

        /// <summary>
        /// A collection of appropriate variables relative to the nature of this <see cref="IComponent"/>.
        /// </summary>
        IReadOnlyCollection<Variable> Variables { get; }

        /// <summary>
        /// A collection of appropriate equations relative to the nature of this <see cref="IComponent"/>.
        /// </summary>
        IReadOnlyCollection<Func<Complex>> Equations { get; }

    }
}
