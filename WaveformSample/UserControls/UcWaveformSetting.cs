using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaveformSample.Charts;
using WaveformSample.Waveforms;

namespace WaveformSample.UserControls
{
    public partial class UcWaveformSetting : UserControl
    {
        private List<IWaveformSequence> _waveformSequences;
        private IChartRenderer _chartRenderer;

        /// <summary>
        /// 波形シーケンスのリスト
        /// </summary>
        [Browsable(false)]
        public List<IWaveformSequence> WaveformSequences
        {
            get => _waveformSequences;
            set
            {
                _waveformSequences = value;
                RefreshDataGridView();
            }
        }

        /// <summary>
        /// チャートのレンダラー
        /// </summary>
        [Browsable(false)]
        public IChartRenderer ChartRenderer
        {
            get => _chartRenderer;
            set
            {
                _chartRenderer = value;
                ucChart1.Renderer = _chartRenderer;
            }
        }

        /// <summary>
        /// シーケンス編集時のイベント
        /// </summary>
        public event EventHandler<IWaveformSequence> SequenceEditRequested;


        public UcWaveformSetting()
        {
            InitializeComponent();

            // データグリッドビューの設定
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // イベントハンドラの設定
            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

            // チャートの初期設定
            ucChart1.ChartTitle = "波形プレビュー";
            ucChart1.XAxisLabel = "時間 (秒)";
            ucChart1.YAxisLabel = "振幅";
            ucChart1.ShowGridLines = true;
        }

        /// <summary>
        /// データグリッドビューを更新する
        /// </summary>
        private void RefreshDataGridView()
        {
            // データグリッドビューをクリア
            dataGridView1.Rows.Clear();

            if (_waveformSequences == null || _waveformSequences.Count == 0)
                return;

            // データグリッドビューにデータを追加
            foreach (var sequence in _waveformSequences)
            {
                int rowIndex = dataGridView1.Rows.Add();
                var row = dataGridView1.Rows[rowIndex];

                // 名前列を設定
                row.Cells[ColumnWaveformSequenceName.Index].Value = sequence.Name;

                // 行にシーケンスの参照を保存
                row.Tag = sequence;
            }

            // 最初の行を選択
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
                // 選択された行のシーケンスをチャートに表示
                UpdateChartWithSelectedSequence();
            }
        }

        /// <summary>
        /// セルクリック時の処理
        /// </summary>
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 「編集」ボタン列がクリックされた場合
            if (e.RowIndex >= 0 && e.ColumnIndex == ColumnEdit.Index)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (row.Tag is IWaveformSequence sequence)
                {
                    // 編集イベントを発生
                    OnSequenceEditRequested(sequence);
                }
            }
        }

        /// <summary>
        /// 行選択変更時の処理
        /// </summary>
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateChartWithSelectedSequence();
        }

        /// <summary>
        /// 選択された行のシーケンスをチャートに表示
        /// </summary>
        private void UpdateChartWithSelectedSequence()
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;

            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow.Tag is IWaveformSequence sequence)
            {
                // チャートに表示
                ucChart1.Sequence = sequence;
            }
        }

        /// <summary>
        /// シーケンス編集要求イベントを発生
        /// </summary>
        protected virtual void OnSequenceEditRequested(IWaveformSequence sequence)
        {
            SequenceEditRequested?.Invoke(this, sequence);
        }

        /// <summary>
        /// 波形シーケンスを更新（編集後など）
        /// </summary>
        public void UpdateSequence(IWaveformSequence sequence)
        {
            if (_waveformSequences == null || sequence == null)
                return;

            // データグリッドビューの更新
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Tag is IWaveformSequence rowSequence && rowSequence == sequence)
                {
                    row.Cells[ColumnWaveformSequenceName.Index].Value = sequence.Name;

                    // 現在選択行なら、チャートも更新
                    if (row.Selected)
                    {
                        ucChart1.Sequence = sequence;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 波形シーケンスを追加
        /// </summary>
        public void AddSequence(IWaveformSequence sequence)
        {
            if (_waveformSequences == null || sequence == null)
                return;

            // リストに追加
            _waveformSequences.Add(sequence);

            // データグリッドビューに行を追加
            int rowIndex = dataGridView1.Rows.Add();
            var row = dataGridView1.Rows[rowIndex];
            row.Cells[ColumnWaveformSequenceName.Index].Value = sequence.Name;
            row.Tag = sequence;

            // 追加した行を選択
            dataGridView1.ClearSelection();
            row.Selected = true;
            UpdateChartWithSelectedSequence();
        }

        /// <summary>
        /// 波形シーケンスを削除
        /// </summary>
        public void RemoveSequence(IWaveformSequence sequence)
        {
            if (_waveformSequences == null || sequence == null)
                return;

            // リストから削除
            _waveformSequences.Remove(sequence);

            // データグリッドビューから行を削除
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                if (dataGridView1.Rows[i].Tag is IWaveformSequence rowSequence && rowSequence == sequence)
                {
                    dataGridView1.Rows.RemoveAt(i);
                    break;
                }
            }

            // 他の行を選択
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
                UpdateChartWithSelectedSequence();
            }
            else
            {
                // シーケンスがなくなった場合はチャートをクリア
                ucChart1.Sequence = null;
            }
        }
    }
}
