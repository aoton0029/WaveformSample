using WaveformSample.Core;
using WaveformSample.Waveforms;

namespace WaveformSample
{

    /*
     * プロジェクト名/               # プロジェクトディレクトリ
  ├── プロジェクト名.wproj    # プロジェクトファイル
  ├── Chuck/                  # Chuckシーケンス用ディレクトリ
  │   ├── Chuck_1_名前.wseq   # 各Chuckシーケンスファイル
  │   ├── Chuck_2_名前.wseq
  │   └── ...
  └── DeChuck/                # DeChuckシーケンス用ディレクトリ
      ├── DeChuck_1_名前.wseq # 各DeChuckシーケンスファイル
      ├── DeChuck_2_名前.wseq
      └── ...

     */
    public partial class Form1 : Form
    {
        // プロジェクトサービスのインスタンス
        private ProjectService _projectService;
        private string _baseTitle = "波形サンプルアプリケーション";

        public Form1()
        {
            InitializeComponent();
            _projectService = new ProjectService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // フォーム読み込み時の初期化処理
        }

    }
}
