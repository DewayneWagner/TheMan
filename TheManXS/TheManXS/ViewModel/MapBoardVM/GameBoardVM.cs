using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.Services;
using TheManXS.ViewModel.MapBoardVM.Action;

namespace TheManXS.ViewModel.MapBoardVM
{
    public class GameBoardVM : BaseViewModel
    {
        public GameBoardVM(bool isForAppProperties) { }

        public GameBoardVM()
        {
            App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)] = this;

            SquareSize = QC.SqSize;

            TitleBar = new TitleBarVM(true);
            StockTicker = new StockTickerBarVM(true);
            ActualGameBoardVM = new ActualGameBoardVM(true);

            SqAttributesList = new SqAttributesList();
        }

        public TitleBarVM TitleBar { get; set; }
        public StockTickerBarVM StockTicker { get; set; }
        public ActualGameBoardVM ActualGameBoardVM { get; set; }
        public Tile CenterTile { get; set; }
        public SqAttributesList SqAttributesList { get; set; }

        private double _squareSize;
        public double SquareSize
        {
            get => _squareSize;
            set
            {
                _squareSize = value;
                SetValue(ref _squareSize, value);
            }
        }
    }
}
