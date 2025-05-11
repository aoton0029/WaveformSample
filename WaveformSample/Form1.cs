using WaveformSample.Core;
using WaveformSample.Waveforms;

namespace WaveformSample
{
    public partial class Form1 : Form
    {
        // プロジェクトサービスのインスタンス
        private ProjectService _projectService;

        public Form1()
        {
            InitializeComponent();
            _projectService = new ProjectService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // フォーム読み込み時の初期化処理
            UpdateStatusBar("準備完了");
        }

        #region ファイルメニューイベントハンドラ

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 変更されたプロジェクトがある場合は保存確認
                if (_projectService.IsProjectDirty() && ConfirmSaveChanges())
                {
                    return; // キャンセルされた場合は処理を中断
                }

                // 新規プロジェクト作成処理
                _projectService.CreateNewProject();
                UpdateStatusBar("新規プロジェクトを作成しました");
            }
            catch (Exception ex)
            {
                ShowError("新規プロジェクト作成中にエラーが発生しました", ex);
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 変更されたプロジェクトがある場合は保存確認
                if (_projectService.IsProjectDirty() && ConfirmSaveChanges())
                {
                    return; // キャンセルされた場合は処理を中断
                }

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "波形プロジェクトファイル (*.wproj)|*.wproj|すべてのファイル (*.*)|*.*";
                    openFileDialog.Title = "プロジェクトを開く";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // プロジェクトを読み込む
                            _projectService.LoadProject(openFileDialog.FileName);
                            UpdateStatusBar($"プロジェクトを開きました: {openFileDialog.FileName}");

                            // UIを更新する必要があればここで実装
                            UpdateUIWithCurrentProject();
                        }
                        catch (FileNotFoundException ex)
                        {
                            ShowError($"指定されたファイルが見つかりません: {openFileDialog.FileName}", ex);
                        }
                        catch (InvalidDataException ex)
                        {
                            ShowError("プロジェクトファイルの形式が正しくありません。", ex);
                        }
                        catch (Exception ex)
                        {
                            ShowError("プロジェクトを開く際にエラーが発生しました", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("プロジェクトを開く処理中にエラーが発生しました", ex);
            }
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // プロジェクトがまだ保存されていない場合は「名前を付けて保存」を実行
                if (!_projectService.IsProjectSaved())
                {
                    saveProjectAsToolStripMenuItem_Click(sender, e);
                    return;
                }

                // プロジェクトを保存（上書き保存）
                if (_projectService.SaveProject())
                {
                    UpdateStatusBar("プロジェクトを保存しました");
                }
                else
                {
                    ShowError("プロジェクト保存中にエラーが発生しました", new Exception("保存に失敗しました。"));
                }
            }
            catch (Exception ex)
            {
                ShowError("プロジェクト保存中にエラーが発生しました", ex);
            }
        }

        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "波形プロジェクトファイル (*.wproj)|*.wproj|すべてのファイル (*.*)|*.*";
                    saveFileDialog.Title = "名前を付けてプロジェクトを保存";
                    saveFileDialog.DefaultExt = "wproj";
                    saveFileDialog.AddExtension = true;

                    // 既存のプロジェクトの場合、そのファイルパスを初期ディレクトリに設定
                    var currentProject = _projectService.GetCurrentProject();
                    if (currentProject != null && !string.IsNullOrEmpty(currentProject.FilePath))
                    {
                        saveFileDialog.InitialDirectory = Path.GetDirectoryName(currentProject.FilePath);
                        saveFileDialog.FileName = Path.GetFileName(currentProject.FilePath);
                    }

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // プロジェクトを指定されたパスに保存
                        _projectService.SaveProjectAs(saveFileDialog.FileName);
                        UpdateStatusBar($"プロジェクトを保存しました: {saveFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("プロジェクト保存中にエラーが発生しました", ex);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 変更されたプロジェクトがある場合は保存確認
            if (_projectService.IsProjectDirty() && ConfirmSaveChanges())
            {
                return; // キャンセルされた場合は処理を中断
            }

            // アプリケーション終了
            Close();
        }

        #endregion

        #region 波形メニューイベントハンドラ

        private void newChuckSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 新規Chuck波形シーケンス作成
                // 現在のプロジェクト内の新しいシーケンス番号を取得
                var currentProject = _projectService.GetCurrentProject();
                int newNumber = currentProject.ChuckWaveformSequences.Count > 0
                    ? currentProject.ChuckWaveformSequences.Max(s => s.Number) + 1
                    : 1;

                var sequence = new ChuckWaveformSequence(SequenceType.Chuck, newNumber, $"Chuck_{newNumber}");

                // プロジェクトに追加
                currentProject.ChuckWaveformSequences.Add(sequence);

                // TODO: シーケンスの編集ダイアログ表示処理

                // プロジェクトの変更フラグを設定
                currentProject.IsDirty = true;

                UpdateStatusBar($"新規Chuck波形シーケンスを作成しました: {sequence.Name}");
            }
            catch (Exception ex)
            {
                ShowError("Chuck波形シーケンス作成中にエラーが発生しました", ex);
            }
        }

        private void newDeChuckSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 新規DeChuck波形シーケンス作成
                // 現在のプロジェクト内の新しいシーケンス番号を取得
                var currentProject = _projectService.GetCurrentProject();
                int newNumber = currentProject.DeChuckWaveformSequences.Count > 0
                    ? currentProject.DeChuckWaveformSequences.Max(s => s.Number) + 1
                    : 1;

                var sequence = new DeChuckWaveformSequence(SequenceType.DeChuck, newNumber, $"DeChuck_{newNumber}");

                // プロジェクトに追加
                currentProject.DeChuckWaveformSequences.Add(sequence);

                // TODO: シーケンスの編集ダイアログ表示処理

                // プロジェクトの変更フラグを設定
                currentProject.IsDirty = true;

                UpdateStatusBar($"新規DeChuck波形シーケンスを作成しました: {sequence.Name}");
            }
            catch (Exception ex)
            {
                ShowError("DeChuck波形シーケンス作成中にエラーが発生しました", ex);
            }
        }

        private void exportSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: 選択された波形のエクスポート処理
                UpdateStatusBar("波形シーケンスをエクスポートしました");
            }
            catch (Exception ex)
            {
                ShowError("波形シーケンスのエクスポート中にエラーが発生しました", ex);
            }
        }

        private void importSequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "波形シーケンスファイル (*.wseq)|*.wseq|すべてのファイル (*.*)|*.*";
                    openFileDialog.Title = "波形シーケンスのインポート";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // TODO: 波形シーケンスのインポート処理

                        // プロジェクトの変更フラグを設定
                        _projectService.GetCurrentProject().IsDirty = true;

                        UpdateStatusBar($"波形シーケンスをインポートしました: {openFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("波形シーケンスのインポート中にエラーが発生しました", ex);
            }
        }

        #endregion

        #region ヘルプメニューイベントハンドラ

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "波形サンプルアプリケーション\nバージョン 1.0.0\n© 2025 Company Name",
                "バージョン情報",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #endregion

        #region ユーティリティメソッド

        private void UpdateStatusBar(string message)
        {
            statusLabel.Text = message;
        }

        private void ShowError(string message, Exception ex)
        {
            MessageBox.Show(
                $"{message}\n\n詳細: {ex.Message}",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            UpdateStatusBar($"エラー: {message}");
        }

        /// <summary>
        /// 変更されたプロジェクトを保存するかどうか確認するダイアログを表示
        /// </summary>
        /// <returns>キャンセルした場合はtrue、それ以外はfalse</returns>
        private bool ConfirmSaveChanges()
        {
            var result = MessageBox.Show(
                "変更を保存しますか？",
                "確認",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 保存処理
                if (_projectService.IsProjectSaved())
                {
                    // 既に保存されている場合は上書き保存
                    return !_projectService.SaveProject();
                }
                else
                {
                    // まだ保存されていない場合は名前をつけて保存
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "波形プロジェクトファイル (*.wproj)|*.wproj|すべてのファイル (*.*)|*.*";
                        saveFileDialog.Title = "名前を付けてプロジェクトを保存";
                        saveFileDialog.DefaultExt = "wproj";
                        saveFileDialog.AddExtension = true;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            _projectService.SaveProjectAs(saveFileDialog.FileName);
                            return false;
                        }
                        else
                        {
                            return true; // 保存ダイアログでキャンセルした場合
                        }
                    }
                }
            }
            else if (result == DialogResult.Cancel)
            {
                return true; // キャンセルした場合
            }

            return false; // 保存しない場合
        }

        /// <summary>
        /// 現在のプロジェクトでUIを更新する
        /// </summary>
        private void UpdateUIWithCurrentProject()
        {
            // TODO: プロジェクトの内容に基づいて、UIの各コンポーネントを更新する
            // 例えば、プロジェクト内の波形シーケンスをリストに表示するなど

            // 現在のプロジェクトを取得
            var currentProject = _projectService.GetCurrentProject();

            // フォームのタイトルを更新
            if (currentProject != null)
            {
                string projectName = string.IsNullOrEmpty(currentProject.Name) ? "無題" : currentProject.Name;
                Text = $"波形サンプルアプリケーション - {projectName}";
            }
            else
            {
                Text = "波形サンプルアプリケーション";
            }
        }

        #endregion

        /// <summary>
        /// 選択された波形シーケンスをエクスポートする
        /// </summary>
        /// <param name="sequence">エクスポートするシーケンス</param>
        private void ExportSequence(IWaveformSequence sequence)
        {
            if (sequence == null)
            {
                ShowError("エクスポートエラー", new InvalidOperationException("エクスポートするシーケンスが選択されていません。"));
                return;
            }

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    string defaultFileName = $"{sequence.SequenceType}_{sequence.Number}_{sequence.Name}.wseq";

                    saveFileDialog.FileName = defaultFileName;
                    saveFileDialog.Filter = "波形シーケンスファイル (*.wseq)|*.wseq|すべてのファイル (*.*)|*.*";
                    saveFileDialog.Title = "波形シーケンスをエクスポート";
                    saveFileDialog.DefaultExt = "wseq";
                    saveFileDialog.AddExtension = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _projectService.SaveSequenceAs(sequence, saveFileDialog.FileName);
                        UpdateStatusBar($"波形シーケンスをエクスポートしました: {saveFileDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("波形シーケンスのエクスポート中にエラーが発生しました", ex);
            }
        }

        /// <summary>
        /// 波形シーケンスをインポートする
        /// </summary>
        private void ImportSequence()
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "波形シーケンスファイル (*.wseq)|*.wseq|すべてのファイル (*.*)|*.*";
                    openFileDialog.Title = "波形シーケンスのインポート";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // シーケンスを読み込む
                        IWaveformSequence sequence = _projectService.ImportSequence(openFileDialog.FileName);

                        // シーケンスの番号を設定
                        var currentProject = _projectService.GetCurrentProject();
                        int newNumber;

                        if (sequence.SequenceType == SequenceType.Chuck)
                        {
                            newNumber = currentProject.ChuckWaveformSequences.Count > 0
                                ? currentProject.ChuckWaveformSequences.Max(s => s.Number) + 1
                                : 1;

                            // シーケンスが既にChuckWaveformSequenceであることを確認
                            ChuckWaveformSequence chuckSequence = sequence as ChuckWaveformSequence;
                            if (chuckSequence == null)
                            {
                                // キャストできない場合は新しいインスタンスを作成
                                chuckSequence = new ChuckWaveformSequence(
                                    SequenceType.Chuck,
                                    newNumber,
                                    sequence.Name);

                                // プロパティをコピー
                                chuckSequence.SampleRate = sequence.SampleRate;
                                chuckSequence.WaveformSteps = sequence.WaveformSteps;
                            }
                            else
                            {
                                // 既存のシーケンスの番号を更新
                                // 注: Number が読み取り専用の場合は、新しいインスタンスを作成する必要があります
                                chuckSequence = new ChuckWaveformSequence(
                                    SequenceType.Chuck,
                                    newNumber,
                                    chuckSequence.Name);

                                chuckSequence.SampleRate = sequence.SampleRate;
                                chuckSequence.WaveformSteps = sequence.WaveformSteps;
                            }

                            sequence = chuckSequence;
                        }
                        else if (sequence.SequenceType == SequenceType.DeChuck)
                        {
                            newNumber = currentProject.DeChuckWaveformSequences.Count > 0
                                ? currentProject.DeChuckWaveformSequences.Max(s => s.Number) + 1
                                : 1;

                            // シーケンスが既にDeChuckWaveformSequenceであることを確認
                            DeChuckWaveformSequence deChuckSequence = sequence as DeChuckWaveformSequence;
                            if (deChuckSequence == null)
                            {
                                // キャストできない場合は新しいインスタンスを作成
                                deChuckSequence = new DeChuckWaveformSequence(
                                    SequenceType.DeChuck,
                                    newNumber,
                                    sequence.Name);

                                // プロパティをコピー
                                deChuckSequence.SampleRate = sequence.SampleRate;
                                deChuckSequence.WaveformSteps = sequence.WaveformSteps;
                            }
                            else
                            {
                                // 既存のシーケンスの番号を更新
                                // 注: Number が読み取り専用の場合は、新しいインスタンスを作成する必要があります
                                deChuckSequence = new DeChuckWaveformSequence(
                                    SequenceType.DeChuck,
                                    newNumber,
                                    deChuckSequence.Name);

                                deChuckSequence.SampleRate = sequence.SampleRate;
                                deChuckSequence.WaveformSteps = sequence.WaveformSteps;
                            }

                            sequence = deChuckSequence;
                        }

                        // 現在のプロジェクトに追加
                        bool success = _projectService.AddSequenceToProject(sequence);

                        if (success)
                        {
                            // プロジェクトの変更フラグは AddSequenceToProject 内で設定済み
                            UpdateStatusBar($"波形シーケンスをインポートしました: {sequence.Name}");

                            // UIを更新
                            UpdateUIWithCurrentProject();
                        }
                        else
                        {
                            ShowError("波形シーケンスのインポートに失敗しました", new Exception("シーケンスの最大数に達しているか、無効なシーケンスです。"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("波形シーケンスのインポート中にエラーが発生しました", ex);
            }
        }
    }
}
