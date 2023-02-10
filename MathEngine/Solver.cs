using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathEngine
{
    public static class Solver
    {
        public static void Test()
        {
            var m = Matrix<double>.Build.DenseOfArray(new double[2, 2] { { 3, 6 }, { 2, 5 } });
            Console.WriteLine(m);
            var v = Vector<double>.Build.DenseOfArray(new double[2] { 12, 9 });
            Console.WriteLine(v);
            var y = m.Solve(v);
            Console.WriteLine(y);
        }

        public static Vector<double> Normalize(Vector<double> vector)
        {
            Vector<double> result = Vector<double>.Build.Dense(vector.Count);
            double min = vector.Minimum();
            double max = vector.Maximum();
            for (int i = 0; i < vector.Count; i++)
            {
                result[i] = (vector[i] - min) / (max - min);
            }
            return result;
        }

        private static double PiVector (Vector<double> vector)
        {
            double result = 1;
            for (int i = 0; i < vector.Count; i++)  
            {
                result *= vector[i];
            }
            return Math.Pow(result, (double)1 / vector.Count);
        }

        public static Vector<double> PriorityVectorByPcm(Matrix<double> pcm)
        {
            /// Ramik, Jaroslav. (2017). Ranking Alternatives by Pairwise Comparisons Matrix and Priority Vector. Scientific Annals of Economics and Business. 64. 10.1515/saeb-2017-0040. 
            /// https://www.researchgate.net/publication/322669316_Ranking_Alternatives_by_Pairwise_Comparisons_Matrix_and_Priority_Vector
            int n = pcm.RowCount;
            Vector<double> pv = Vector<double>.Build.Dense(n);
            Vector<double> temp = Vector<double>.Build.Dense(n);
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                temp[i] = PiVector(pcm.Row(i));
                sum += temp[i];
            }
            for (int i = 0; i < n; i++)
            {
                pv[i] = temp[i] / sum;
            }

            return pv;
        }
    }
}
