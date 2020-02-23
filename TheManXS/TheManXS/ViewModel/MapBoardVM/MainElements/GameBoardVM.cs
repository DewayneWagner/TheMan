using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardVM : BaseViewModel
    {
        public GameBoardVM() 
        {
            App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)] = this;
        }
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

        private Grid _splitScreenGrid;
        public Grid SplitScreenGrid
        {
            get => _splitScreenGrid;
            set
            {
                _splitScreenGrid = value;
                SetValue(ref _splitScreenGrid, value);
            }
        }

        private GameBoardGrid _gameBoardGrid;
        public GameBoardGrid GameBoardGrid
        {
            get => _gameBoardGrid;
            set
            {
                _gameBoardGrid = value;
                SetValue(ref _gameBoardGrid, value);
            }
        }

        public bool SideSQActionPanelExists { get; set; }
    }
}
