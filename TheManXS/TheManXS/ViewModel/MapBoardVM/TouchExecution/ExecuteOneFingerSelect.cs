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
            Coordinate touchPoint = new Coordinate(getTouchPointOnBitMap());
            g.MapVM.ActiveSQ = g.MapVM.SquareDictionary[touchPoint.SQKey];
            paintSKRect();
            g.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, g.MapVM);

            SKPoint getTouchPointOnBitMap()
            {
                SKPoint touchPointOnScreen = getTouchPointOnScreen();
                float bitmapX = ((touchPointOnScreen.X - g.MapVM.MapMatrix.TransX) / g.MapVM.MapMatrix.ScaleX);
                float bitmapY = ((touchPointOnScreen.Y - g.MapVM.MapMatrix.TransY) / g.MapVM.MapMatrix.ScaleY);

                return new SKPoint(bitmapX, bitmapY);
            }

            SKPoint getTouchPointOnScreen() => g.MapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            
            void paintSKRect()
            {
                using (SKCanvas gameBoard = new SKCanvas(g.MapVM.Map))
                {
                    gameBoard.DrawRect(touchPoint.SKRect,highlightedSq);
                    gameBoard.Save();
                }
            }
        }
    }
}
