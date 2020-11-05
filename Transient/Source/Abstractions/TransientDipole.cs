using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Pamola.Components;
using System.Linq;

namespace Pamola.Transient
{
    public abstract class TransientDipole : TransientElement
    {
        public TransientDipole() : base(2)
        {
        }

        public Terminal Positive { get => Terminals.First(); }

        public Terminal Negative { get => Terminals.Last(); }

        private Complex CurrentBehaviour() => Positive.Current + Negative.Current;

        protected override IReadOnlyCollection<Func<Complex>> Equations
        {
            get => DipoleEquations.Concat(new List<Func<Complex>>() { CurrentBehaviour }).ToList();
        }

        protected abstract IReadOnlyCollection<Func<Complex>> DipoleEquations { get; }
    }
}
