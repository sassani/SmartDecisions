using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;




namespace MathEngine.AHP
{
    public class AHPModel
    {
        public string ModelName { get; set; }
        public Alternative[] Alternatives { get; set; }
        public AHPNode Goal { get; set; }

        private readonly Dictionary<string, double[]> detailsAlternativesPV = new Dictionary<string, double[]>();
        private readonly Dictionary<string, double[]> detailsCriteriasPV = new Dictionary<string, double[]>();

        public AHPModel()
        {
        }

        public void CreateAhpModelFromJsonString(string json)
        {
            if (json != null)
            {
                var ahpJsonModel = JsonConvert.DeserializeObject<AHPModel>(json);
                ModelName = ahpJsonModel.ModelName;
                Goal = ahpJsonModel.Goal;
                Goal.InheritedWeight = 1;
                Goal.LocalWeight = 1;
                Alternatives = ahpJsonModel.Alternatives;
            }
        }

        public void RunDFS()
        {
            Vector<double> summation = Vector<double>.Build.Dense(Alternatives.Length, 0);

            Stack<AHPNode> queue = new Stack<AHPNode>();
            queue.Push(Goal);
            while (queue.Count > 0)
            {
                AHPNode node = queue.Pop();
                node.FillLocalPriorityVector();
                if (node.Criterias.Count == 0)
                {
                    var pv = node.LocalPriorityVector.Multiply(node.InheritedWeight);
                    summation = summation.Add(pv);
                    detailsAlternativesPV.Add(node.Label, pv.ToArray());
                }
                else
                {
                    for (int i = 0; i < node.Criterias.Count; i++)
                    {
                        node.Criterias[i].InheritedWeight = node.LocalPriorityVector[i] * node.InheritedWeight;
                        node.Criterias[i].LocalWeight = node.LocalPriorityVector[i];
                        queue.Push(node.Criterias[i]);
                        detailsCriteriasPV.Add(node.Criterias[i].Label, node.LocalPriorityVector.ToArray());
                    }
                }
            }
            for (int i = 0; i < Alternatives.Length; i++)
            {
                Alternatives[i].FinalWeight = summation[i];
            }
        }

        public string GetReport()
        {
            var report = new
            {
                Title = ModelName,
                Alternatives,
                Report = Goal,
                Details = new
                {
                    Alternatives = detailsAlternativesPV,
                    Criterias = detailsCriteriasPV
                }
            };
            return JsonConvert.SerializeObject(report);
        }

        public Object ReportAsJson()
        {
            var report = new
            {
                Title = ModelName,
                Alternatives,
                Report = Goal,
                Details = new
                {
                    Alternatives = detailsAlternativesPV,
                    Criterias = detailsCriteriasPV
                }
            };
            return (report);
        }

        public override string ToString()
        {
            string txt = $"AHP model\n\rTitle:\t{ModelName}";

            return txt;
        }


    }
}
