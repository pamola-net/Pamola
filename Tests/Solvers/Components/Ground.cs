using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace Pamola.Solvers.UT.Components
{
    public class Ground : Element
    {
        public Ground() : base(1)
        { }

        public Terminal Terminal { get => Terminals.First(); }

        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        protected override IReadOnlyCollection<Func<double>> Equations => new List<Func<double>>() { TerminalVoltageIsZero };

        private double TerminalVoltageIsZero()
        {
            return Terminal.Node?.Voltage ?? 0.0;
        }
    }
}
