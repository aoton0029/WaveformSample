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
        public UcChart()
        {
            InitializeComponent();
            // ダブルバッファリングを有効化して描画のちらつきを防止
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
        }
    }
}

