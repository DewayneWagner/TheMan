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
        public MapVM MapVM { get; set; }
        public StockTickerBarVM StockTicker { get; set; }
        public TitleBarVM TitleBar { get; set; }

        private GameBoardSplitScreenGrid _gameBoardSplitScreenGrid;
        public GameBoardSplitScreenGrid GameBoardSplitScreenGrid
        {
            get => _gameBoardSplitScreenGrid;
            set
            {
                SetValue(ref _gameBoardSplitScreenGrid, value);
                _gameBoardSplitScreenGrid = value;
            }
        }
    }
}
