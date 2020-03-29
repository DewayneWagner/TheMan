using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.MapBoardVM.TouchExecution;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapBoard : ContentPage
    {
        bool _createNewMap;
        private Game _game;
        bool Execute;
        
        public MapBoard()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            
            _createNewMap = true;
            InitializeComponent();
            _game.GameBoardVM.SplitScreenGrid = SplitScreenGrid;
            TickerSP.Children.Add(_game.GameBoardVM.TickerVM.Ticker);
        }

        private void mapBoardCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {            
            if (_createNewMap) { createNewMap(); }

            var m = _game.GameBoardVM.MapVM;

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            QC.SqSize = m.SKBitMapOfMap.Width / QC.ColQ;

            canvas.Clear();
            canvas.SetMatrix(m.MapMatrix);
            canvas.DrawBitmap(m.SKBitMapOfMap, 0, 0);

            void createNewMap()
            {
                QC.ScreenHeight = e.Info.Height;
                QC.ScreenWidth = e.Info.Width;
                
                m = _game.GameBoardVM.MapVM = new MapVM(_game);
                _createNewMap = false;
                m.MapCanvasView = mapBoardCanvasView;
                m.MapCanvasView.IgnorePixelScaling = true;

                double height = _game.GameBoardVM.MapVM.SKBitMapOfMap.Height;
                double width = _game.GameBoardVM.MapVM.SKBitMapOfMap.Width;

                QC.MapCanvasViewHeight = mapBoardCanvasView.Height;
                QC.MapCanvasViewWidth = mapBoardCanvasView.Width;

            }
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {
            var m = _game.GameBoardVM.MapVM;
            if (_game.GameBoardVM.TouchEffectsEnabled)            
            {
                if (isTouchOnGameBoardPortionOfScreen())
                {
                    m.MapTouchList.AddTouchAction(args);

                    if (m.MapTouchList.AllTouchEffectsExited && m.MapTouchList.Count != 0)
                    {
                        _game.GameBoardVM.TouchEffectsEnabled = false;
                        ExecuteTouch();
                        m.MapTouchList = new MapTouchListOfMapTouchIDLists();
                        _game.GameBoardVM.TouchEffectsEnabled = true;
                    }
                    else if (m.MapTouchList.NoExecutionRequired) { m.MapTouchList = new MapTouchListOfMapTouchIDLists(); }
                }
            }
            bool isTouchOnGameBoardPortionOfScreen()
            {                
                if(args.Location.X < m.MapCanvasView.Width && args.Location.Y < m.MapCanvasView.Height) { return true; }
                else { return false; }
            }
            
            // try / catch loop to wrap this section in, once bugs are out to prevent crashing
            //try {}
            //catch { t = new MapTouchListOfMapTouchIDLists(); }
        }

        private void ExecuteTouch()
        {
            var m = _game.GameBoardVM.MapVM;
            switch (m.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    _game.GameBoardVM.TouchEffectsEnabled = false;
                    new ExecuteOneFingerSelect(_game);
                    break;
                case MapTouchType.OneFingerDragSelect:
                    _game.GameBoardVM.TouchEffectsEnabled = false;
                    new ExecuteOneFingerDrag(_game);
                    break;
                case MapTouchType.TwoFingerPan:
                    new ExecuteTwoFingerPan(_game);
                    break;
                case MapTouchType.Pinch:
                    new ExecutePinch(_game);
                    break;
                default:
                    break;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Execute = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                TickerSP.TranslationX -= 5f;
                if(Math.Abs(TickerSP.TranslationX) > Width)
                {
                    TickerSP.TranslationX = TickerSP.Width;
                }
                return Execute;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Execute = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            new EndTurnAction(_game);
            TickerSP.Children.RemoveAt(0);
            TickerSP.Children.Add(_game.GameBoardVM.TickerVM.Ticker);
        }
    }
}