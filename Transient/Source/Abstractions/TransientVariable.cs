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
    
        public Func<Complex> Equation { get; }

        public TransientVariable(Variable variable, Func<Complex> equation)
        {
            Variable = variable;
            Equation = equation;
        }
    }
}
