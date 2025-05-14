using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveformSample.Waveforms
{
    public static class WaveformGenerator
    {
        /// <summary>
        /// 波形を生成する
        /// </summary>
        /// <param name="waveformType">波形の種類</param>
        /// <param name="frequency">周波数</param>
        /// <param name="amplitude">振幅</param>
        /// <param name="duration">持続時間</param>
        /// <returns>生成された波形データ</returns>
        public static double[] GenerateWaveform(WaveformType waveformType, double frequency, double amplitude, double duration)
        {
            int sampleRate = 44100; // サンプリングレート
            int sampleCount = (int)(sampleRate * duration);
            double[] waveformData = new double[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                double t = (double)i / sampleRate;
                switch (waveformType)
                {
                    case WaveformType.Sine:
                        waveformData[i] = amplitude * Math.Sin(2 * Math.PI * frequency * t);
                        break;
                    case WaveformType.Square:
                        waveformData[i] = amplitude * (Math.Sign(Math.Sin(2 * Math.PI * frequency * t)) + 1) / 2;
                        break;
                    case WaveformType.Triangle:
                        waveformData[i] = amplitude * (1 - Math.Abs((t * frequency) % 1 - 0.5) * 4);
                        break;
                    case WaveformType.Sawtooth:
                        waveformData[i] = amplitude * (2 * (t * frequency % 1) - 1);
                        break;
                }
            }
            return waveformData;
        }

    }
}
