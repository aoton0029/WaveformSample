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
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                return JsonSerializer.Serialize(this, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("プロジェクトのシリアライズ中にエラーが発生しました。", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("プロジェクトのJSON変換中に予期しないエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// JSON文字列からプロジェクトを復元する
        /// </summary>
        /// <param name="json">JSON文字列</param>
        /// <returns>復元されたプロジェクト</returns>
        public static Project FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json), "JSONデータが指定されていません。");
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var project = JsonSerializer.Deserialize<Project>(json, options);
                if (project == null)
                {
                    throw new InvalidDataException("プロジェクトの復元に失敗しました。JSONデータが無効です。");
                }

                project.IsDirty = false;
                return project;
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("JSONデータの形式が不正です。", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("プロジェクトの復元中に予期しないエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// ChuckWaveformSequenceを名前で検索する
        /// </summary>
        /// <param name="name">検索する名前</param>
        /// <returns>見つかったシーケンス、見つからない場合はnull</returns>
        public ChuckWaveformSequence FindChuckSequenceByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return ChuckWaveformSequences.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// DeChuckWaveformSequenceを名前で検索する
        /// </summary>
        /// <param name="name">検索する名前</param>
        /// <returns>見つかったシーケンス、見つからない場合はnull</returns>
        public DeChuckWaveformSequence FindDeChuckSequenceByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return DeChuckWaveformSequences.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
