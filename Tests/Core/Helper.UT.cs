using System;
using System.Linq;
using Xunit;
using Pamola.Components;
using Pamola.Transient;
using System.Numerics;

namespace Pamola.UT
{
    /// <summary>
    /// /// Refers to tests of <see cref="Pamola.Helper"/>.
    /// </summary>
    public class HelperUT
    {
        [Fact]
        public void ThrowsOnNull()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowOnNull());
        }

        [Fact]
        public void ChainsObjects()
        {
            object obj = new object();
            Assert.Equal(obj, obj.ThrowOnNull());
        }

        [Fact]
        public void ToCacheEnumerableHitsOncePerExpansion()
        {
            var hitCount = 0;
            var cachedEnumerable = Enumerable.Range(0,10).Select(
                i => 
                {
                    hitCount++;
                    return i;
                }
            ).ToCachedEnumerable();

            var firstItem = cachedEnumerable.First();
            var firstItem2 = cachedEnumerable.First();

            Assert.Equal(1, hitCount);
            Assert.Equal(firstItem, firstItem2);
        }

        [Fact]
        public void ToCachedEnumerableHitsEqualExpansionSize()
        {
            var hitCount = 0;
            var cachedEnumerable = Enumerable.Range(0,10).Select(
                i => 
                {
                    hitCount++;
                    return i;
                }
            ).ToCachedEnumerable();

            var first4Items = cachedEnumerable.Take(4).ToList();
            var first5Items = cachedEnumerable.Take(5).ToList();
            Assert.Equal(5, hitCount);
            Assert.Equal(first4Items, first5Items.Take(4));
        }

        [Fact]
        public void TestSetTransientVariables()
        {
            var R = new IdealResistor(10.0);
            var C = new IdealCapacitor(0.047);
            var V = new IdealDCVoltageSource(10.0);

            R.Positive.ConnectTo(V.Positive);
            C.Positive.ConnectTo(R.Negative);
            C.Negative.ConnectTo(V.Negative);

            var circuit = R.GetCircuit();   

            var s = new[] {
                new Complex(10.0, 0)
            };

            circuit.SetTransientVariables(s);

            Assert.Equal(s.First(), C.Charge);
        }
    }
}
