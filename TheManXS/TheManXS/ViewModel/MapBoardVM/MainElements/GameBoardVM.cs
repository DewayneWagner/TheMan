using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.Services;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardVM : BaseViewModel
    {
        public GameBoardVM() { }
        public GameBoardVM(bool isForAppDictionary) { }

        public GameBoardSplitScreenGrid SplitScreenGrid { get; set; }
        public MapVM ActualMap { get; set; }
        public StockTickerBarVM StockTicker { get; set; }
        public TitleBarVM TitleBar { get; set; }
    }
}
