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

        public void FillLocalPriorityVector()
        {
            LocalPriorityVector = Solver.PriorityVectorByPcm(Matrix<double>.Build.DenseOfRowArrays(Comparison));
        }
    }
}
