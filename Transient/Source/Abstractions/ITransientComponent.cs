using System;
using System.Collections.Generic;

namespace Pamola.Transient
{
    public interface ITransientComponent : IComponent
    {
        IReadOnlyCollection<TransientVariable> TransientVariables { get; }
    }
}
