using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;

namespace Pamola.Transient
{
    public abstract class TransientElement : Element, ITransientComponent
    {
        public TransientElement(int numberOfTerminals) : base(numberOfTerminals)
        {
        }

        protected virtual IReadOnlyCollection<TransientVariable> TransientVariables { get; } = Enumerable.Empty<TransientVariable>().ToArray();

        IReadOnlyCollection<TransientVariable> ITransientComponent.TransientVariables => TransientVariables;
       
    }
}
