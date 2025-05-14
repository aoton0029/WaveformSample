using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveformSample.Waveforms
{
    public interface IWaveformSequence
    {
        SequenceType SequenceType { get; }
        
        int Number { get; }

        string Name { get; set; }

        int Pitch { get; set; }

        List<WaveformStep> WaveformSteps { get; set; }
    }
}
