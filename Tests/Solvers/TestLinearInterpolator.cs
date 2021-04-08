using System.Numerics;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Pamola.Transient.UT
{
    public class TestLinearInterpolator
    {
        [Fact]
        public void TestInterpolateWorksOnLinear()
        {
            var baseStates = new List<TransientState>()
            {
                new TransientState(0, new List<Complex>() { new Complex(1, 3.5)}),
                new TransientState(1, new List<Complex>() { new Complex(2, 4.9)})
            };
            var interpolator = new LinearInterpolator();
            var result = interpolator.Interpolate(baseStates, 0.7);
            Assert.Equal(1.7, result.State.First().Real);
            Assert.Equal(4.48, result.State.First().Imaginary);
        }
    }
}