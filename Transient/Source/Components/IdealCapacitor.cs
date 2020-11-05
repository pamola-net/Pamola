using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;

namespace Pamola.Transient
{
    public class IdealCapacitor : TransientDipole
    {
        public double Capacitance { get; set; }

        public Complex Charge { get; set; }

        public IdealCapacitor(double capacitance)
        {
            Capacitance = capacitance;
            TransientVariables = new List<TransientVariable>() 
            {
                new TransientVariable(new Variable(() => Charge, x => Charge = x), CapacitorEquation)
            };
        }

        protected override IReadOnlyCollection<Func<Complex>> DipoleEquations => new List<Func<Complex>>() { ChargingEquation };

        protected override IReadOnlyCollection<TransientVariable> TransientVariables { get; }

        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        private Complex CapacitorEquation()
        {
            var I = Positive.Current;
            var C = Capacitance;

            return I / C;
        }

        private Complex ChargingEquation()
        {
            Complex V = Positive.Node.Voltage - Negative.Node.Voltage;

            return V - Charge;
        }
        
    }
}
