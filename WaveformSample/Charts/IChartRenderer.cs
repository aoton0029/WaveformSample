using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.UserControls;
using WaveformSample.Waveforms;

namespace WaveformSample.Charts
{
    public interface IChartRenderer
    {
        void RenderChart(UcChart chart, IWaveformSequence sequence);
        //void SetTitle(string title);
        //void SetXLabel(string label);
        //void SetYLabel(string label);
        //void SetGridLines(bool showGrid);
        //void SetAxisLimits(double xMin, double xMax, double yMin, double yMax);
        //void SetLineColor(System.Drawing.Color color);
        //void SetLineWidth(int width);
        //void SetPointSize(int size);
    }
}
