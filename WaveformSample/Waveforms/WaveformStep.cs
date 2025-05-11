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
        private WaveformType _waveType;
        public WaveformType WaveType
        {
            get => _waveType;
            set => SetProperty(ref _waveType, value);
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        } // 時間(秒)

        private double _startFrequency;
        public double StartFrequency
        {
            get => _startFrequency;
            set => SetProperty(ref _startFrequency, value);
        }

        private double _endFrequency;
        public double EndFrequency
        {
            get => _endFrequency;
            set => SetProperty(ref _endFrequency, value);
        }

        private bool _isFrequencySweep;
        public bool IsFrequencySweep
        {
            get => _isFrequencySweep;
            set => SetProperty(ref _isFrequencySweep, value);
        }

        private double _startAmplitude;
        public double StartAmplitude
        {
            get => _startAmplitude;
            set => SetProperty(ref _startAmplitude, value);
        }

        private double _endAmplitude;
        public double EndAmplitude
        {
            get => _endAmplitude;
            set => SetProperty(ref _endAmplitude, value);
        }

        private bool _isAmplitudeSweep;
        public bool IsAmplitudeSweep
        {
            get => _isAmplitudeSweep;
            set => SetProperty(ref _isAmplitudeSweep, value);
        }

        private double _startDCOffset;
        public double StartDCOffset
        {
            get => _startDCOffset;
            set => SetProperty(ref _startDCOffset, value);
        }

        private double _endDCOffset;
        public double EndDCOffset
        {
            get => _endDCOffset;
            set => SetProperty(ref _endDCOffset, value);
        }

        private bool _isDCOffsetSweep;
        public bool IsDCOffsetSweep
        {
            get => _isDCOffsetSweep;
            set => SetProperty(ref _isDCOffsetSweep, value);
        }

        private double _phase;
        public double Phase
        {
            get => _phase;
            set => SetProperty(ref _phase, value);
        }
    }
}
