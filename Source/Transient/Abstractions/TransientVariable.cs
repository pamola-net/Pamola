using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Pamola.Transient
{
    //TODO: Change from variable to state
    public class TransientVariable
    {
        public Variable Variable { get; }
    
        public Func<double> Equation { get; }

        public TransientVariable(Variable variable, Func<double> equation)
        {
            Variable = variable;
            Equation = equation;
        }
    }
}
