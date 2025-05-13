namespace WaveformSample
{
    partial class FormEditWaveform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ucGrid1 = new WaveformSample.UserControls.UcGrid();
            ucChart1 = new WaveformSample.UserControls.UcChart();
            txtlblTitleWaveformSequenceName = new TextBox();
            lblTitleWaveformSequenceName = new Label();
            btnImport = new Button();
            btnCancel = new Button();
            btnApply = new Button();
            nudSampleRate = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)nudSampleRate).BeginInit();
            SuspendLayout();
            // 
            // ucGrid1
            // 
            ucGrid1.Location = new Point(12, 415);
            ucGrid1.Name = "ucGrid1";
            ucGrid1.Size = new Size(953, 174);
            ucGrid1.TabIndex = 0;
            ucGrid1.WaveformSteps = null;
            // 
            // ucChart1
            // 
            ucChart1.Location = new Point(12, 41);
            ucChart1.Name = "ucChart1";
            ucChart1.Size = new Size(953, 368);
            ucChart1.TabIndex = 8;
            // 
            // txtlblTitleWaveformSequenceName
            // 
            txtlblTitleWaveformSequenceName.Location = new Point(118, 12);
            txtlblTitleWaveformSequenceName.Name = "txtlblTitleWaveformSequenceName";
            txtlblTitleWaveformSequenceName.Size = new Size(406, 23);
            txtlblTitleWaveformSequenceName.TabIndex = 2;
            // 
            // lblTitleWaveformSequenceName
            // 
            lblTitleWaveformSequenceName.AutoSize = true;
            lblTitleWaveformSequenceName.Location = new Point(17, 15);
            lblTitleWaveformSequenceName.Name = "lblTitleWaveformSequenceName";
            lblTitleWaveformSequenceName.Size = new Size(95, 15);
            lblTitleWaveformSequenceName.TabIndex = 3;
            lblTitleWaveformSequenceName.Text = "Waveform Name";
            // 
            // btnImport
            // 
            btnImport.Location = new Point(732, 10);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(91, 23);
            btnImport.TabIndex = 4;
            btnImport.Text = "import";
            btnImport.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(787, 595);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(86, 28);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            btnApply.Location = new Point(879, 595);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(86, 28);
            btnApply.TabIndex = 6;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            // 
            // nudSampleRate
            // 
            nudSampleRate.Location = new Point(557, 12);
            nudSampleRate.Name = "nudSampleRate";
            nudSampleRate.Size = new Size(108, 23);
            nudSampleRate.TabIndex = 7;
            // 
            // FormEditWaveform
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(977, 625);
            Controls.Add(nudSampleRate);
            Controls.Add(btnApply);
            Controls.Add(btnCancel);
            Controls.Add(btnImport);
            Controls.Add(lblTitleWaveformSequenceName);
            Controls.Add(txtlblTitleWaveformSequenceName);
            Controls.Add(ucChart1);
            Controls.Add(ucGrid1);
            Name = "FormEditWaveform";
            Text = "FormEditWaveform";
            ((System.ComponentModel.ISupportInitialize)nudSampleRate).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private UserControls.UcGrid ucGrid1;
        private UserControls.UcChart ucChart1;
        private TextBox txtlblTitleWaveformSequenceName;
        private Label lblTitleWaveformSequenceName;
        private Button btnImport;
        private Button btnCancel;
        private Button btnApply;
        private NumericUpDown nudSampleRate;
    }
}