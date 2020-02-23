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
        private GameBoardVM _gameBoardVM;
        
        public MapBoard()
        {
            _gameBoardVM = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            
            _createNewMap = true;
            InitializeComponent();
            _gameBoardVM.SplitScreenGrid = SplitScreenGrid;
        }

        private void mapBoardCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {            
            if (_createNewMap) { createNewMap(); }

            var m = _gameBoardVM.MapVM;

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            QC.SqSize = m.Map.Width / QC.ColQ;

            canvas.Clear();
            canvas.SetMatrix(m.MapMatrix);
            canvas.DrawBitmap(m.Map, 0, 0);

            void createNewMap()
            {
                QC.ScreenHeight = e.Info.Height;
                QC.ScreenWidth = e.Info.Width;
                QC.MapCanvasViewHeight = mapBoardCanvasView.Height;
                QC.MapCanvasViewWidth = mapBoardCanvasView.Width;

                m = _gameBoardVM.MapVM = new MapVM(_gameBoardVM);
                _createNewMap = false;
                m.MapCanvasView = mapBoardCanvasView;
                m.MapCanvasView.IgnorePixelScaling = true;
            }
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {            
            if (_gameBoardVM.MapVM.TouchEffectsEnabled)
            {
                var t = _gameBoardVM.MapVM.MapTouchList;
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
            
            // try / catch loop to wrap this section in, once bugs are out to prevent crashing
            //try {}
            //catch { t = new MapTouchListOfMapTouchIDLists(); }
        }

        private void ExecuteTouch()
        {
            var m = _gameBoardVM.MapVM;
            switch (m.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    new ExecuteOneFingerSelect(_gameBoardVM);
                    break;
                case MapTouchType.OneFingerDragSelect:
                    new ExecuteOneFingerDrag(_gameBoardVM);
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