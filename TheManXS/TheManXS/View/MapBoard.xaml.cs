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
            var m = _gameBoardVM.GameBoardGrid.MapVM;

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

                m = createMapObjects();
                _createNewMap = false;
                m.MapCanvasView = mapBoardCanvasView;
                m.MapCanvasView.IgnorePixelScaling = true;
                
                //m.MapCanvasView.HorizontalOptions = LayoutOptions.FillAndExpand;
                //m.MapCanvasView.VerticalOptions = LayoutOptions.FillAndExpand;
            }

            MapVM createMapObjects()
            {
                _gameBoardVM.GameBoardGrid = new GameBoardGrid(_gameBoardVM);
                _gameBoardVM.Content = _gameBoardVM.GameBoardGrid;
                _gameBoardVM.GameBoardGrid.MapVM = new MapVM(_gameBoardVM);

                _gameBoardVM.GameBoardGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                _gameBoardVM.GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

                _gameBoardVM.GameBoardGrid.Children.Add(_gameBoardVM.GameBoardGrid.MapVM, 0, 0);
                return _gameBoardVM.GameBoardGrid.MapVM;
            }
        }

        private void TouchEffect_TouchAction(object sender, ViewModel.MapBoardVM.TouchTracking.TouchActionEventArgs args)
        {
            var t = _gameBoardVM.GameBoardGrid.MapVM.MapTouchList;
            //if (t.TouchEffectsEnabled) 
            { t.AddTouchAction(args); }

            if (t.AllTouchEffectsExited && t.Count != 0)
            {
                _createNewMap = false;
                //g.MapVM.TouchEffectsEnabled = false;
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
            var m = _gameBoardVM.GameBoardGrid.MapVM;
            switch (m.MapTouchList.MapTouchType)
            {
                case MapTouchType.OneFingerSelect:
                    new ExecuteOneFingerSelect(_gameBoardVM);
                    AddSidePanel();                 
                    //_gameBoardVM.GameBoardGrid.AddSidePanel();
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

        private void AddSidePanel()
        {
            SplitScreenGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            //_gameBoardVM.GameBoardGrid.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, _gameBoardVM.GameBoardGrid.MapVM);
            //SplitScreenGrid.Children.Add(_gameBoardVM.GameBoardGrid.ActionPanelGrid, 1, 0);
            _gameBoardVM.SplitScreenGrid.Children.Add(new ActionPanelGrid(ActionPanelGrid.PanelType.SQ,
                _gameBoardVM.GameBoardGrid.MapVM),1,0);

            //_gameBoardVM.GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            //_gameBoardVM.GameBoardGrid.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, _gameBoardVM.GameBoardGrid.MapVM);
            //_gameBoardVM.GameBoardGrid.Children.Add(_gameBoardVM.GameBoardGrid.ActionPanelGrid,1,0);
            //_gameBoardVM.SideSQActionPanelExists = true;
        }

        // from Action Panel Class before deleting
        public void CloseActionPanel()
        {
            //g.MapVM.TouchEffectsEnabled = true;
            _gameBoardVM.GameBoardGrid.Children.Remove(_gameBoardVM.GameBoardGrid.ActionPanelGrid);
            _gameBoardVM.SideSQActionPanelExists = false;
            _gameBoardVM.GameBoardGrid.ColumnDefinitions.RemoveAt(1);
        }
    }
}