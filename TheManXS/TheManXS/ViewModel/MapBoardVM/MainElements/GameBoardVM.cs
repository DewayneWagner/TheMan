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

        private StockTickerBarVM _stockTickerBarVM;
        public StockTickerBarVM StockTicker
        {
            get => _stockTickerBarVM;
            set
            {
                _stockTickerBarVM = value;
                SetValue(ref _stockTickerBarVM, value);
            }
        }

        private TitleBarVM _titleBar;
        public TitleBarVM TitleBar
        {
            get => _titleBar;
            set
            {
                _titleBar = value;
                SetValue(ref _titleBar, value);
            }
        }

        private GameBoardSplitScreenGrid _gameBoardSplitScreenGrid;
        public GameBoardSplitScreenGrid GameBoardSplitScreenGrid
        {
            get => _gameBoardSplitScreenGrid;
            set
            {
                _gameBoardSplitScreenGrid = value;
                SetValue(ref _gameBoardSplitScreenGrid, value);
            }
        }
    }
}
