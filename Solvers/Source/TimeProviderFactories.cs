

namespace Pamola.Solvers
{
    public static class TimeProviderFactories
    {
        public static TimeProvider ConstantTimeProvider(
            double time
        )
        {
            return new TimeProvider(
                (s, d) => time
            );
        }
    }
}