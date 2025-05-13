using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Charts;

namespace WaveformSample.Waveforms
{
    public class DeChuckWaveformSequence : IWaveformSequence
    {
        public SequenceType SequenceType { get; }

        public int Number { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private List<WaveformStep> _waveformSteps;
        public List<WaveformStep> WaveformSteps
        {
            get => _waveformSteps;
            set
            {
                _waveformSteps = value;
                WaveformStepsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int SampleRate { get; set; }

        public int MaxTime => SampleRate * 1000;

        public ChuckChartRenderer ChartRenderer { get; set; }

        // イベント定義
        public event EventHandler NameChanged;
        public event EventHandler WaveformStepsChanged;

        public DeChuckWaveformSequence(SequenceType sequenceType, int number, string name)
        {
            SequenceType = sequenceType;
            Number = number;
            Name = name;
            WaveformSteps = new List<WaveformStep>();
            SampleRate = 1;
        }
    }
}
