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
        public Dipole() : base(2)
        { }

        public Terminal Positive { get => Terminals.First(); }

        public Terminal Negative { get => Terminals.Last(); }

        private double CurrentBehavior() => Positive.Current + Negative.Current;

        protected override IReadOnlyCollection<Func<double>> Equations
        {
            get => DipoleEquations.Concat(new List<Func<double>>(){ CurrentBehavior }).ToList();
        }

        protected abstract IReadOnlyCollection<Func<double>> DipoleEquations { get; }

    }
}
