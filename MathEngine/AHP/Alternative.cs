using System;
using System.Collections.Generic;
using System.Text;

namespace MathEngine.AHP
{
    public class Alternative
    {
        public string Label { get; set; }
        public string ImageUrl { get; set; }
        public double FinalWeight { get; set; } = 0;
    }
}
