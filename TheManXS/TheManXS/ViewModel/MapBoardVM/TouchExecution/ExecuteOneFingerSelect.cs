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
            var m = g.GameBoardGridVM.GameBoardGrid.MapVM;
            Coordinate touchPoint = new Coordinate(getTouchPointOnBitMap());
            m.ActiveSQ = m.SquareDictionary[touchPoint.SQKey];
            paintSKRect();
            g.GameBoardGridVM.GameBoardGrid.ActionPanelGrid = new ActionPanelGrid(ActionPanelGrid.PanelType.SQ, m);

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
                using (SKCanvas gameBoard = new SKCanvas(g.GameBoardGridVM.GameBoardGrid.MapVM.Map))
                {
                    gameBoard.DrawRect(touchPoint.SKRect,highlightedSq);
                    gameBoard.Save();
                }
            }
        }
    }
}
