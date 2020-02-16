﻿using SkiaSharp;
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
        private MapVM _mapVM;
        bool _createNewMap;
        GameBoardSplitScreenGrid _gameBoardSplitScreenGrid;
        public MapBoard()
        {
            _createNewMap = true;
            InitializeComponent();
            GameBoardVM g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            g.GameBoardSplitScreenGrid = _gameBoardSplitScreenGrid = (GameBoardSplitScreenGrid)ScreenGrid;
        }

        private void mapBoardCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            QC.ScreenHeight = e.Info.Height;
            QC.ScreenWidth = e.Info.Width;
            QC.MapCanvasViewHeight = mapBoardCanvasView.Height;
            QC.MapCanvasViewWidth = mapBoardCanvasView.Width;
            
            if (_createNewMap)
            {
                _mapVM = new MapVM();
                _createNewMap = false;
                _mapVM.MapCanvasView = mapBoardCanvasView;
                _mapVM.MapCanvasView.IgnorePixelScaling = true;
            }

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            QC.SqSize = _mapVM.Map.Width / QC.ColQ;

            canvas.Clear();
            canvas.SetMatrix(_mapVM.MapMatrix);
            canvas.DrawBitmap(_mapVM.Map, 0, 0);
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {
            var t = _mapVM.MapTouchList;
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
            switch (_mapVM.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    new ExecuteOneFingerSelect(_gameBoardSplitScreenGrid);
                    break;
                case MapTouchType.OneFingerDragSelect:
                    new ExecuteOneFingerDrag(_gameBoardSplitScreenGrid);
                    break;
                case MapTouchType.TwoFingerPan:
                    new ExecuteTwoFingerPan(_mapVM);
                    break;
                case MapTouchType.Pinch:
                    new ExecutePinch(_mapVM);
                    break;
                default:
                    break;
            }
        }
    }
}