using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.Action;
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

        private GameBoardGridVM _gameBoardGridVM;
        public GameBoardGridVM GameBoardGridVM
        {
            get => _gameBoardGridVM;
            set
            {
                _gameBoardGridVM = value;
                SetValue(ref _gameBoardGridVM, value);
            }
        }

        //private MapVM _mapVM;
        //public MapVM MapVM
        //{
        //    get => _mapVM;
        //    set
        //    {
        //        _mapVM = value;
        //        SetValue(ref _mapVM, value);
        //    }
        //}

        
        
        public bool SideSQActionPanelExists { get; set; }
    }
}
