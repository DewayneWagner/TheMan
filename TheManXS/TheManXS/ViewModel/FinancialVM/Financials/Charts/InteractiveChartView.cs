using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp.Views;
using Microcharts.Forms;
using TheManXS.Model.Main;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials.Charts
{
    public class InteractiveChartView : ChartView
    {
        Game _game;
        List<Entry> _chartviewEntryList;
        public InteractiveChartView(Game game)
        {
            _game = game;

        }
        
    }
}
