using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static TheManXS.Model.Settings.SettingsMaster;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM
{
    public class MapVM : BaseViewModel
    {
        private SKPaint tile = new SKPaint() { Style = SKPaintStyle.StrokeAndFill };
        private System.Random rnd = new System.Random();

        public MapVM(TerrainTypeE tt)
        {
            // need to update this later....
            QC.IsNewGame = true;

            TerrainType = tt;
            if (QC.IsNewGame) 
            {
                Map = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));
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
        public TerrainTypeE TerrainType { get; set; }

        public SKMatrix MapMatrix;

        private Grid _screenGrid;
        public Grid ScreenGrid
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
