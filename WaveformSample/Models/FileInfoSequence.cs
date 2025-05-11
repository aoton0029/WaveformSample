using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Waveforms;

namespace WaveformSample.Models
{
    public class FileInfoSequence
    {
        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// シーケンス名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ファイルパス（相対パス）
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// シーケンスタイプ
        /// </summary>
        public SequenceType SequenceType { get; set; }
    }
}
