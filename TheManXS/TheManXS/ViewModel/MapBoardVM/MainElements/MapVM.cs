using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static TheManXS.Model.Settings.SettingsMaster;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class MapVM : BaseViewModel
    {
        private SKPaint tile = new SKPaint() { Style = SKPaintStyle.StrokeAndFill };
        private System.Random rnd = new System.Random();

        public MapVM(bool isForAppDictionary) { }

        public MapVM()
        {
            // need to update this later....
            GameBoardVM g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            g.ActualMap = this;

            QC.IsNewGame = true;

            if (QC.IsNewGame) 
            {
                Map = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));
                InfrastructureBuilder = new Infrastructure.Builder(this);
                new Map(this);
            }
            else { LoadMap(); }

            MapTouchList = new MapTouchListOfMapTouchIDLists();
            MapMatrix = SKMatrix.MakeIdentity();
        }
        private SKBitmap _map;
        public SKBitmap Map
        {
            get => _map;
            set
            {
                _map = value;
                SetValue(ref _map, value);
            }
        }

        public MapTouchListOfMapTouchIDLists MapTouchList { get; set; }
        public SKCanvasView MapCanvasView { get; set; }
        public TitleBarVM TitleBar { get; set; }
        public StockTickerBarVM StockTicker { get; set; }
        public SqAttributesList SqAttributesList { get; set; }
        public Infrastructure.Builder InfrastructureBuilder { get; set; }

        public SKMatrix MapMatrix;

        private GameBoardSplitScreenGrid _screenGrid;
        public GameBoardSplitScreenGrid ScreenGrid
        {
            get => _screenGrid;
            set
            {
                SetValue(ref _screenGrid, value);
                _screenGrid = value;
            }
        }
        
        private void LoadMap() { }
    }   
    
}
