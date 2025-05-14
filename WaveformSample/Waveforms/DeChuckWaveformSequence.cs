using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Charts;
using WaveformSample.Core;

namespace WaveformSample.Waveforms
{
    public class DeChuckWaveformSequence : WaveformSequenceBase, IDeepCopy<DeChuckWaveformSequence>
    {
        public DeChuckWaveformSequence(int number, string name)
           : base(number, name)
        {
            SequenceType = SequenceType.Chuck;
        }

        public override bool Equals(object? obj)
        {
            if (obj is DeChuckWaveformSequence other)
            {
                return Number == other.Number &&
                       Name == other.Name &&
                       SequenceType == other.SequenceType &&
                       WaveformSteps.SequenceEqual(other.WaveformSteps);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + Number.GetHashCode();
            hash = hash * 31 + (Name?.GetHashCode() ?? 0);
            hash = hash * 31 + SequenceType.GetHashCode();
            foreach (var waveform in WaveformSteps)
            {
                hash = hash * 31 + waveform.GetHashCode();
            }
            return hash;
        }

        public DeChuckWaveformSequence DeepCopy()
        {
            DeChuckWaveformSequence copy = new DeChuckWaveformSequence(Number, Name);
            copy.SequenceType = SequenceType;
            copy.WaveformSteps = new List<WaveformStep>(WaveformSteps.Select(w => w.DeepCopy()));
            return copy;
        }
    }
}
