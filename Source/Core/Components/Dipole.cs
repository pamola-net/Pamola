using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace Pamola.Components
{
    /// <summary>
    /// Implements an abstract <see cref="Pamola.Components.Dipole"/> circuit element.
    /// </summary>
    public abstract class Dipole : 
        Element
    {
        /// <summary>
        /// Creates a <see cref="Dipole"/> as an <see cref="Element"/> containing
        /// only two <see cref="Terminal"/> objects.
        /// </summary>
        /// <returns></returns>
        public Dipole() : base(2)
        { }

        /// <summary>
        /// Defines the positive <see cref="Terminal"/>.
        /// </summary>
        /// <returns>The first item in <see cref="Element.Terminals"/>.</returns>
        public Terminal Positive { get => Terminals.First(); }

        /// <summary>
        /// Defines the negative <see cref="Terminal"/>.
        /// </summary>
        /// <returns>The second and last item in <see cref="Element.Terminals"/>, since a
        /// <see cref="Dipole"/> has only two terminais.</returns>
        public Terminal Negative { get => Terminals.Last(); }

        /// <summary>
        /// The current entering <see cref="Positive"/> terminal is equal to the current leaving
        /// the <see cref="Negative"/> terminal.
        /// </summary>
        /// <returns>The sum of all <see cref="Terminal.Current"/> flowing into <see cref="Dipole"/>.</returns>
        private Complex CurrentBehaviour() => Positive.Current + Negative.Current;

        /// <summary>
        /// Overrides <see cref="Element.Equations"/>, so that it returns <see cref="CurrentBehaviour"/>, 
        /// as well as <see cref="DipoleEquations"/>.
        /// </summary>
        /// <value>A read-only collection of functions describing the <see cref="Dipole"/> behaviour.</value>
        protected override IReadOnlyCollection<Func<Complex>> Equations
        {
            get => DipoleEquations.Concat(new List<Func<Complex>>(){ CurrentBehaviour }).ToList();
        }

        /// <summary>
        /// Describes the electrical behaviour of an <see cref="Dipole"/> as an abstract property.
        /// </summary>
        /// <value>Electrical behaviour of <see cref="Dipole"/>.</value>
        protected abstract IReadOnlyCollection<Func<Complex>> DipoleEquations { get; }

    }
}
