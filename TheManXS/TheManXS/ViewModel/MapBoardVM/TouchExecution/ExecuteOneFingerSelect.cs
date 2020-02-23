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
        GameBoardVM g;
        SKPaint highlightedSq = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red,
        };

        public ExecuteOneFingerSelect(GameBoardVM gameBoardVM)
        {
            g = gameBoardVM;
            ExecuteOneFingerSelectAction();
        }
        private void ExecuteOneFingerSelectAction()
        {
            var m = g.MapVM;
            Coordinate touchPoint = new Coordinate(getTouchPointOnBitMap());
            m.ActiveSQ = m.SquareDictionary[touchPoint.SQKey];
            paintSKRect();
            addSidePanel();

            SKPoint getTouchPointOnBitMap()
            {
                SKPoint touchPointOnScreen = getTouchPointOnScreen();
                float bitmapX = ((touchPointOnScreen.X - m.MapMatrix.TransX) / m.MapMatrix.ScaleX);
                float bitmapY = ((touchPointOnScreen.Y - m.MapMatrix.TransY) / m.MapMatrix.ScaleY);

                return new SKPoint(bitmapX, bitmapY);
            }

            SKPoint getTouchPointOnScreen() => m.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            
            void paintSKRect()
            {
                using (SKCanvas gameBoard = new SKCanvas(g.MapVM.Map))
                {
                    gameBoard.DrawRect(touchPoint.SKRect,highlightedSq);
                    gameBoard.Save();
                }
            }

            void addSidePanel()
            {
                g.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, g);
                g.SplitScreenGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition()
                    { Width = new GridLength(1, GridUnitType.Auto) });
                g.SplitScreenGrid.Children.Add(g.ActionPanelGrid, 1, 0);
                g.SideSQActionPanelExists = true;
                g.MapVM.TouchEffectsEnabled = false;
            }
        }
    }
}
