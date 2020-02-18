using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class StockTickerBarVM
    {
        public StockTickerBarVM(bool isForInitializingGameBoardVM) { }
        public StockTickerBarVM()
        {
            GameBoardVM g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            g.StockTicker = this;
            Ticker = "PWT $15.32 +1.2% | CPG $2.34 -0.2% | TLM $5.43 +4% | Gold $45.32 +2% | Oil $65.34 -4% |";
        }
        public string Ticker { get; set; }
    }
}
