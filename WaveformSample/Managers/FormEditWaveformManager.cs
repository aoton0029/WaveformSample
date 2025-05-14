using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Core;
using WaveformSample.Waveforms;

namespace WaveformSample.Managers
{
    /// <summary>
    /// FormEditWaveformを管理するクラス
    /// シングルトンパターンを使用して、一つのインスタンスのみ存在することを保証
    /// </summary>
    public class FormEditWaveformManager
    {
        // シングルトンインスタンス
        private static FormEditWaveformManager _instance;

        // 管理するフォームのインスタンス
        private FormEditWaveform _formEditWaveform;

        // スレッドロック用オブジェクト
        private static readonly object _lock = new object();

        /// <summary>
        /// シングルトンインスタンスを取得
        /// </summary>
        public static FormEditWaveformManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FormEditWaveformManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// プライベートコンストラクタ（外部からのインスタンス化を防止）
        /// </summary>
        private FormEditWaveformManager()
        {
        }

        /// <summary>
        /// 編集フォームを表示する
        /// 既にフォームが開かれている場合は、そのフォームにフォーカスする
        /// </summary>
        /// <param name="sequence">編集する波形シーケンス</param>
        /// <param name="projectContext">プロジェクトコンテキスト</param>
        public void ShowEditForm(IWaveformSequence sequence, ProjectContext projectContext)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence), "波形シーケンスがnullです。");

            if (projectContext == null)
                throw new ArgumentNullException(nameof(projectContext), "プロジェクトコンテキストがnullです。");

            // UIスレッドで実行する
            if (Form.ActiveForm?.InvokeRequired == true)
            {
                Form.ActiveForm.Invoke(new Action(() => ShowEditFormInternal(sequence, projectContext)));
            }
            else
            {
                ShowEditFormInternal(sequence, projectContext);
            }
        }

        /// <summary>
        /// 編集フォームを内部的に表示する処理
        /// </summary>
        private void ShowEditFormInternal(IWaveformSequence sequence, ProjectContext projectContext)
        {
            // 既にフォームが表示されている場合
            if (_formEditWaveform != null && !_formEditWaveform.IsDisposed)
            {
                // 現在のフォームにフォーカスを当てる
                _formEditWaveform.SetSequence(sequence, projectContext);
                _formEditWaveform.Activate();

                // 最小化されている場合は元のサイズに戻す
                if (_formEditWaveform.WindowState == FormWindowState.Minimized)
                    _formEditWaveform.WindowState = FormWindowState.Normal;

                return;
            }

            // 新しいフォームを作成
            _formEditWaveform = new FormEditWaveform();

            // FormClosedイベントハンドラを設定
            _formEditWaveform.FormClosed += (sender, e) => _formEditWaveform = null;

            // シーケンスとプロジェクトコンテキストを設定
            _formEditWaveform.SetSequence(sequence, projectContext);

            // モーダレスとして表示
            _formEditWaveform.Show();
        }

        /// <summary>
        /// 編集フォームを閉じる
        /// </summary>
        public void CloseEditForm()
        {
            if (_formEditWaveform != null && !_formEditWaveform.IsDisposed)
            {
                _formEditWaveform.Close();
                _formEditWaveform = null;
            }
        }

        /// <summary>
        /// 編集フォームが表示されているかどうかを確認
        /// </summary>
        /// <returns>フォームが表示されている場合はtrue、そうでない場合はfalse</returns>
        public bool IsEditFormVisible()
        {
            return _formEditWaveform != null && !_formEditWaveform.IsDisposed;
        }
    }
}
