using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ProjectContext = new ProjectContext();
        }

        /// <summary>
        /// アプリケーションの初期化
        /// </summary>
        public void Initialize()
        {
            // アプリケーションの初期化処理（必要に応じて実装）
        }

        /// <summary>
        /// アプリケーションの終了処理
        /// </summary>
        public void Shutdown()
        {
            // アプリケーションの終了処理（必要に応じて実装）
        }

        /// <summary>
        /// 新しいプロジェクトを作成する
        /// </summary>
        public void CreateNewProject()
        {
            ProjectContext.CreateNewProject();
        }

        /// <summary>
        /// プロジェクトを保存する
        /// </summary>
        /// <param name="filePath">保存先のファイルパス</param>
        public void SaveProject(string filePath)
        {
            ProjectContext.SaveProject(filePath);
        }

        /// <summary>
        /// 現在のプロジェクトを保存する（上書き保存）
        /// </summary>
        /// <returns>成功した場合はtrue、失敗した場合はfalse</returns>
        public bool SaveCurrentProject()
        {
            return ProjectContext.SaveCurrentProject();
        }

        /// <summary>
        /// プロジェクトを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        public void LoadProject(string filePath)
        {
            ProjectContext.LoadProject(filePath);
        }
    }
}
