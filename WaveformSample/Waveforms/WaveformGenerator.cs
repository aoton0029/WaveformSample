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

        /// <summary>
        /// 時間変化する三角波を生成する
        /// </summary>
        /// <param name="currentTimeSec">現在の時間（秒）</param>
        /// <param name="durationSec">持続時間（秒）</param>
        /// <param name="startAmplitude">開始時の振幅</param>
        /// <param name="endAmplitude">終了時の振幅</param>
        /// <param name="startFrequency">開始時の周波数（Hz）</param>
        /// <param name="endFrequency">終了時の周波数（Hz）</param>
        /// <param name="startDcOffset">開始時のDCオフセット</param>
        /// <param name="endDcOffset">終了時のDCオフセット</param>
        /// <param name="phase">位相（ラジアン）</param>
        /// <returns>生成された三角波の値</returns>
        public static double GenerateTriangleWave(
            double currentTimeSec,
            double durationSec,
            double startAmplitude,
            double endAmplitude,
            double startFrequency,
            double endFrequency,
            double startDcOffset,
            double endDcOffset,
            double phase)
        {
            if (durationSec <= 0)
                return 0;

            // 経過率（0.0～1.0）を計算
            double progressRatio = Math.Min(Math.Max(currentTimeSec / durationSec, 0.0), 1.0);

            // 各パラメータを現在時刻に応じて線形補間
            double currentAmplitude = startAmplitude + (endAmplitude - startAmplitude) * progressRatio;
            double currentFrequency = startFrequency + (endFrequency - startFrequency) * progressRatio;
            double currentDcOffset = startDcOffset + (endDcOffset - startDcOffset) * progressRatio;

            // 位相を考慮した時間
            double t = currentTimeSec + phase / (2 * Math.PI * currentFrequency);

            // 三角波を生成
            double triangleWave = 1 - Math.Abs((t * currentFrequency) % 1 - 0.5) * 4;

            // 振幅とDCオフセットを適用
            return currentAmplitude * triangleWave + currentDcOffset;
        }
    }
}
