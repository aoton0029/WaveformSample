using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
