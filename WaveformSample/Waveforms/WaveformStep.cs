using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveformSample.Waveforms
{
    public class WaveformStep : ObservableObject
    {
        public WaveformType WaveType { get; set; }
        public int Duration { get; set; } // 時間(秒)

        public double StartFrequency { get; set; }
        public double EndFrequency { get; set; }
        public bool IsFrequencySweep { get; set; }

        public double StartAmplitude { get; set; }
        public double EndAmplitude { get; set; }
        public bool IsAmplitudeSweep { get; set; }

        public double StartDCOffset { get; set; }
        public double EndDCOffset { get; set; }
        public bool IsDCOffsetSweep { get; set; }

        public double Phase { get; set; }
    }
}
