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
using WaveformSample.Waveforms;

namespace WaveformSample.UserControls
{
    public partial class UcGrid : UserControl
    {
        private ObservableCollection<WaveformStep> _waveformSteps;

        public UcGrid()
        {
            InitializeComponent();

            // DataGridViewのイベントハンドラを設定
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;
            dataGridView1.DataError += DataGridView1_DataError;

            // WaveformType列にEnum値を設定
            SetupWaveformTypeColumn();
        }

        private void SetupWaveformTypeColumn()
        {
            // WaveformTypeの列挙型の値をコンボボックスにセット
            // 注意: WaveformTypeのenum定義が見つからなかったため、実際の定義に合わせて変更してください
            if (ColumnWaveformType is DataGridViewComboBoxColumn comboColumn)
            {
                comboColumn.DataSource = Enum.GetValues(typeof(WaveformType));
            }
        }

        /// <summary>
        /// WaveformStepsコレクションをグリッドにバインドするプロパティ
        /// </summary>
        public ObservableCollection<WaveformStep> WaveformSteps
        {
            get => _waveformSteps;
            set
            {
                // 以前のコレクションのイベントハンドラを削除
                if (_waveformSteps != null)
                {
                    _waveformSteps.CollectionChanged -= WaveformSteps_CollectionChanged;
                }

                _waveformSteps = value;

                // 新しいコレクションにイベントハンドラを追加
                if (_waveformSteps != null)
                {
                    _waveformSteps.CollectionChanged += WaveformSteps_CollectionChanged;
                    RefreshDataGridView();
                }
            }
        }

        /// <summary>
        /// コレクションが変更された時の処理
        /// </summary>
        private void WaveformSteps_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshDataGridView();
        }

        /// <summary>
        /// データグリッドビューを更新
        /// </summary>
        private void RefreshDataGridView()
        {
            dataGridView1.Rows.Clear();

            if (_waveformSteps == null) return;

            // データグリッドビューにデータを反映
            foreach (var step in _waveformSteps)
            {
                int rowIndex = dataGridView1.Rows.Add();
                var row = dataGridView1.Rows[rowIndex];

                UpdateRowFromWaveformStep(row, step);
            }
        }

        /// <summary>
        /// 行のデータをWaveformStepから更新
        /// </summary>
        private void UpdateRowFromWaveformStep(DataGridViewRow row, WaveformStep step)
        {
            row.Cells[ColumnWaveformType.Index].Value = step.WaveType;
            row.Cells[ColumnStepTime.Index].Value = step.Duration;
            row.Cells[ColumnIsFrequencySweep.Index].Value = step.IsFrequencySweep;
            row.Cells[ColumnStartFrequency.Index].Value = step.StartFrequency;
            row.Cells[ColumnEndFrequency.Index].Value = step.EndFrequency;
            row.Cells[ColumnIsAmplitudeSweep.Index].Value = step.IsAmplitudeSweep;
            row.Cells[ColumnStartAmplitude.Index].Value = step.StartAmplitude;
            row.Cells[ColumnEndAmplitude.Index].Value = step.EndAmplitude;
            row.Cells[ColumnPhase.Index].Value = step.Phase;
            row.Cells[ColumnIsDCOffsetSweep.Index].Value = step.IsDCOffsetSweep;
            row.Cells[ColumnStartDCOffset.Index].Value = step.StartDCOffset;
            row.Cells[ColumnEndDCOffset.Index].Value = step.EndDCOffset;

            // 行にWaveformStepへの参照を保存
            row.Tag = step;
        }

