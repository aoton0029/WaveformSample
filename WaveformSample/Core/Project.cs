using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using WaveformSample.Waveforms;

namespace WaveformSample.Core
{
    public class Project
    {
        /// <summary>
        /// プロジェクト名
        /// </summary>
        public string Name { get; set; } = "新規プロジェクト";

        /// <summary>
        /// プロジェクトの説明
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// プロジェクトのバージョン
        /// </summary>
        public string Version { get; set; } = "1.0";

        /// <summary>
        /// プロジェクトファイルのパス（保存されていない場合はnull）
        /// </summary>
        [JsonIgnore]
        public string FilePath { get; set; } = null;

        /// <summary>
        /// プロジェクトが変更されたかどうか
        /// </summary>
        [JsonIgnore]
        public bool IsDirty { get; set; } = false;

        /// <summary>
        /// Chuck波形シーケンスのリスト（最大10個）
        /// </summary>
        public List<ChuckWaveformSequence> ChuckWaveformSequences { get; set; } = new List<ChuckWaveformSequence>();

        /// <summary>
        /// DeChuck波形シーケンスのリスト（最大10個）
        /// </summary>
        public List<DeChuckWaveformSequence> DeChuckWaveformSequences { get; set; } = new List<DeChuckWaveformSequence>();

        /// <summary>
        /// プロジェクトをJSONに変換する
        /// </summary>
        /// <returns>JSON文字列</returns>
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(this, options);
        }

        /// <summary>
        /// JSON文字列からプロジェクトを復元する
        /// </summary>
        /// <param name="json">JSON文字列</param>
        /// <returns>復元されたプロジェクト</returns>
        public static Project FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var project = JsonSerializer.Deserialize<Project>(json, options);
            project.IsDirty = false;
            return project;
        }
    }
}
