namespace WaveformSample.UserControls
{
    partial class UcMain
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
            lblTitleProjectName = new Label();
            txtProjectName = new TextBox();
            rdbChuck = new RadioButton();
            rdbDeChuck = new RadioButton();
            ucWaveformSetting1 = new UcWaveformSetting();
            SuspendLayout();
            // 
            // lblTitleProjectName
            // 
            lblTitleProjectName.AutoSize = true;
            lblTitleProjectName.Location = new Point(12, 10);
            lblTitleProjectName.Name = "lblTitleProjectName";
            lblTitleProjectName.Size = new Size(75, 15);
            lblTitleProjectName.TabIndex = 0;
            lblTitleProjectName.Text = "ProjectName";
            // 
            // txtProjectName
            // 
            txtProjectName.Location = new Point(93, 7);
            txtProjectName.Name = "txtProjectName";
            txtProjectName.Size = new Size(350, 23);
            txtProjectName.TabIndex = 1;
            // 
            // rdbChuck
            // 
            rdbChuck.Appearance = Appearance.Button;
            rdbChuck.Checked = true;
            rdbChuck.Font = new Font("メイリオ", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            rdbChuck.Location = new Point(3, 36);
            rdbChuck.Name = "rdbChuck";
            rdbChuck.Size = new Size(124, 53);
            rdbChuck.TabIndex = 2;
            rdbChuck.TabStop = true;
            rdbChuck.Text = "Chuck";
            rdbChuck.TextAlign = ContentAlignment.MiddleCenter;
            rdbChuck.UseVisualStyleBackColor = true;
            // 
            // rdbDeChuck
            // 
            rdbDeChuck.Appearance = Appearance.Button;
            rdbDeChuck.Font = new Font("メイリオ", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            rdbDeChuck.Location = new Point(133, 36);
            rdbDeChuck.Name = "rdbDeChuck";
            rdbDeChuck.Size = new Size(124, 53);
            rdbDeChuck.TabIndex = 3;
            rdbDeChuck.Text = "DeChuck";
            rdbDeChuck.TextAlign = ContentAlignment.MiddleCenter;
            rdbDeChuck.UseVisualStyleBackColor = true;
            // 
            // ucWaveformSetting1
            // 
            ucWaveformSetting1.Location = new Point(3, 95);
            ucWaveformSetting1.Name = "ucWaveformSetting1";
            ucWaveformSetting1.Size = new Size(978, 533);
            ucWaveformSetting1.TabIndex = 4;
            // 
            // UcMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ucWaveformSetting1);
            Controls.Add(rdbDeChuck);
            Controls.Add(rdbChuck);
            Controls.Add(txtProjectName);
            Controls.Add(lblTitleProjectName);
            Name = "UcMain";
            Size = new Size(984, 631);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitleProjectName;
        private TextBox txtProjectName;
        private RadioButton rdbChuck;
        private RadioButton rdbDeChuck;
        private UcWaveformSetting ucWaveformSetting1;
    }
}
