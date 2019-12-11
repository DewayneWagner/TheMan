using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpTouchEffects.MapBoardVM;
using SkiaSharpTouchEffects.MapBoardVM.MapColors;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static SkiaSharpTouchEffects.MapBoardVM.TileConstructCalc;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM
{
    public class MapVM : BaseViewModel
    {
        private SKPaint tile = new SKPaint() { Style = SKPaintStyle.StrokeAndFill };
        private System.Random rnd = new System.Random();

        public MapVM(TerrainTypeE tt)
        {
            TerrainType = tt;
            if (QC.IsNewGame) { new Map(this).InitMap(); }
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
