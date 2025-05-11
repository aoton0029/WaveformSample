namespace WaveformSample.UserControls
{
    partial class UcGrid
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            ColumnWaveformType = new DataGridViewComboBoxColumn();
            ColumnStepTime = new DataGridViewTextBoxColumn();
            ColumnIsFrequencySweep = new DataGridViewCheckBoxColumn();
            ColumnStartFrequency = new DataGridViewTextBoxColumn();
            ColumnEndFrequency = new DataGridViewTextBoxColumn();
            ColumnIsAmplitudeSweep = new DataGridViewCheckBoxColumn();
            ColumnStartAmplitude = new DataGridViewTextBoxColumn();
            ColumnEndAmplitude = new DataGridViewTextBoxColumn();
            ColumnPhase = new DataGridViewTextBoxColumn();
            ColumnIsDCOffsetSweep = new DataGridViewCheckBoxColumn();
            ColumnStartDCOffset = new DataGridViewTextBoxColumn();
            ColumnEndDCOffset = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnWaveformType, ColumnStepTime, ColumnIsFrequencySweep, ColumnStartFrequency, ColumnEndFrequency, ColumnIsAmplitudeSweep, ColumnStartAmplitude, ColumnEndAmplitude, ColumnPhase, ColumnIsDCOffsetSweep, ColumnStartDCOffset, ColumnEndDCOffset });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(690, 174);
            dataGridView1.TabIndex = 0;
            // 
            // ColumnWaveformType
            // 
            ColumnWaveformType.HeaderText = "WaveformType";
            ColumnWaveformType.Name = "ColumnWaveformType";
            // 
            // ColumnStepTime
            // 
            ColumnStepTime.HeaderText = "StepTime";
            ColumnStepTime.Name = "ColumnStepTime";
            ColumnStepTime.Resizable = DataGridViewTriState.True;
            ColumnStepTime.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnIsFrequencySweep
            // 
            ColumnIsFrequencySweep.HeaderText = "IsFrequencySweep";
            ColumnIsFrequencySweep.Name = "ColumnIsFrequencySweep";
            ColumnIsFrequencySweep.Resizable = DataGridViewTriState.True;
            ColumnIsFrequencySweep.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnStartFrequency
            // 
            ColumnStartFrequency.HeaderText = "StartFrequency";
            ColumnStartFrequency.Name = "ColumnStartFrequency";
            // 
            // ColumnEndFrequency
            // 
            ColumnEndFrequency.HeaderText = "EndFrequency";
            ColumnEndFrequency.Name = "ColumnEndFrequency";
            // 
            // ColumnIsAmplitudeSweep
            // 
            ColumnIsAmplitudeSweep.HeaderText = "IsAmplitudeSweep";
            ColumnIsAmplitudeSweep.Name = "ColumnIsAmplitudeSweep";
            // 
            // ColumnStartAmplitude
            // 
            ColumnStartAmplitude.HeaderText = "StartAmplitude";
            ColumnStartAmplitude.Name = "ColumnStartAmplitude";
            // 
            // ColumnEndAmplitude
            // 
            ColumnEndAmplitude.HeaderText = "EndAmplitude";
            ColumnEndAmplitude.Name = "ColumnEndAmplitude";
            // 
            // ColumnPhase
            // 
            ColumnPhase.HeaderText = "Phase(deg)";
            ColumnPhase.Name = "ColumnPhase";
            // 
            // ColumnIsDCOffsetSweep
            // 
            ColumnIsDCOffsetSweep.HeaderText = "IsDCOffsetSweep";
            ColumnIsDCOffsetSweep.Name = "ColumnIsDCOffsetSweep";
            // 
            // ColumnStartDCOffset
            // 
            ColumnStartDCOffset.HeaderText = "StartDCOffset";
            ColumnStartDCOffset.Name = "ColumnStartDCOffset";
            // 
            // ColumnEndDCOffset
            // 
            ColumnEndDCOffset.HeaderText = "EndDCOffset";
            ColumnEndDCOffset.Name = "ColumnEndDCOffset";
            // 
            // UcGrid
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dataGridView1);
            Name = "UcGrid";
            Size = new Size(690, 174);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewComboBoxColumn ColumnWaveformType;
        private DataGridViewTextBoxColumn ColumnStepTime;
        private DataGridViewCheckBoxColumn ColumnIsFrequencySweep;
        private DataGridViewTextBoxColumn ColumnStartFrequency;
        private DataGridViewTextBoxColumn ColumnEndFrequency;
        private DataGridViewCheckBoxColumn ColumnIsAmplitudeSweep;
        private DataGridViewTextBoxColumn ColumnStartAmplitude;
        private DataGridViewTextBoxColumn ColumnEndAmplitude;
        private DataGridViewTextBoxColumn ColumnPhase;
        private DataGridViewCheckBoxColumn ColumnIsDCOffsetSweep;
        private DataGridViewTextBoxColumn ColumnStartDCOffset;
        private DataGridViewTextBoxColumn ColumnEndDCOffset;
    }
}
