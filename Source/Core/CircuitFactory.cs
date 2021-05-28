using System;
using System.Collections.Generic;
using System.Text;

namespace Pamola
{
    /// <summary>
    /// Provides extension methods for creating <see cref="Circuit"/> objects.
    /// </summary>
    public static class CircuitFactory
    {
        /// <summary>
        /// Creates a <see cref="Circuit"/> based on <paramref name="component"/>'s recursive connections.
        /// </summary>
        /// <param name="component">The source component.</param>
        /// <returns>A Circuit containg the recursive components.</returns>
        public static Circuit GetCircuit(this IComponent component)
        {
            ISet<IComponent> currentComponents = new HashSet<IComponent>();
            component.InsertInternals(currentComponents);
            return new Circuit(currentComponents);
        }
        
        /// <summary>
        /// Internal logic for <see cref="GetCircuit"/>. Recursivelly adds <paramref name="component"/>,
        /// and its <see cref="IComponent.AdjacentComponents"/> to <paramref name="currentComponents"/>.
        /// </summary>
        /// <param name="component">The component being currently inserted.</param>
        /// <param name="currentComponents">A set of all internally conected components.</param>
        private static void InsertInternals(this IComponent component, ISet<IComponent> currentComponents)
        {
            if (currentComponents.Add(component))
            {
                foreach(var adjacent in component.AdjacentComponents)
                {
                    adjacent.InsertInternals(currentComponents);
                }
            }
        }

    }
}
