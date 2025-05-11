using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WaveformSample.Models;
using WaveformSample.Waveforms;

namespace WaveformSample.Core
{
    public class ProjectContext
    {
        /// <summary>
        /// 現在のプロジェクト
        /// </summary>
        public Project CurrentProject { get; private set; }

        /// <summary>
        /// シーケンスの最大数
        /// </summary>
        private const int MaxSequenceCount = 10;

        // プロジェクト関連のイベント定義
        public event EventHandler ProjectNameChanged;
        public event EventHandler ProjectDirtyStateChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProjectContext()
        {
            // 空のプロジェクトを作成
            CreateNewProject();
        }

        /// <summary>
        /// 新しいプロジェクトを作成する
        /// </summary>
        public void CreateNewProject()
        {
            try
            {
                // 既存のプロジェクトからイベント登録を解除
                if (CurrentProject != null)
                {
                    CurrentProject.NameChanged -= OnProjectNameChanged;
                    CurrentProject.DirtyStateChanged -= OnProjectDirtyStateChanged;
                }

                CurrentProject = new Project();

                // 新しいプロジェクトのイベントを購読
                CurrentProject.NameChanged += OnProjectNameChanged;
                CurrentProject.DirtyStateChanged += OnProjectDirtyStateChanged;

                // デフォルトの波形シーケンスを初期化
                InitializeDefaultSequences();

                // シーケンスの変更監視を開始
                CurrentProject.MonitorWaveformStepChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("新規プロジェクト作成中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// デフォルトの波形シーケンスを初期化する
        /// </summary>
        private void InitializeDefaultSequences()
        {
            try
            {
                CurrentProject.ChuckWaveformSequences.Clear();
                CurrentProject.DeChuckWaveformSequences.Clear();

                // 10個のChuckWaveformSequenceを追加
                for (int i = 0; i < 10; i++)
                {
                    var chuckSequence = new ChuckWaveformSequence(SequenceType.Chuck, i + 1, $"Chuck_{i + 1}");
                    CurrentProject.ChuckWaveformSequences.Add(chuckSequence);
                }

                // 10個のDeChuckWaveformSequenceを追加
                for (int i = 0; i < 10; i++)
                {
                    var deChuckSequence = new DeChuckWaveformSequence(SequenceType.DeChuck, i + 1, $"DeChuck_{i + 1}");
                    CurrentProject.DeChuckWaveformSequences.Add(deChuckSequence);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("デフォルトシーケンスの初期化中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// プロジェクトをファイルに保存する
        /// </summary>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveProject(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            if (CurrentProject == null)
            {
                throw new InvalidOperationException("保存するプロジェクトが存在しません。");
            }

            try
            {
                // プロジェクト名とディレクトリパスを取得
                string projectFileName = Path.GetFileNameWithoutExtension(filePath);
                string projectDirPath = Path.GetDirectoryName(filePath);

                // プロジェクトディレクトリを作成 (プロジェクト名のディレクトリ)
                string projectDirectory = Path.Combine(projectDirPath, projectFileName);
                if (!Directory.Exists(projectDirectory))
                {
                    Directory.CreateDirectory(projectDirectory);
                }

                // プロジェクトファイルを新しいパスに設定（プロジェクトディレクトリ内のwprojファイル）
                string newFilePath = Path.Combine(projectDirectory, Path.GetFileName(filePath));

                // Chuckシーケンス用のディレクトリ
                string chuckSequencesDir = Path.Combine(projectDirectory, "Chuck");
                if (!Directory.Exists(chuckSequencesDir))
                {
                    Directory.CreateDirectory(chuckSequencesDir);
                }

                // DeChuckシーケンス用のディレクトリ
                string deChuckSequencesDir = Path.Combine(projectDirectory, "DeChuck");
                if (!Directory.Exists(deChuckSequencesDir))
                {
                    Directory.CreateDirectory(deChuckSequencesDir);
                }

                // シーケンスのパス情報を保持するリスト
                List<FileInfoSequence> chuckSequenceFiles = new List<FileInfoSequence>();
                List<FileInfoSequence> deChuckSequenceFiles = new List<FileInfoSequence>();

                // Chuck波形シーケンスを個別ファイルに保存
                foreach (var sequence in CurrentProject.ChuckWaveformSequences)
                {
                    string sequenceFileName = $"Chuck_{sequence.Number}_{SafeFileName(sequence.Name)}.wseq";
                    string sequenceFilePath = Path.Combine(chuckSequencesDir, sequenceFileName);

                    // シーケンスをJSONに変換して保存
                    SaveSequenceToFile(sequence, sequenceFilePath);

                    // シーケンス情報をリストに追加
                    chuckSequenceFiles.Add(new FileInfoSequence
                    {
                        Number = sequence.Number,
                        Name = sequence.Name,
                        FilePath = Path.Combine("Chuck", sequenceFileName), // 相対パスを保存
                        SequenceType = SequenceType.Chuck
                    });
                }

                // DeChuck波形シーケンスを個別ファイルに保存
                foreach (var sequence in CurrentProject.DeChuckWaveformSequences)
                {
                    string sequenceFileName = $"DeChuck_{sequence.Number}_{SafeFileName(sequence.Name)}.wseq";
                    string sequenceFilePath = Path.Combine(deChuckSequencesDir, sequenceFileName);

                    // シーケンスをJSONに変換して保存
                    SaveSequenceToFile(sequence, sequenceFilePath);

                    // シーケンス情報をリストに追加
                    deChuckSequenceFiles.Add(new FileInfoSequence
                    {
                        Number = sequence.Number,
                        Name = sequence.Name,
                        FilePath = Path.Combine("DeChuck", sequenceFileName), // 相対パスを保存
                        SequenceType = SequenceType.DeChuck
                    });
                }

                // プロジェクト情報のみを含む一時オブジェクトを作成
                var projectInfo = new FileInfoProject
                {
                    Name = CurrentProject.Name,
                    Description = CurrentProject.Description,
                    CreatedAt = CurrentProject.CreatedAt,
                    UpdatedAt = DateTime.Now,
                    Version = CurrentProject.Version,
                    ChuckSequences = chuckSequenceFiles,
                    DeChuckSequences = deChuckSequenceFiles
                };

                // プロジェクト情報をJSONに変換
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                string json = JsonSerializer.Serialize(projectInfo, options);

                // プロジェクトファイルに書き込み（ディレクトリ内のファイル）
                File.WriteAllText(newFilePath, json);

                // プロジェクトの状態を更新
                CurrentProject.FilePath = newFilePath; // 新しいパスに更新
                CurrentProject.UpdatedAt = DateTime.Now;
                CurrentProject.IsDirty = false;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("ファイルの保存先へのアクセス権限がありません。", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"プロジェクトファイルの保存中にエラーが発生しました: {filePath}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"プロジェクトの保存中に予期しないエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// シーケンスを個別ファイルに保存
        /// </summary>
        /// <param name="sequence">保存するシーケンス</param>
        /// <param name="filePath">保存先のファイルパス</param>
        private void SaveSequenceToFile(IWaveformSequence sequence, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(sequence, sequence.GetType(), options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new IOException($"シーケンスの保存中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// ファイル名に使用できない文字を置き換える
        /// </summary>
        /// <param name="fileName">元のファイル名</param>
        /// <returns>安全なファイル名</returns>
        private string SafeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "unnamed";

            // ファイル名に使用できない文字を置き換え
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }


        /// <summary>
        /// 現在のプロジェクトを保存する（上書き保存）
        /// </summary>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool SaveCurrentProject()
        {
            if (CurrentProject == null)
            {
                throw new InvalidOperationException("保存するプロジェクトが存在しません。");
            }

            if (string.IsNullOrEmpty(CurrentProject.FilePath))
            {
                return false; // ファイルパスが設定されていない場合は失敗
            }

            try
            {
                SaveProject(CurrentProject.FilePath);
                return true;
            }
            catch (Exception)
            {
                return false; // 例外が発生した場合は失敗として処理
            }
        }

        /// <summary>
        /// ファイルからプロジェクトを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        public void LoadProject(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定されたプロジェクトファイルが見つかりません。", filePath);
            }

            try
            {
                // プロジェクトファイルを読み込む
                string json = File.ReadAllText(filePath);

                // JSONからプロジェクト情報を復元
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var projectInfo = JsonSerializer.Deserialize<FileInfoProject>(json, options);
                if (projectInfo == null)
                {
                    throw new InvalidDataException("プロジェクトファイルの形式が不正です。");
                }

                // 新しいプロジェクトを作成
                var project = new Project
                {
                    Name = projectInfo.Name,
                    Description = projectInfo.Description,
                    CreatedAt = projectInfo.CreatedAt,
                    UpdatedAt = projectInfo.UpdatedAt,
                    Version = projectInfo.Version,
                    FilePath = filePath,
                    IsDirty = false
                };

                // プロジェクトのディレクトリとシーケンスディレクトリのパスを取得
                string projectDirectory = Path.GetDirectoryName(filePath);

                // Chuck波形シーケンスを読み込む
                project.ChuckWaveformSequences.Clear();
                if (projectInfo.ChuckSequences != null)
                {
                    foreach (var sequenceInfo in projectInfo.ChuckSequences)
                    {
                        string sequenceFilePath = Path.Combine(projectDirectory, sequenceInfo.FilePath);
                        if (File.Exists(sequenceFilePath))
                        {
                            var sequence = LoadSequenceFromFile<ChuckWaveformSequence>(sequenceFilePath);
                            project.ChuckWaveformSequences.Add(sequence);
                        }
                        else
                        {
                            // ファイルが見つからない場合は警告ログを出力
                            Console.WriteLine($"警告: シーケンスファイルが見つかりません: {sequenceFilePath}");
                        }
                    }
                }

                // DeChuck波形シーケンスを読み込む
                project.DeChuckWaveformSequences.Clear();
                if (projectInfo.DeChuckSequences != null)
                {
                    foreach (var sequenceInfo in projectInfo.DeChuckSequences)
                    {
                        string sequenceFilePath = Path.Combine(projectDirectory, sequenceInfo.FilePath);
                        if (File.Exists(sequenceFilePath))
                        {
                            var sequence = LoadSequenceFromFile<DeChuckWaveformSequence>(sequenceFilePath);
                            project.DeChuckWaveformSequences.Add(sequence);
                        }
                        else
                        {
                            // ファイルが見つからない場合は警告ログを出力
                            Console.WriteLine($"警告: シーケンスファイルが見つかりません: {sequenceFilePath}");
                        }
                    }
                }

                // 既存のプロジェクトからイベント登録を解除
                if (CurrentProject != null)
                {
                    CurrentProject.NameChanged -= OnProjectNameChanged;
                    CurrentProject.DirtyStateChanged -= OnProjectDirtyStateChanged;
                }

                // 現在のプロジェクトを設定
                CurrentProject = project;

                // プロジェクトのイベントを購読
                CurrentProject.NameChanged += OnProjectNameChanged;
                CurrentProject.DirtyStateChanged += OnProjectDirtyStateChanged;

                // シーケンスの変更監視を開始
                CurrentProject.MonitorWaveformStepChanges();
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("プロジェクトファイルの形式が不正です。", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"プロジェクトファイルの読み込み中にエラーが発生しました: {filePath}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"プロジェクトの読み込み中に予期しないエラーが発生しました: {filePath}", ex);
            }
        }

        // イベントハンドラ
        private void OnProjectNameChanged(object sender, EventArgs e)
        {
            ProjectNameChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnProjectDirtyStateChanged(object sender, EventArgs e)
        {
            ProjectDirtyStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// シーケンスをファイルから読み込む
        /// </summary>
        /// <typeparam name="T">シーケンスの型</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>読み込んだシーケンス</returns>
        private T LoadSequenceFromFile<T>(string filePath) where T : IWaveformSequence
        {
            try
            {
                string json = File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var sequence = JsonSerializer.Deserialize<T>(json, options);
                if (sequence == null)
                {
                    throw new InvalidDataException($"シーケンスファイルの形式が不正です: {filePath}");
                }

                return sequence;
            }
            catch (Exception ex)
            {
                throw new IOException($"シーケンスの読み込み中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// ChuckWaveformSequenceを追加する
        /// </summary>
        /// <param name="sequence">追加するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool AddChuckWaveformSequence(ChuckWaveformSequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "追加するシーケンスがnullです。");
            }

            if (CurrentProject == null)
            {
                throw new InvalidOperationException("プロジェクトが存在しません。");
            }

            if (CurrentProject.ChuckWaveformSequences.Count >= MaxSequenceCount)
            {
                throw new InvalidOperationException($"Chuck波形シーケンスの最大数({MaxSequenceCount})に達しています。");
            }

            try
            {
                CurrentProject.ChuckWaveformSequences.Add(sequence);
                CurrentProject.IsDirty = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Chuck波形シーケンスの追加中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// DeChuckWaveformSequenceを追加する
        /// </summary>
        /// <param name="sequence">追加するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool AddDeChuckWaveformSequence(DeChuckWaveformSequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "追加するシーケンスがnullです。");
            }

            if (CurrentProject == null)
            {
                throw new InvalidOperationException("プロジェクトが存在しません。");
            }

            if (CurrentProject.DeChuckWaveformSequences.Count >= MaxSequenceCount)
            {
                throw new InvalidOperationException($"DeChuck波形シーケンスの最大数({MaxSequenceCount})に達しています。");
            }

            try
            {
                CurrentProject.DeChuckWaveformSequences.Add(sequence);
                CurrentProject.IsDirty = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("DeChuck波形シーケンスの追加中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// ChuckWaveformSequenceを削除する
        /// </summary>
        /// <param name="sequence">削除するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool RemoveChuckWaveformSequence(ChuckWaveformSequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "削除するシーケンスがnullです。");
            }

            if (CurrentProject == null)
            {
                throw new InvalidOperationException("プロジェクトが存在しません。");
            }

            bool result = CurrentProject.ChuckWaveformSequences.Remove(sequence);
            if (result)
            {
                CurrentProject.IsDirty = true;
            }
            return result;
        }

        /// <summary>
        /// DeChuckWaveformSequenceを削除する
        /// </summary>
        /// <param name="sequence">削除するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool RemoveDeChuckWaveformSequence(DeChuckWaveformSequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "削除するシーケンスがnullです。");
            }

            if (CurrentProject == null)
            {
                throw new InvalidOperationException("プロジェクトが存在しません。");
            }

            bool result = CurrentProject.DeChuckWaveformSequences.Remove(sequence);
            if (result)
            {
                CurrentProject.IsDirty = true;
            }
            return result;
        }

        /// <summary>
        /// シングルシーケンスをファイルとして保存する
        /// </summary>
        /// <param name="sequence">保存するシーケンス</param>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveSequence(IWaveformSequence sequence, string filePath)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "保存するシーケンスがnullです。");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            try
            {
                // ディレクトリが存在しない場合は作成
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // シーケンスをJSONに変換して保存
                SaveSequenceToFile(sequence, filePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"シーケンスの保存中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// シングルシーケンスをファイルから読み込む
        /// </summary>
        /// <typeparam name="T">シーケンスの型</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>読み込んだシーケンス</returns>
        public T LoadSequence<T>(string filePath) where T : IWaveformSequence
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定されたシーケンスファイルが見つかりません。", filePath);
            }

            return LoadSequenceFromFile<T>(filePath);
        }

        /// <summary>
        /// 指定したシーケンスを更新後、プロジェクトを保存する
        /// </summary>
        /// <param name="sequence">更新するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool UpdateAndSaveSequence(IWaveformSequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "更新するシーケンスがnullです。");
            }

            if (CurrentProject == null)
            {
                throw new InvalidOperationException("プロジェクトが存在しません。");
            }

            if (string.IsNullOrEmpty(CurrentProject.FilePath))
            {
                return false; // プロジェクトが保存されていない場合は失敗
            }

            try
            {
                // シーケンスの種類によって処理を分ける
                switch (sequence.SequenceType)
                {
                    case SequenceType.Chuck:
                        // 既存のChuckシーケンスを探す
                        var chuckIndex = CurrentProject.ChuckWaveformSequences.FindIndex(s => s.Number == sequence.Number);
                        if (chuckIndex >= 0)
                        {
                            // 既存のシーケンスを更新
                            CurrentProject.ChuckWaveformSequences[chuckIndex] = (ChuckWaveformSequence)sequence;
                        }
                        break;

                    case SequenceType.DeChuck:
                        // 既存のDeChuckシーケンスを探す
                        var deChuckIndex = CurrentProject.DeChuckWaveformSequences.FindIndex(s => s.Number == sequence.Number);
                        if (deChuckIndex >= 0)
                        {
                            // 既存のシーケンスを更新
                            CurrentProject.DeChuckWaveformSequences[deChuckIndex] = (DeChuckWaveformSequence)sequence;
                        }
                        break;
                }

                // プロジェクトが変更されたことをマーク
                CurrentProject.IsDirty = true;

                // プロジェクトを保存
                return SaveCurrentProject();
            }
            catch (Exception)
            {
                return false; // 例外が発生した場合は失敗として処理
            }
        }
    }
}
