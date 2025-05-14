using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveformSample.Charts;
using WaveformSample.Core;
using WaveformSample.Waveforms;

namespace WaveformSample
{
    public partial class FormEditWaveform : Form
    {
        // 編集中のシーケンス
        private IWaveformSequence _currentSequence;

        // プロジェクトコンテキスト
        private ProjectContext _projectContext;

        // 元のシーケンスの状態（キャンセル時に戻すため）
        private List<WaveformStep> _originalSteps;
        private string _originalName;
        private int _originalPitch;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormEditWaveform()
        {
            InitializeComponent();

            // イベントハンドラを設定
            this.Load += FormEditWaveform_Load;
            this.btnCancel.Click += BtnCancel_Click;
            this.btnApply.Click += BtnApply_Click;
            this.btnImport.Click += BtnImport_Click;
            this.txtlblTitleWaveformSequenceName.TextChanged += TxtlblTitleWaveformSequenceName_TextChanged;
            this.nudSampleRate.ValueChanged += NudSampleRate_ValueChanged;

            // グリッド編集後にチャートを更新するイベントハンドラを設定
            this.ucGrid1.WaveformStepsChanged += UcGrid1_WaveformStepsChanged;
        }

        /// <summary>
        /// シーケンスとプロジェクトコンテキストを設定する
        /// </summary>
        /// <param name="sequence">編集するシーケンス</param>
        /// <param name="projectContext">プロジェクトコンテキスト</param>
        public void SetSequence(IWaveformSequence sequence, ProjectContext projectContext)
        {
            _currentSequence = sequence;
            _projectContext = projectContext;

            // 元の状態を保存
            _originalName = sequence.Name;
            _originalPitch = sequence.Pitch;
            _originalSteps = new List<WaveformStep>(sequence.WaveformSteps.Select(s => s.Clone() as WaveformStep));

            // UIに反映
            UpdateUI();
        }

        /// <summary>
        /// UI要素にシーケンスの情報を反映する
        /// </summary>
        private void UpdateUI()
        {
            if (_currentSequence == null)
                return;

            // テキストボックスとNumericUpDownにシーケンス情報を設定
            txtlblTitleWaveformSequenceName.Text = _currentSequence.Name;
            nudSampleRate.Value = _currentSequence.Pitch;

            // グリッドにWaveformStepsを設定
            ucGrid1.WaveformSteps = _currentSequence.WaveformSteps;

            // チャートにWaveformStepsを設定して波形を表示
            UpdateChart();
        }

        /// <summary>
        /// チャートを更新
        /// </summary>
        private void UpdateChart()
        {
            // チャートコントロールに波形シーケンスデータを設定
            ucChart1.SetWaveformData(_currentSequence);
        }

        #region イベントハンドラ

        private void FormEditWaveform_Load(object sender, EventArgs e)
        {
            // フォーム読み込み時の処理
            UpdateUI();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // 変更をキャンセルして元の状態に戻す
            if (_currentSequence != null)
            {
                _currentSequence.Name = _originalName;
                _currentSequence.Pitch = _originalPitch;
                _currentSequence.WaveformSteps = new List<WaveformStep>(_originalSteps.Select(s => s.Clone() as WaveformStep));
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            // 変更を適用してプロジェクトに保存
            if (_currentSequence != null && _projectContext != null)
            {
                try
                {
                    // シーケンスの状態はすでに更新されているので、プロジェクトを保存
                    _projectContext.UpdateAndSaveSequence(_currentSequence);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"波形シーケンスの保存中にエラーが発生しました: {ex.Message}",
                                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // 波形ファイルのインポート機能
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Waveform files (*.wseq)|*.wseq|All files (*.*)|*.*";
                openFileDialog.Title = "インポートする波形シーケンスを選択";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 選択されたファイルからシーケンスを読み込む
                        var importedSequence = _projectContext.LoadSequence<IWaveformSequence>(openFileDialog.FileName);

                        // 読み込んだシーケンスのWaveformStepsを現在のシーケンスにコピー
                        if (importedSequence != null && importedSequence.WaveformSteps != null)
                        {
                            // 現在のシーケンスにインポートしたステップをコピー
                            _currentSequence.WaveformSteps = new List<WaveformStep>(
                                importedSequence.WaveformSteps.Select(s => s.Clone() as WaveformStep));

                            // UIを更新
                            UpdateUI();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"波形シーケンスのインポート中にエラーが発生しました: {ex.Message}",
                                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void TxtlblTitleWaveformSequenceName_TextChanged(object sender, EventArgs e)
        {
            // シーケンス名を変更
            if (_currentSequence != null)
            {
                _currentSequence.Name = txtlblTitleWaveformSequenceName.Text;
            }
        }

        private void NudSampleRate_ValueChanged(object sender, EventArgs e)
        {
            // ピッチ値を変更
            if (_currentSequence != null)
            {
                _currentSequence.Pitch = (int)nudSampleRate.Value;
                // チャートを更新
                UpdateChart();
            }
        }

        private void UcGrid1_WaveformStepsChanged(object sender, EventArgs e)
        {
            // グリッドでWaveformStepsが変更されたときの処理
            if (_currentSequence != null)
            {
                // チャートを更新
                UpdateChart();
            }
        }

        #endregion

        private void FormEditWaveform_FormClosing(object sender, FormClosingEventArgs e)
        {
            // モーダレスフォームとしてDialogResultがセットされていない場合（手動での閉じる操作）
            if (this.DialogResult == DialogResult.None && IsDataModified())
            {
                // 変更があれば確認ダイアログを表示
                var result = MessageBox.Show("変更を保存しますか？", "確認",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 変更を保存
                    if (_currentSequence != null && _projectContext != null)
                    {
                        try
                        {
                            _projectContext.UpdateAndSaveSequence(_currentSequence);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"波形シーケンスの保存中にエラーが発生しました: {ex.Message}",
                                           "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true; // フォームを閉じない
                            return;
                        }
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // フォームを閉じない
                }
                else
                {
                    // 変更を破棄（元に戻す）
                    if (_currentSequence != null)
                    {
                        _currentSequence.Name = _originalName;
                        _currentSequence.Pitch = _originalPitch;
                        _currentSequence.WaveformSteps = new List<WaveformStep>(_originalSteps.Select(s => s.Clone() as WaveformStep));
                    }
                }
            }
        }

        // データが変更されたかどうかを確認
        private bool IsDataModified()
        {
            if (_currentSequence == null)
                return false;

            // 名前またはピッチが変更されたか確認
            if (_currentSequence.Name != _originalName || _currentSequence.Pitch != _originalPitch)
                return true;

            // WaveformStepsの数が変更されたか確認
            if (_currentSequence.WaveformSteps.Count != _originalSteps.Count)
                return true;

            // WaveformStepsの内容が変更されたか確認（簡易比較）
            // 本来は各プロパティの詳細な比較が必要
            for (int i = 0; i < _currentSequence.WaveformSteps.Count; i++)
            {
                var current = _currentSequence.WaveformSteps[i];
                var original = _originalSteps[i];

                // 主要なプロパティを比較（実際の実装では全プロパティを比較する必要があります）
                if (current.Duration != original.Duration ||
                    current.StartFrequency != original.StartFrequency ||
                    current.EndFrequency != original.EndFrequency ||
                    current.StartAmplitude != original.StartAmplitude ||
                    current.EndAmplitude != original.EndAmplitude ||
                    current.IsFrequencySweep != original.IsFrequencySweep)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
