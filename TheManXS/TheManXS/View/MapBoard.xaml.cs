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
        private GameBoardVM g;
        public MapBoard(bool isForMapVM) { }
        public MapBoard()
        {
            g = (GameBoardVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];            
            _createNewMap = true;
            InitializeComponent();     
        }

        private void mapBoardCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var m = g.MapVM;

            if (_createNewMap) { createNewMap(); }

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

                m = new MapVM();
                _createNewMap = false;
                m.MapCanvasView = mapBoardCanvasView;
                m.MapCanvasView.HorizontalOptions = LayoutOptions.FillAndExpand;
                m.MapCanvasView.VerticalOptions = LayoutOptions.FillAndExpand;
                m.MapCanvasView.IgnorePixelScaling = true;
            }
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {
            var t = g.MapVM.MapTouchList;
            if (g.MapVM.TouchEffectsEnabled) { t.AddTouchAction(args); }

            if (t.AllTouchEffectsExited && t.Count != 0)
            {
                _createNewMap = false;
                g.MapVM.TouchEffectsEnabled = false;
                ExecuteTouch();
                t = new MapTouchListOfMapTouchIDLists();
                _createNewMap = true;
            }
            else if (t.NoExecutionRequired) { t = new MapTouchListOfMapTouchIDLists(); }

            //try
            //{                
            //    t.AddTouchAction(args);

            //    if (t.AllTouchEffectsExited && t.Count != 0)
            //    {
            //        _createNewMap = false;
            //        ExecuteTouch();
            //        t = new MapTouchListOfMapTouchIDLists();
            //        _createNewMap = true;
            //    }
            //    else if (t.NoExecutionRequired) { t = new MapTouchListOfMapTouchIDLists(); }
            //}
            //catch { t = new MapTouchListOfMapTouchIDLists(); }
        }

        private void ExecuteTouch()
        {
            var m = g.MapVM;
            switch (m.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    new ExecuteOneFingerSelect(g);                    
                    AddSidePanel();                    
                    break;
                case MapTouchType.OneFingerDragSelect:
                    new ExecuteOneFingerDrag(g);
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

        private void AddSidePanel()
        {
            SplitScreenGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            SplitScreenGrid.Children.Add(g.ActionPanelGrid,1,0);
            g.SideSQActionPanelExists = true;
        }

        // from Action Panel Class before deleting
        public void CloseActionPanel()
        {
            g.MapVM.TouchEffectsEnabled = true;
            SplitScreenGrid.Children.Remove(g.ActionPanelGrid);
            g.SideSQActionPanelExists = false;
            SplitScreenGrid.ColumnDefinitions.RemoveAt(1);
        }
    }
}