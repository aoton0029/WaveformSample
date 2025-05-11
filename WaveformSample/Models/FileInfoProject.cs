using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveformSample.Models
{
    public class FileInfoProject
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
        public List<FileInfoSequence> ChuckSequences { get; set; } = new List<FileInfoSequence>();

        /// <summary>
        /// DeChuck波形シーケンスの情報リスト
        /// </summary>
        public List<FileInfoSequence> DeChuckSequences { get; set; } = new List<FileInfoSequence>();
    }
}
