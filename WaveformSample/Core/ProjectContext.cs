using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            CurrentProject = new Project();

            // デフォルトの波形シーケンスを初期化
            InitializeDefaultSequences();
        }

        /// <summary>
        /// デフォルトの波形シーケンスを初期化する
        /// </summary>
        private void InitializeDefaultSequences()
        {
            CurrentProject.ChuckWaveformSequences.Clear();
            CurrentProject.DeChuckWaveformSequences.Clear();

            // 10個のChuckWaveformSequenceを追加
            for (int i = 0; i < 10; i++)
            {
                var chuckSequence = new ChuckWaveformSequence(SequenceType.Chuck, i + 1, $"Chuck_{i + 1}");
                chuckSequence.SampleRate = 44100; // デフォルトのサンプルレート
                CurrentProject.ChuckWaveformSequences.Add(chuckSequence);
            }

            // 10個のDeChuckWaveformSequenceを追加
            for (int i = 0; i < 10; i++)
            {
                var deChuckSequence = new DeChuckWaveformSequence(SequenceType.Chuck, i + 1, $"DeChuck_{i + 1}");
                deChuckSequence.Name = $"DeChuck_{i + 1}";
                CurrentProject.DeChuckWaveformSequences.Add(deChuckSequence);
            }
        }

        /// <summary>
        /// プロジェクトをファイルに保存する
        /// </summary>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveProject(string filePath)
        {
            // プロジェクトをJSONに変換
            string json = CurrentProject.ToJson();

            // ファイルに書き込み
            File.WriteAllText(filePath, json);

            // プロジェクトの状態を更新
            CurrentProject.FilePath = filePath;
            CurrentProject.UpdatedAt = DateTime.Now;
            CurrentProject.IsDirty = false;
        }

        /// <summary>
        /// 現在のプロジェクトを保存する（上書き保存）
        /// </summary>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool SaveCurrentProject()
        {
            if (string.IsNullOrEmpty(CurrentProject.FilePath))
            {
                return false; // ファイルパスが設定されていない場合は失敗
            }

            try
            {
                SaveProject(CurrentProject.FilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ファイルからプロジェクトを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        public void LoadProject(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定されたプロジェクトファイルが見つかりません。", filePath);
            }

            // ファイルを読み込む
            string json = File.ReadAllText(filePath);

            // JSONからプロジェクトを復元
            CurrentProject = Project.FromJson(json);
            CurrentProject.FilePath = filePath;
            CurrentProject.IsDirty = false;
        }

        /// <summary>
        /// ChuckWaveformSequenceを追加する
        /// </summary>
        /// <param name="sequence">追加するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool AddChuckWaveformSequence(ChuckWaveformSequence sequence)
        {
            if (CurrentProject.ChuckWaveformSequences.Count >= 10)
            {
                return false; // 最大数に達している場合は追加しない
            }

            CurrentProject.ChuckWaveformSequences.Add(sequence);
            CurrentProject.IsDirty = true;
            return true;
        }

        /// <summary>
        /// DeChuckWaveformSequenceを追加する
        /// </summary>
        /// <param name="sequence">追加するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool AddDeChuckWaveformSequence(DeChuckWaveformSequence sequence)
        {
            if (CurrentProject.DeChuckWaveformSequences.Count >= 10)
            {
                return false; // 最大数に達している場合は追加しない
            }

            CurrentProject.DeChuckWaveformSequences.Add(sequence);
            CurrentProject.IsDirty = true;
            return true;
        }

        /// <summary>
        /// ChuckWaveformSequenceを削除する
        /// </summary>
        /// <param name="sequence">削除するシーケンス</param>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool RemoveChuckWaveformSequence(ChuckWaveformSequence sequence)
        {
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
            bool result = CurrentProject.DeChuckWaveformSequences.Remove(sequence);
            if (result)
            {
                CurrentProject.IsDirty = true;
            }
            return result;
        }
    }
}