        /// <summary>
        /// セルの表示をカスタマイズ
        /// </summary>
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count) return;

            var row = dataGridView1.Rows[e.RowIndex];
            if (row.Tag is not WaveformStep step) return;

            // 周波数スイープのセル書式設定
            if (e.ColumnIndex == ColumnStartFrequency.Index || e.ColumnIndex == ColumnEndFrequency.Index)
            {
                e.CellStyle.BackColor = step.IsFrequencySweep ? Color.White : Color.LightGray;
                e.CellStyle.ForeColor = step.IsFrequencySweep ? Color.Black : Color.DarkGray;
            }
            // 振幅スイープのセル書式設定
            else if (e.ColumnIndex == ColumnStartAmplitude.Index || e.ColumnIndex == ColumnEndAmplitude.Index)
            {
                e.CellStyle.BackColor = step.IsAmplitudeSweep ? Color.White : Color.LightGray;
                e.CellStyle.ForeColor = step.IsAmplitudeSweep ? Color.Black : Color.DarkGray;
            }
            // DCオフセットスイープのセル書式設定
            else if (e.ColumnIndex == ColumnStartDCOffset.Index || e.ColumnIndex == ColumnEndDCOffset.Index)
            {
                e.CellStyle.BackColor = step.IsDCOffsetSweep ? Color.White : Color.LightGray;
                e.CellStyle.ForeColor = step.IsDCOffsetSweep ? Color.Black : Color.DarkGray;
            }
        }

        /// <summary>
        /// セルの値が変更された時の処理
        /// </summary>
        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView1.Rows.Count) return;

            var row = dataGridView1.Rows[e.RowIndex];
            if (row.Tag is not WaveformStep step) return;

            // 変更された値をWaveformStepに適用
            if (e.ColumnIndex == ColumnWaveformType.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                step.WaveType = (WaveformType)row.Cells[e.ColumnIndex].Value;
            }
            else if (e.ColumnIndex == ColumnStepTime.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (int.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out int duration))
                {
                    step.Duration = duration;
                }
            }
            else if (e.ColumnIndex == ColumnIsFrequencySweep.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                step.IsFrequencySweep = (bool)row.Cells[e.ColumnIndex].Value;
                dataGridView1.InvalidateCell(ColumnStartFrequency.Index, e.RowIndex);
                dataGridView1.InvalidateCell(ColumnEndFrequency.Index, e.RowIndex);
            }
            else if (e.ColumnIndex == ColumnStartFrequency.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double frequency))
                {
                    step.StartFrequency = frequency;
                }
            }
            else if (e.ColumnIndex == ColumnEndFrequency.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double frequency))
                {
                    step.EndFrequency = frequency;
                }
            }
            else if (e.ColumnIndex == ColumnIsAmplitudeSweep.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                step.IsAmplitudeSweep = (bool)row.Cells[e.ColumnIndex].Value;
                dataGridView1.InvalidateCell(ColumnStartAmplitude.Index, e.RowIndex);
                dataGridView1.InvalidateCell(ColumnEndAmplitude.Index, e.RowIndex);
            }
            else if (e.ColumnIndex == ColumnStartAmplitude.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double amplitude))
                {
                    step.StartAmplitude = amplitude;
                }
            }
            else if (e.ColumnIndex == ColumnEndAmplitude.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double amplitude))
                {
                    step.EndAmplitude = amplitude;
                }
            }
            else if (e.ColumnIndex == ColumnPhase.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double phase))
                {
                    step.Phase = phase;
                }
            }
            else if (e.ColumnIndex == ColumnIsDCOffsetSweep.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                step.IsDCOffsetSweep = (bool)row.Cells[e.ColumnIndex].Value;
                dataGridView1.InvalidateCell(ColumnStartDCOffset.Index, e.RowIndex);
                dataGridView1.InvalidateCell(ColumnEndDCOffset.Index, e.RowIndex);
            }
            else if (e.ColumnIndex == ColumnStartDCOffset.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double offset))
                {
                    step.StartDCOffset = offset;
                }
            }
            else if (e.ColumnIndex == ColumnEndDCOffset.Index && row.Cells[e.ColumnIndex].Value != null)
            {
                if (double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out double offset))
                {
                    step.EndDCOffset = offset;
                }
            }
        }

        /// <summary>
        /// 編集コントロールの表示時の処理
        /// </summary>
        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell.RowIndex < 0) return;

            var row = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex];
            if (row.Tag is not WaveformStep step) return;

            // スイープフラグに基づいて編集可能性を制御
            if (dataGridView1.CurrentCell.ColumnIndex == ColumnStartFrequency.Index ||
                dataGridView1.CurrentCell.ColumnIndex == ColumnEndFrequency.Index)
            {
                dataGridView1.CurrentCell.ReadOnly = !step.IsFrequencySweep;
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == ColumnStartAmplitude.Index ||
                     dataGridView1.CurrentCell.ColumnIndex == ColumnEndAmplitude.Index)
            {
                dataGridView1.CurrentCell.ReadOnly = !step.IsAmplitudeSweep;
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == ColumnStartDCOffset.Index ||
                     dataGridView1.CurrentCell.ColumnIndex == ColumnEndDCOffset.Index)
            {
                dataGridView1.CurrentCell.ReadOnly = !step.IsDCOffsetSweep;
            }
        }

        /// <summary>
        /// データエラー処理
        /// </summary>
        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // データ型変換エラーなどを処理
            e.ThrowException = false;
            MessageBox.Show($"データエラーが発生しました: {e.Exception.Message}", "エラー",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 新しい行を追加
        /// </summary>
        public void AddRow()
        {
            if (_waveformSteps == null) return;

            // 新しいWaveformStepを作成して追加
            var newStep = new WaveformStep
            {
                Duration = 1,
                StartFrequency = 1000,
                EndFrequency = 1000,
                StartAmplitude = 1.0,
                EndAmplitude = 1.0,
                Phase = 0,
                StartDCOffset = 0,
                EndDCOffset = 0
            };

            _waveformSteps.Add(newStep);
        }

        /// <summary>
        /// 選択行を削除
        /// </summary>
        public void DeleteSelectedRow()
        {
            if (_waveformSteps == null || dataGridView1.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.Tag is WaveformStep step && _waveformSteps.Contains(step))
                {
                    _waveformSteps.Remove(step);
                }
            }
        }
    }
}
