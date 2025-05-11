namespace WaveformSample.UserControls
{
    partial class UcWaveformSetting
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
            ucChart1 = new UcChart();
            ColumnWaveformSequenceName = new DataGridViewTextBoxColumn();
            ColumnEdit = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnWaveformSequenceName, ColumnEdit });
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(262, 489);
            dataGridView1.TabIndex = 0;
            // 
            // ucChart1
            // 
            ucChart1.ChartTitle = null;
            ucChart1.Location = new Point(271, 3);
            ucChart1.Name = "ucChart1";
            ucChart1.Renderer = null;
            ucChart1.Sequence = null;
            ucChart1.ShowGridLines = true;
            ucChart1.Size = new Size(666, 599);
            ucChart1.TabIndex = 1;
            ucChart1.XAxisLabel = null;
            ucChart1.YAxisLabel = null;
            // 
            // ColumnWaveformSequenceName
            // 
            ColumnWaveformSequenceName.DataPropertyName = "Name";
            ColumnWaveformSequenceName.HeaderText = "Name";
            ColumnWaveformSequenceName.Name = "ColumnWaveformSequenceName";
            ColumnWaveformSequenceName.ReadOnly = true;
            // 
            // ColumnEdit
            // 
            ColumnEdit.HeaderText = "";
            ColumnEdit.Name = "ColumnEdit";
            ColumnEdit.ReadOnly = true;
            ColumnEdit.Text = "Edit";
            ColumnEdit.UseColumnTextForButtonValue = true;
            // 
            // UcWaveformSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ucChart1);
            Controls.Add(dataGridView1);
            Name = "UcWaveformSetting";
            Size = new Size(940, 605);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private UcChart ucChart1;
        private DataGridViewTextBoxColumn ColumnWaveformSequenceName;
        private DataGridViewButtonColumn ColumnEdit;
    }
}
