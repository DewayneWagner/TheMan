using Microcharts.Forms;
using System.Collections.Generic;
using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials.Charts
{
    public class InteractiveChartView : ChartView
    {
        Game _game;
        //List<Entry> _chartviewEntryList;
        public InteractiveChartView(Game game)
        {
            _game = game;

        }

    }
}
