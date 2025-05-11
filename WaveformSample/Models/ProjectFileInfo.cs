using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Waveforms;

namespace WaveformSample.Models
{
    /// <summary>
    /// プロジェクトファイルの情報
    /// </summary>
    public class ProjectFileInfo
    {
        /// <summary>
        /// プロジェクト名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// プロジェクトの説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// プロジェクトのバージョン
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Chuck波形シーケンスの情報リスト
        /// </summary>
        public List<SequenceFileInfo> ChuckSequences { get; set; } = new List<SequenceFileInfo>();

        /// <summary>
        /// DeChuck波形シーケンスの情報リスト
        /// </summary>
        public List<SequenceFileInfo> DeChuckSequences { get; set; } = new List<SequenceFileInfo>();
    }

    /// <summary>
    /// シーケンスファイルの情報
    /// </summary>
    public class SequenceFileInfo
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
