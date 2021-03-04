using System.Numerics;
using System.Collections.Generic;

namespace Pamola.Transient
{
    public delegate double TimeProvider(
        IReadOnlyList<Complex> state,
        IReadOnlyList<Complex> derivatives);

}