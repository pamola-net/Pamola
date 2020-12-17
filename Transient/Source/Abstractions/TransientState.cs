
using System;
using System.Collections.Generic;
using System.Numerics;


namespace Pamola.Transient
{

    public struct TransientState
    {
        public double Time { get; set; }

        public IReadOnlyList<Complex> State { get; set; }
    }    

}