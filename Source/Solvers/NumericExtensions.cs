using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace Pamola.Solvers
{
    public static class NumericExtensions
    {
        public static double Derivative(
            this Func<double, double> func, 
            double value, 
            double tolerance)
        {
            var f1 = func(value + tolerance/2.0);
            var f2 = func(value - tolerance/2.0);

            return (f1 - f2) / tolerance;
        }

        public static Complex Derivative(this Func<double, double> func, double value) => 
            func.Derivative(value, 1e-8);

        public static IReadOnlyList<double> Gradient(
            this Func<IReadOnlyList<double>, double> func,
            IReadOnlyList<double> values,
            double tolerance) =>
            values.
                Select<double, Func<double, double>>((value, i) => (double x) => func(values.Select((currentX, j) => j == i ? x : currentX).ToList())).
                Select((f, i) => f.Derivative(values[i], tolerance)).ToList();
        

        public static IReadOnlyList<double> Gradient(this Func<IReadOnlyList<double>, double> func, IReadOnlyList<double> values) => 
            func.Gradient(values, 1e-8);

        public static IReadOnlyList<IReadOnlyList<double>> Jacobian(
            this IReadOnlyList<Func<IReadOnlyList<double>, double>> funcs, 
            IReadOnlyList<double> values, 
            double tolerance) => 
            funcs.
                Select(f => f.Gradient(values, tolerance)).ToList();
        

        public static IReadOnlyList<IReadOnlyList<double>> Jacobian(this IReadOnlyList<Func<IReadOnlyList<double>, double>> funcs, IReadOnlyList<double> values) => 
            funcs.Jacobian(values, 1e-8);
    }
}
