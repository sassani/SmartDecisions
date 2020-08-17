using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;

namespace MathEngine.AHP
{
    public class AHPNode
    {
        public string Label { get; set; }
        public double InheritedWeight { get; set; }
        public double LocalWeight { get; set; }
        public List<AHPNode> Criterias { get; set; } = new List<AHPNode>();
        public double[][] Comparison { get; set; }
        [JsonIgnore]
        public Vector<double> LocalPriorityVector { get; set; }
        public double InconsistencyIndex { get; set; }

        public void FillLocalPriorityVector()
        {
            LocalPriorityVector = Solver.PriorityVectorByPcm(Matrix<double>.Build.DenseOfRowArrays(Comparison));
            SetConsistencyIndex();
        }

        private void SetConsistencyIndex()
        {
            InconsistencyIndex = 0;
            int n = Comparison[0].Length;
            for (int i = 0; i < n - 2; i++)
            {
                for (int j = i + 1; j < n - 1; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        double temp = Comparison[k][j] * Comparison[i][k] / Comparison[i][j];
                        if (temp > 1) temp = 1 / temp;
                        temp = 1 - temp;
                        if (temp > InconsistencyIndex) InconsistencyIndex = temp;
                    }
                }
            }
        }
    }
}
