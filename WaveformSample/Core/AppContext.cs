using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Waveforms;

namespace WaveformSample.Core
{
    public class AppContext
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static AppContext _instance;

        /// <summary>
        /// シングルトンインスタンスを取得する
        /// </summary>
        public static AppContext Instance => _instance ?? (_instance = new AppContext());

        /// <summary>
        /// プロジェクトコンテキスト
        /// </summary>
        public ProjectContext ProjectContext { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private AppContext()
        {
            try
            {
                ProjectContext = new ProjectContext();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("アプリケーションコンテキストの初期化中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// アプリケーションの初期化
        /// </summary>
        public void Initialize()
        {
            try
            {
                // アプリケーションの初期化処理（必要に応じて実装）
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("アプリケーションの初期化中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// アプリケーションの終了処理
        /// </summary>
        public void Shutdown()
        {
            try
            {
                // アプリケーションの終了処理（必要に応じて実装）
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("アプリケーションの終了処理中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// 新しいプロジェクトを作成する
        /// </summary>
        public void CreateNewProject()
        {
            try
            {
                ProjectContext.CreateNewProject();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("新規プロジェクトの作成中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// プロジェクトを保存する
        /// </summary>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveProject(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            try
            {
                ProjectContext.SaveProject(filePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"プロジェクトの保存中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// 現在のプロジェクトを保存する（上書き保存）
        /// </summary>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool SaveCurrentProject()
        {
            try
            {
                return ProjectContext.SaveCurrentProject();
            }
            catch (InvalidOperationException)
            {
                throw; // プロジェクトが存在しない場合など
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("プロジェクトの保存中にエラーが発生しました。", ex);
            }
        }

        /// <summary>
        /// プロジェクトを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        public void LoadProject(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "ファイルパスが指定されていません。");
            }

            try
            {
                ProjectContext.LoadProject(filePath);
            }
            catch (FileNotFoundException)
            {
                throw; // 既に適切な例外がスローされている場合はそのまま伝播
            }
            catch (InvalidDataException)
            {
                throw; // 既に適切な例外がスローされている場合はそのまま伝播
            }
            catch (Exception ex)
            {
                throw new IOException($"プロジェクトの読み込み中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// 単一のシーケンスをファイルとして保存する
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
                ProjectContext.SaveSequence(sequence, filePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"シーケンスの保存中にエラーが発生しました: {filePath}", ex);
            }
        }

        /// <summary>
        /// 単一のシーケンスをファイルから読み込む
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

            try
            {
                return ProjectContext.LoadSequence<T>(filePath);
            }
            catch (Exception ex)
            {
                throw new IOException($"シーケンスの読み込み中にエラーが発生しました: {filePath}", ex);
            }
        }
    }
}
