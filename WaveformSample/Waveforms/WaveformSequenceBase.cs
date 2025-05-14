using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveformSample.Waveforms
{
    /// <summary>
    /// 波形シーケンスの基本クラス
    /// </summary>
    public abstract class WaveformSequenceBase : ObservableObject, IWaveformSequence
    {

        public SequenceType SequenceType { get; protected set; }

        public int Number { get; protected set; }

        public string Key => $"{SequenceType}_{Number + 1:D2}";

        private string _name;
        private int _pitch;
        private List<WaveformStep> _waveformSteps;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public int Pitch { get => _pitch; set => SetProperty(ref _pitch, value); }
        public List<WaveformStep> WaveformSteps { get => _waveformSteps; set => SetProperty(ref _waveformSteps, value); }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sequenceType">シーケンスタイプ</param>
        /// <param name="number">シーケンス番号</param>
        /// <param name="name">シーケンス名</param>
        protected WaveformSequenceBase(int number, string name)
        {
            Number = number;
            _name = name;
            _waveformSteps = new List<WaveformStep>();
            _pitch = 1;
        }

        public List<(int, int)> CreateExportDataValues()
        {
            int max_time = Pitch * 1000;
            int tmp_time = 0;
            int start_time = 0;
            int end_time = 0;
            int cnt = 1;
            List<(int, int)> data = new List<(int, int)>();
            foreach (var step in WaveformSteps)
            {
                for (int i = 0; i < step.Duration; i += Pitch)
                {
                    // 1ms単位でデータを作成
                    end_time = start_time + (int)(step.Duration * 1000.0 / step.Duration);
                    if (end_time > max_time)
                    {
                        end_time = max_time;
                        break;
                    }
                    // 波形の値を計算
                    double value = WaveformGenerator.GenerateWaveform(WaveformType.Sine, step.StartFrequency, step.EndFrequency, step.StartAmplitude, step.EndAmplitude, step.IsFrequencySweep, (double)tmp_time / 1000.0);
                    // データを追加
                    data.Add((cnt, (int)value*10));
                    cnt++;
                }
            }
            return data;
        }
    }
}
