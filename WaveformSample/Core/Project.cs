using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using WaveformSample.Waveforms;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace WaveformSample.Core
{
    public class Project
    {

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

        private string _name = "新規プロジェクト";
        /// <summary>
        /// プロジェクト名
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    IsDirty = true;
                    NameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private string _description = "";
        /// <summary>
        /// プロジェクトの説明
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    IsDirty = true;
                }
            }
        }

        // 他のプロパティも同様に修正...

        private bool _isDirty = false;
        /// <summary>
        /// プロジェクトが変更されたかどうか
        /// </summary>
        [JsonIgnore]
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    DirtyStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        // イベント定義
        /// <summary>
        /// プロジェクト名が変更された時に発生するイベント
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// プロジェクトの変更状態が変更された時に発生するイベント
        /// </summary>
        public event EventHandler DirtyStateChanged;

        // シーケンスプロパティとシーケンス変更時のイベント実装
        private List<ChuckWaveformSequence> _chuckWaveformSequences = new List<ChuckWaveformSequence>();

        /// <summary>
        /// Chuck波形シーケンスのリスト（最大10個）
        /// </summary>
        public List<ChuckWaveformSequence> ChuckWaveformSequences
        {
            get => _chuckWaveformSequences;
            set
            {
                _chuckWaveformSequences = value;
                IsDirty = true;
            }
        }

        private List<DeChuckWaveformSequence> _deChuckWaveformSequences = new List<DeChuckWaveformSequence>();
        /// <summary>
        /// DeChuck波形シーケンスのリスト（最大10個）
        /// </summary>
        public List<DeChuckWaveformSequence> DeChuckWaveformSequences
        {
            get => _deChuckWaveformSequences;
            set
            {
                _deChuckWaveformSequences = value;
                IsDirty = true;
            }
        }
        /// <summary>
        /// シーケンス変更を監視するメソッド
        /// </summary>
        public void MonitorWaveformStepChanges()
        {
            // Chuck波形シーケンスのWaveformStepsの変更を監視
            foreach (var sequence in ChuckWaveformSequences)
            {
                // 既存のイベントハンドラを登録解除（重複登録を防ぐため）
                sequence.NameChanged -= OnSequencePropertyChanged;
                sequence.WaveformStepsChanged -= OnSequencePropertyChanged;

                // イベントハンドラを登録
                sequence.NameChanged += OnSequencePropertyChanged;
                sequence.WaveformStepsChanged += OnSequencePropertyChanged;

                // 各WaveformStepのプロパティ変更を監視
                foreach (var step in sequence.WaveformSteps)
                {
                    // ObservableObjectのPropertyChangedイベントを購読
                    if (step is ObservableObject observableStep)
                    {
                        // 既存のハンドラを解除してから追加（重複登録を防ぐため）
                        observableStep.PropertyChanged -= OnWaveformStepPropertyChanged;
                        observableStep.PropertyChanged += OnWaveformStepPropertyChanged;
                    }
                }
            }

            // DeChuck波形シーケンスのWaveformStepsの変更を監視（同様の実装）
            foreach (var sequence in DeChuckWaveformSequences)
            {
                // 既存のイベントハンドラを登録解除
                sequence.NameChanged -= OnSequencePropertyChanged;
                sequence.WaveformStepsChanged -= OnSequencePropertyChanged;

                // イベントハンドラを登録
                sequence.NameChanged += OnSequencePropertyChanged;
                sequence.WaveformStepsChanged += OnSequencePropertyChanged;

                // 各WaveformStepのプロパティ変更を監視
                foreach (var step in sequence.WaveformSteps)
                {
                    // ObservableObjectのPropertyChangedイベントを購読
                    if (step is ObservableObject observableStep)
                    {
                        // 既存のハンドラを解除してから追加
                        observableStep.PropertyChanged -= OnWaveformStepPropertyChanged;
                        observableStep.PropertyChanged += OnWaveformStepPropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// WaveformStepのプロパティが変更されたときのイベントハンドラ
        /// </summary>
        private void OnWaveformStepPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // WaveformStepのプロパティが変更されたら、プロジェクトが変更されたと見なす
            IsDirty = true;
        }
        /// <summary>
        /// シーケンスのプロパティが変更されたときのイベントハンドラ
        /// </summary>
        private void OnSequencePropertyChanged(object sender, EventArgs e)
        {
            // シーケンスのプロパティが変更されたら、プロジェクトが変更されたと見なす
            IsDirty = true;

            // WaveformStepsが変更された場合は、新しいStepのプロパティ変更も監視
            if (sender is IWaveformSequence sequence)
            {
                foreach (var step in sequence.WaveformSteps)
                {
                    if (step is ObservableObject observableStep)
                    {
                        observableStep.PropertyChanged -= OnWaveformStepPropertyChanged;
                        observableStep.PropertyChanged += OnWaveformStepPropertyChanged;
                    }
                }
            }
        }

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
