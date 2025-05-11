using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveformSample.Charts
{
    public struct ChartPoint
    {
        public double Time { get; set; }
        public double Value { get; set; }
        public ChartPoint(double x, double y)
        {
            Time = x;
            Value = y;
        }
        public override string ToString()
        {
            return $"({Time}, {Value})";
        }
    }
}
