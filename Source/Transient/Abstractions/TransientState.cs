
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pamola.Transient
{

    public struct TransientState
    {
        public double Time { get; set; }

        public IReadOnlyList<double> State { get; set; }

        public TransientState(double time, IReadOnlyList<double> state)
        {
            Time = time;
            State = state;
        }
    }    

}