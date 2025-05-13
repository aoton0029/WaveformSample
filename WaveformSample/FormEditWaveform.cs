using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveformSample.Charts;
using WaveformSample.Waveforms;

namespace WaveformSample
{
    public partial class FormEditWaveform : Form
    {
        private IWaveformSequence _waveformSequence;
        private IChartRenderer _chartRenderer;
        private bool _isModified = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormEditWaveform()
        {
            InitializeComponent();
            InitializeEvents();
        }

        /// <summary>
        /// 波形シーケンスとチャートレンダラーを指定して初期化するコンストラクタ
        /// </summary>
        /// <param name="waveformSequence">編集対象の波形シーケンス</param>
        /// <param name="chartRenderer">チャートのレンダラー</param>
        public FormEditWaveform(IWaveformSequence waveformSequence, IChartRenderer chartRenderer)
        {
            InitializeComponent();

            _waveformSequence = waveformSequence;
            _chartRenderer = chartRenderer;

            InitializeEvents();
            LoadWaveformSequence();
        }

        /// <summary>
        /// イベントハンドラを初期化
        /// </summary>
        private void InitializeEvents()
        {
            // ボタンイベントの登録
            btnApply.Click += BtnApply_Click;
            btnCancel.Click += BtnCancel_Click;
            btnImport.Click += BtnImport_Click;

            // テキストボックスのイベント
            txtlblTitleWaveformSequenceName.TextChanged += Control_ValueChanged;
            nudSampleRate.ValueChanged += Control_ValueChanged;

            // UcGridのイベント
            ucGrid1.WaveformStepChanged += UcGrid1_WaveformStepChanged;

            // フォームのイベント
            FormClosing += FormEditWaveform_FormClosing;
        }

        /// <summary>
        /// 波形シーケンスデータをフォームにロード
        /// </summary>
        private void LoadWaveformSequence()
        {
            if (_waveformSequence == null)
                return;

            // UIコントロールにデータをロード
            txtlblTitleWaveformSequenceName.Text = _waveformSequence.Name;
            nudSampleRate.Value = _waveformSequence.SampleRate;

            // グリッドにWaveformStepsをセット
            ucGrid1.WaveformSteps = new ObservableCollection<WaveformStep>(_waveformSequence.WaveformSteps);

            // チャートの設定
            ucChart1.Renderer = _chartRenderer;
            ucChart1.Sequence = _waveformSequence;
            ucChart1.ChartTitle = "波形プレビュー";
            ucChart1.XAxisLabel = "時間 (秒)";
            ucChart1.YAxisLabel = "振幅";

            _isModified = false;
        }

        /// <summary>
        /// UIからシーケンスデータを更新
        /// </summary>
        private void UpdateWaveformSequence()
        {
            if (_waveformSequence == null)
                return;

            _waveformSequence.Name = txtlblTitleWaveformSequenceName.Text;
            _waveformSequence.SampleRate = (int)nudSampleRate.Value;

            // WaveformStepsは既にucGrid1を通じて更新されている

            // チャートを更新
            ucChart1.Sequence = _waveformSequence;
        }

        #region イベントハンドラ

        /// <summary>
        /// コントロール値変更時のイベント
        /// </summary>
        private void Control_ValueChanged(object sender, EventArgs e)
        {
            _isModified = true;

            // リアルタイムに波形シーケンスを更新してチャートに反映
            UpdateWaveformSequence();
        }

        /// <summary>
        /// グリッドのWaveformStep変更時のイベント
        /// </summary>
        private void UcGrid1_WaveformStepChanged(object sender, EventArgs e)
        {
            _isModified = true;

            // チャートの更新
            ucChart1.Sequence = _waveformSequence;
        }

        /// <summary>
        /// 適用ボタンクリック
        /// </summary>
        private void BtnApply_Click(object sender, EventArgs e)
        {
            UpdateWaveformSequence();
            _isModified = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// インポートボタンクリック
        /// </summary>
        private void BtnImport_Click(object sender, EventArgs e)
        {
            // 波形データのインポート処理を実装
            // 例: ファイル選択ダイアログを表示してCSVなどから読み込む
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*";
                openFileDialog.Title = "波形データをインポート";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // ファイルからデータを読み込む処理
                        // ImportWaveformData(openFileDialog.FileName);
                        _isModified = true;

                        // チャートを更新
                        ucChart1.Sequence = _waveformSequence;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"インポート中にエラーが発生しました: {ex.Message}", "エラー",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// フォームを閉じる前の確認
        /// </summary>
        private void FormEditWaveform_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 変更があれば保存確認
            if (_isModified && DialogResult != DialogResult.OK)
            {
                var result = MessageBox.Show("変更を保存しますか？", "確認",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateWaveformSequence();
                    DialogResult = DialogResult.OK;
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion
    }
}
