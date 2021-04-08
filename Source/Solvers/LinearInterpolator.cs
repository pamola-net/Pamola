using System.Linq;
using System.Collections.Generic;

namespace Pamola.Transient
{
    public class LinearInterpolator : IInterpolator
    {
        public TransientState Interpolate(
            IEnumerable<TransientState> states,
            double time
        )
        {
            var itemsBefore = states.TakeWhile(s => s.Time < time);
            var stateBefore = itemsBefore.Last();
            var stateAfter = states.Skip(itemsBefore.Count()).Take(1).First();
            
            var x0 = stateBefore.Time;
            var x1 = stateAfter.Time;
            var x = time;

            var linearCoefficients = stateBefore.State.Zip(
                stateAfter.State, 
                (y0, y1) => (y1 - y0) / (x1 - x0)
                );

            var resultState = linearCoefficients.Zip(
                stateBefore.State,
                (m, y0) => m * (x - x0) + y0);

            return new TransientState(
                time, resultState.ToList()
            );
        }
    }

}