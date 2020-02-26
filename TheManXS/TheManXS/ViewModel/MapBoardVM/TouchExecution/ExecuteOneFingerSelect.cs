using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecuteOneFingerSelect
    {
        Game _game;
        SKPaint highlightedSq = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red,
        };
        
        public ExecuteOneFingerSelect(Game game)
        {
            _game = game;
            ExecuteOneFingerSelectAction();
        }
        public SelectedSQHighlight SelectedSQHighlight { get; set; }

        private void ExecuteOneFingerSelectAction()
        {
            var m = _game.GameBoardVM.MapVM;
            Coordinate touchPoint = new Coordinate(getTouchPointOnBitMap());
            _game.ActiveSQ = _game.SquareDictionary[touchPoint.SQKey];
            addSidePanel();

            SKPoint getTouchPointOnBitMap()
            {
                SKPoint touchPointOnScreen = getTouchPointOnScreen();
                float bitmapX = ((touchPointOnScreen.X - m.MapMatrix.TransX) / m.MapMatrix.ScaleX);
                float bitmapY = ((touchPointOnScreen.Y - m.MapMatrix.TransY) / m.MapMatrix.ScaleY);

                return new SKPoint(bitmapX, bitmapY);
            }

            SKPoint getTouchPointOnScreen() => m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
                        
            void addSidePanel()
            {
                var g = _game.GameBoardVM;
                g.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, _game);
                g.SplitScreenGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition()
                    { Width = new GridLength(1, GridUnitType.Auto) });
                g.SplitScreenGrid.Children.Add(g.ActionPanelGrid, 1, 0);
                g.SideSQActionPanelExists = true;
            }
        }
    }
}
