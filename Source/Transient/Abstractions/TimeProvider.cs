using System.Numerics;
using System.Collections.Generic;

namespace Pamola.Transient
{
    public delegate double TimeProvider(
        IReadOnlyList<double> state,
        IReadOnlyList<double> derivatives);

}