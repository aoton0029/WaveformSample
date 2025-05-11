using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Waveforms;

namespace WaveformSample.Core
{
    public class ProjectService
    {
        private AppContext _appContext;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProjectService()
        {
            _appContext = AppContext.Instance;
        }

        /// <summary>
        /// 新しいプロジェクトを作成する
        /// </summary>
        public void CreateNewProject()
        {
            _appContext.CreateNewProject();
        }

        /// <summary>
        /// 現在のプロジェクトを取得する
        /// </summary>
        /// <returns>現在のプロジェクト</returns>
        public Project GetCurrentProject()
        {
            return _appContext.ProjectContext.CurrentProject;
        }

        /// <summary>
        /// プロジェクトを指定されたパスに保存する
        /// </summary>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveProjectAs(string filePath)
        {
            _appContext.SaveProject(filePath);
        }

        /// <summary>
        /// 現在のプロジェクトを保存する（上書き保存）
        /// </summary>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool SaveProject()
        {
            return _appContext.SaveCurrentProject();
        }

        /// <summary>
        /// プロジェクトをファイルから読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        public void LoadProject(string filePath)
        {
            _appContext.LoadProject(filePath);
        }

        /// <summary>
        /// プロジェクトが変更されたかどうかを取得する
        /// </summary>
        /// <returns>変更されている場合はtrue、そうでない場合はfalse</returns>
        public bool IsProjectDirty()
        {
            return _appContext.ProjectContext.CurrentProject.IsDirty;
        }

        /// <summary>
        /// プロジェクトが保存されているかどうかを取得する
        /// </summary>
        /// <returns>保存されている場合はtrue、そうでない場合はfalse</returns>
        public bool IsProjectSaved()
        {
            return !string.IsNullOrEmpty(_appContext.ProjectContext.CurrentProject.FilePath);
        }

        /// <summary>
        /// 単一のシーケンスをファイルとして保存する
        /// </summary>
        /// <param name="sequence">保存するシーケンス</param>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveSequenceAs(IWaveformSequence sequence, string filePath)
        {
            _appContext.ProjectContext.SaveSequence(sequence, filePath);
        }

        /// <summary>
        /// 単一のシーケンスをファイルから読み込む
        /// </summary>
        /// <typeparam name="T">シーケンスの型</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>読み込んだシーケンス</returns>
        public T LoadSequence<T>(string filePath) where T : IWaveformSequence
        {
            return _appContext.ProjectContext.LoadSequence<T>(filePath);
        }

        /// <summary>
        /// シーケンスをインポートする
        /// </summary>
        /// <param name="filePath">インポートするファイルのパス</param>
        /// <returns>インポートされたシーケンス</returns>
        public IWaveformSequence ImportSequence(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定されたシーケンスファイルが見つかりません。", filePath);
            }

            try
            {
                // ファイル内容からシーケンスタイプを判断
                string json = File.ReadAllText(filePath);
                if (json.Contains("\"SequenceType\":0") || json.Contains("\"sequenceType\":0") ||
                    json.Contains("\"sequenceType\":\"Chuck\"") || json.Contains("\"SequenceType\":\"Chuck\""))
                {
                    return _appContext.ProjectContext.LoadSequence<ChuckWaveformSequence>(filePath);
                }
                else if (json.Contains("\"SequenceType\":1") || json.Contains("\"sequenceType\":1") ||
                         json.Contains("\"sequenceType\":\"DeChuck\"") || json.Contains("\"SequenceType\":\"DeChuck\""))
                {
                    return _appContext.ProjectContext.LoadSequence<DeChuckWaveformSequence>(filePath);
                }
                else
                {
                    throw new InvalidDataException("シーケンスファイルの形式が不正です。");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"シーケンスのインポート中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// 現在のプロジェクトにシーケンスを追加する
        /// </summary>
        /// <param name="sequence">追加するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool AddSequenceToProject(IWaveformSequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence), "追加するシーケンスがnullです。");
            }

            try
            {
                // シーケンスタイプによって処理を分ける
                switch (sequence.SequenceType)
                {
                    case SequenceType.Chuck:
                        return _appContext.ProjectContext.AddChuckWaveformSequence((ChuckWaveformSequence)sequence);

                    case SequenceType.DeChuck:
                        return _appContext.ProjectContext.AddDeChuckWaveformSequence((DeChuckWaveformSequence)sequence);

                    default:
                        throw new InvalidOperationException($"不明なシーケンスタイプです: {sequence.SequenceType}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("シーケンスの追加中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// 指定されたディレクトリにあるシーケンスファイルを検索する
        /// </summary>
        /// <param name="directory">検索するディレクトリ</param>
        /// <param name="sequenceType">検索するシーケンスタイプ (nullの場合はすべて)</param>
        /// <returns>見つかったシーケンスファイルのパスリスト</returns>
        public List<string> FindSequenceFiles(string directory, SequenceType? sequenceType = null)
        {
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                return new List<string>();
            }

            try
            {
                List<string> result = new List<string>();

                // 拡張子 .wseq のファイルを検索
                string[] files = Directory.GetFiles(directory, "*.wseq", SearchOption.AllDirectories);

                // シーケンスタイプによるフィルタリング
                if (sequenceType.HasValue)
                {
                    foreach (string file in files)
                    {
                        try
                        {
                            // ファイル名からシーケンスタイプを判断
                            string fileName = Path.GetFileName(file);
                            if (sequenceType.Value == SequenceType.Chuck && fileName.StartsWith("Chuck_"))
                            {
                                result.Add(file);
                            }
                            else if (sequenceType.Value == SequenceType.DeChuck && fileName.StartsWith("DeChuck_"))
                            {
                                result.Add(file);
                            }
                        }
                        catch
                        {
                            // ファイル名の解析に失敗した場合は無視
                        }
                    }
                }
                else
                {
                    // フィルタなしならすべて追加
                    result.AddRange(files);
                }

                return result;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
