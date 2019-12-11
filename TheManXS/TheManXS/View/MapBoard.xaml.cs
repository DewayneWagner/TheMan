using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel.MapBoardVM;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static TheManXS.Model.Settings.SettingsMaster;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapBoard : ContentPage
    {
        private MapVM _mapVM;
        bool _createNewMap;
        private TerrainTypeE _tt;
        public MapBoard()
        {
            _createNewMap = true;
            InitializeComponent();
        }

        private void mapBoardCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            QC.ScreenHeight = e.Info.Height;
            QC.ScreenWidth = e.Info.Width;

            // things that only need to be set once upon creation of canvas
            if (_createNewMap)
            {
                _mapVM = new MapVM();
                _mapVM.MapCanvasView = mapBoardCanvasView;
                _mapVM.MapCanvasView.IgnorePixelScaling = true;
                _createNewMap = false;
            }

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            QC.RenderedSQSize = _mapVM.Map.Width / QC.ColQ;

            canvas.Clear();
            canvas.SetMatrix(_mapVM.MapMatrix);
            canvas.DrawBitmap(_mapVM.Map, 0, 0);

            QC.MapCanvasViewHeight = mapBoardCanvasView.Height;
            QC.MapCanvasViewWidth = mapBoardCanvasView.Width;
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {
            try
            {
                var t = _mapVM.MapTouchList;
                t.AddTouchAction(args);

                if (t.AllTouchEffectsExited && t.Count != 0)
                {
                    _createNewMap = false;
                    new TouchExecution(_mapVM);
                    t = new MapTouchListOfMapTouchIDLists();
                    _createNewMap = true;
                }
                else if (t.NoExecutionRequired) { t = new MapTouchListOfMapTouchIDLists(); }
            }
            catch { _mapVM.MapTouchList = new MapTouchListOfMapTouchIDLists(); }
        }
    }
}