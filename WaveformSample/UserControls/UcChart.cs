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
    public partial class UcChart : UserControl
    {
        private IChartRenderer _renderer;
        private IWaveformSequence _sequence;

        /// <summary>
        /// チャートレンダラー
        /// </summary>
        [Browsable(false)]
        public IChartRenderer Renderer
        {
            get => _renderer;
            set
            {
                _renderer = value;
                RenderChart();
            }
        }

        /// <summary>
        /// 波形シーケンス
        /// </summary>
        [Browsable(false)]
        public IWaveformSequence Sequence
        {
            get => _sequence;
            set
            {
                _sequence = value;
                RenderChart();
            }
        }

        /// <summary>
        /// チャートのタイトル
        /// </summary>
        [Category("Chart"), Description("チャートのタイトル")]
        public string ChartTitle { get; set; }

        /// <summary>
        /// X軸のラベル
        /// </summary>
        [Category("Chart"), Description("X軸のラベル")]
        public string XAxisLabel { get; set; }

        /// <summary>
        /// Y軸のラベル
        /// </summary>
        [Category("Chart"), Description("Y軸のラベル")]
        public string YAxisLabel { get; set; }

        /// <summary>
        /// グリッド線の表示
        /// </summary>
        [Category("Chart"), Description("グリッド線の表示")]
        public bool ShowGridLines { get; set; } = true;

        public UcChart()
        {
            InitializeComponent();
            // ダブルバッファリングを有効化して描画のちらつきを防止
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// チャートを描画します
        /// </summary>
        public void RenderChart()
        {
            if (_renderer != null && _sequence != null)
            {
                _renderer.RenderChart(this, _sequence);
                Invalidate();
            }
        }

        /// <summary>
        /// チャートを描画します
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        /// <param name="sequence">波形シーケンス</param>
        public void RenderChart(IChartRenderer renderer, IWaveformSequence sequence)
        {
            _renderer = renderer;
            _sequence = sequence;
            RenderChart();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // カスタム描画ロジックがある場合はここに追加
            // 基本的なチャート描画はIChartRendererに任せる
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // サイズ変更時にチャートを再描画
            RenderChart();
        }
    }
}

