using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel.MapBoardVM;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.MapBoardVM.TouchExecution;
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
        bool _createNewMap;
        GameBoardSplitScreenGrid _gameBoardSplitScreenGrid;
        public MapBoard()
        {
            _createNewMap = true;
            InitializeComponent();

            GameBoardVM g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            _gameBoardSplitScreenGrid = g.GameBoardSplitScreenGrid;
        }

        private void mapBoardCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            QC.ScreenHeight = e.Info.Height;
            QC.ScreenWidth = e.Info.Width;
            QC.MapCanvasViewHeight = mapBoardCanvasView.Height;
            QC.MapCanvasViewWidth = mapBoardCanvasView.Width;

            var m = _gameBoardSplitScreenGrid.MapVM;
            
            if (_createNewMap)
            {
                m = new MapVM();
                _createNewMap = false;
                m.MapCanvasView = mapBoardCanvasView;
                m.MapCanvasView.IgnorePixelScaling = true;
            }

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            QC.SqSize = m.Map.Width / QC.ColQ;

            canvas.Clear();
            canvas.SetMatrix(m.MapMatrix);
            canvas.DrawBitmap(m.Map, 0, 0);
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {
            var t = _gameBoardSplitScreenGrid.MapVM.MapTouchList;
            try
            {                
                t.AddTouchAction(args);

                if (t.AllTouchEffectsExited && t.Count != 0)
                {
                    _createNewMap = false;
                    ExecuteTouch();
                    t = new MapTouchListOfMapTouchIDLists();
                    _createNewMap = true;
                }
                else if (t.NoExecutionRequired) { t = new MapTouchListOfMapTouchIDLists(); }
            }
            catch { t = new MapTouchListOfMapTouchIDLists(); }
        }
        private void ExecuteTouch()
        {
            var m = _gameBoardSplitScreenGrid.MapVM;
            switch (m.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    new ExecuteOneFingerSelect(_gameBoardSplitScreenGrid);
                    break;
                case MapTouchType.OneFingerDragSelect:
                    new ExecuteOneFingerDrag(_gameBoardSplitScreenGrid);
                    break;
                case MapTouchType.TwoFingerPan:
                    new ExecuteTwoFingerPan(m);
                    break;
                case MapTouchType.Pinch:
                    new ExecutePinch(m);
                    break;
                default:
                    break;
            }
        }
    }
}