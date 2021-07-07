using System;
using System.Collections.Generic;
using System.Linq;
using NumSharp;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;


namespace Pamola.Solvers
{
    public class TensorFlowSolver : ISolver
    {
        public TensorFlowSolver(IReadOnlyList<double> initialGuess) 
        {
            InitialGuess = initialGuess;
            Tolerance = 1e-8;
            StopCriteria = (Y, i) => i >= 100 || Y.All(y => y*y < Tolerance);
        }

        public double Tolerance { get; set; }

        public Func<IReadOnlyList<double>, int, bool> StopCriteria { get; set; }

        public IReadOnlyList<double> InitialGuess { get; set; }


        public IReadOnlyList<double> Solve(Func<IReadOnlyList<double>, IReadOnlyList<double>> equations)
        {
            return IterativeSolve(equations).
                Select((Xk, k) => (Xk, k)).
                First(itk => StopCriteria(equations(itk.Xk).ToList(), itk.k))
                .Xk;
        }

        private IEnumerable<IReadOnlyList<double>> IterativeSolve(Func<IReadOnlyList<double>, IReadOnlyList<double>> equations)
        {
            var optimizer = keras.optimizers.SGD(0.01f);
            var variables = tf.Variable(InitialGuess.ToArray(), name:"Xk");
            while (true)
            {
                using var g = tf.GradientTape();

                var loss = tf.convert_to_tensor(equations(variables.AsTensor().ToArray<double>().ToList()).Select(x => x * x).ToArray());
                
                var gradients = g.gradient(loss, (variables));    

                optimizer.apply_gradients((gradients, variables));

                yield return variables.AsTensor().ToArray<double>().ToList();
            }
        }
    }
}