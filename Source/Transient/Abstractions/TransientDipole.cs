using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Pamola.Components;
using System.Linq;

namespace Pamola.Transient
{
    // TODO: Implement inheritance from Dipole and ITransientComponent.
    public abstract class TransientDipole : TransientElement
    {
        public TransientDipole() : base(2)
        {
        }

        public Terminal Positive { get => Terminals.First(); }

        public Terminal Negative { get => Terminals.Last(); }

        private double CurrentBehavior() => Positive.Current + Negative.Current;

        protected override IReadOnlyCollection<Func<double>> Equations
        {
            get => DipoleEquations.Concat(new List<Func<double>>() { CurrentBehavior }).ToList();
        }

        protected abstract IReadOnlyCollection<Func<double>> DipoleEquations { get; }
    }
}
