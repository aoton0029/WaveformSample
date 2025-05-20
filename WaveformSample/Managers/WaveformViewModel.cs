using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveformSample.Charts;
using WaveformSample.UserControls;
using WaveformSample.Waveforms;

namespace WaveformSample.Managers
{
    public class WaveformViewModel
    {
        private ObservableCollection<WaveformStep> _waveformDataList;
        private readonly UcGrid _grid;
        private readonly UcChart _chart;

        public WaveformViewModel(UcGrid grid, UcChart chart)
        {
            _chart = chart;
            _grid = grid;
        }

        public void Initialize()
        {
            _grid.Initialize();
            _chart.Initialize();
        }

           
    }
}
