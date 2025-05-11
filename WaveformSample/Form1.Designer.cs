namespace WaveformSample
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newProjectToolStripMenuItem = new ToolStripMenuItem();
            openProjectToolStripMenuItem = new ToolStripMenuItem();
            saveProjectToolStripMenuItem = new ToolStripMenuItem();
            saveProjectAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            waveformToolStripMenuItem = new ToolStripMenuItem();
            newChuckSequenceToolStripMenuItem = new ToolStripMenuItem();
            newDeChuckSequenceToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exportSequenceToolStripMenuItem = new ToolStripMenuItem();
            importSequenceToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, waveformToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1001, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newProjectToolStripMenuItem, openProjectToolStripMenuItem, saveProjectToolStripMenuItem, saveProjectAsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(67, 20);
            fileToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // newProjectToolStripMenuItem
            // 
            newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            newProjectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newProjectToolStripMenuItem.Size = new Size(248, 22);
            newProjectToolStripMenuItem.Text = "新規プロジェクト(&N)";
            newProjectToolStripMenuItem.Click += newProjectToolStripMenuItem_Click;
            // 
            // openProjectToolStripMenuItem
            // 
            openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            openProjectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openProjectToolStripMenuItem.Size = new Size(248, 22);
            openProjectToolStripMenuItem.Text = "プロジェクトを開く(&O)";
            openProjectToolStripMenuItem.Click += openProjectToolStripMenuItem_Click;
            // 
            // saveProjectToolStripMenuItem
            // 
            saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            saveProjectToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveProjectToolStripMenuItem.Size = new Size(248, 22);
            saveProjectToolStripMenuItem.Text = "プロジェクトの保存(&S)";
            saveProjectToolStripMenuItem.Click += saveProjectToolStripMenuItem_Click;
            // 
            // saveProjectAsToolStripMenuItem
            // 
            saveProjectAsToolStripMenuItem.Name = "saveProjectAsToolStripMenuItem";
            saveProjectAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveProjectAsToolStripMenuItem.Size = new Size(248, 22);
            saveProjectAsToolStripMenuItem.Text = "名前を付けて保存(&A)";
            saveProjectAsToolStripMenuItem.Click += saveProjectAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(245, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            exitToolStripMenuItem.Size = new Size(248, 22);
            exitToolStripMenuItem.Text = "終了(&X)";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // waveformToolStripMenuItem
            // 
            waveformToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newChuckSequenceToolStripMenuItem, newDeChuckSequenceToolStripMenuItem, toolStripSeparator2, exportSequenceToolStripMenuItem, importSequenceToolStripMenuItem });
            waveformToolStripMenuItem.Name = "waveformToolStripMenuItem";
            waveformToolStripMenuItem.Size = new Size(62, 20);
            waveformToolStripMenuItem.Text = "波形(&W)";
            // 
            // newChuckSequenceToolStripMenuItem
            // 
            newChuckSequenceToolStripMenuItem.Name = "newChuckSequenceToolStripMenuItem";
            newChuckSequenceToolStripMenuItem.Size = new Size(185, 22);
            newChuckSequenceToolStripMenuItem.Text = "新規Chuck波形(&C)";
            newChuckSequenceToolStripMenuItem.Click += newChuckSequenceToolStripMenuItem_Click;
            // 
            // newDeChuckSequenceToolStripMenuItem
            // 
            newDeChuckSequenceToolStripMenuItem.Name = "newDeChuckSequenceToolStripMenuItem";
            newDeChuckSequenceToolStripMenuItem.Size = new Size(185, 22);
            newDeChuckSequenceToolStripMenuItem.Text = "新規DeChuck波形(&D)";
            newDeChuckSequenceToolStripMenuItem.Click += newDeChuckSequenceToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(182, 6);
            // 
            // exportSequenceToolStripMenuItem
            // 
            exportSequenceToolStripMenuItem.Name = "exportSequenceToolStripMenuItem";
            exportSequenceToolStripMenuItem.Size = new Size(185, 22);
            exportSequenceToolStripMenuItem.Text = "波形のエクスポート(&E)";
            exportSequenceToolStripMenuItem.Click += exportSequenceToolStripMenuItem_Click;
            // 
            // importSequenceToolStripMenuItem
            // 
            importSequenceToolStripMenuItem.Name = "importSequenceToolStripMenuItem";
            importSequenceToolStripMenuItem.Size = new Size(185, 22);
            importSequenceToolStripMenuItem.Text = "波形のインポート(&I)";
            importSequenceToolStripMenuItem.Click += importSequenceToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(65, 20);
            helpToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(158, 22);
            aboutToolStripMenuItem.Text = "バージョン情報(&A)";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip1.Location = new Point(0, 597);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1001, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(55, 17);
            statusLabel.Text = "準備完了";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1001, 619);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "波形サンプルアプリケーション";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newProjectToolStripMenuItem;
        private ToolStripMenuItem openProjectToolStripMenuItem;
        private ToolStripMenuItem saveProjectToolStripMenuItem;
        private ToolStripMenuItem saveProjectAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem waveformToolStripMenuItem;
        private ToolStripMenuItem newChuckSequenceToolStripMenuItem;
        private ToolStripMenuItem newDeChuckSequenceToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exportSequenceToolStripMenuItem;
        private ToolStripMenuItem importSequenceToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel;
    }
}
