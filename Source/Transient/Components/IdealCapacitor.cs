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

        public double Charge { get; set; }

        public IdealCapacitor(double capacitance)
        {
            Capacitance = capacitance;
            TransientVariables = new List<TransientVariable>() 
            {
                new TransientVariable(new Variable(() => Charge, x => Charge = x), CapacitorEquation)
            };
        }

        protected override IReadOnlyCollection<Func<double>> DipoleEquations => new List<Func<double>>() { ChargingEquation };

        protected override IReadOnlyCollection<TransientVariable> TransientVariables { get; }

        protected override IReadOnlyCollection<Variable> Variables => new List<Variable>();

        private double CapacitorEquation() => 
            Positive.Current / Capacitance;
        
        private double ChargingEquation() => 
            Positive.Node.Voltage - Negative.Node.Voltage - Charge;
        
        
    }
}
