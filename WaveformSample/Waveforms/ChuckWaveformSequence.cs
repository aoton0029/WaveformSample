using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Charts;

namespace WaveformSample.Waveforms
{
    public class ChuckWaveformSequence : IWaveformSequence
    {
        public SequenceType SequenceType { get; }

        public int Number { get; }

        public string Name { get; set; }

        public List<WaveformStep> WaveformSteps { get; set; }

        public int SampleRate { get ; set; }

        public int MaxTime => SampleRate * 1000;

        public ChuckChartRenderer ChartRenderer { get; set; }

        public ChuckWaveformSequence(SequenceType sequenceType, int number, string name)
        {
            SequenceType = sequenceType;
            Number = number;
            Name = name;
            WaveformSteps = new List<WaveformStep>();
            SampleRate = 1;
        }
    }
}
