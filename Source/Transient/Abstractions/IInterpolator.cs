using System.Collections.Generic;

namespace Pamola.Transient
{
    public interface IInterpolator
    {
        TransientState Interpolate(IEnumerable<TransientState> states, double time);
    }
}