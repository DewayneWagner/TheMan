﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchTracking;

namespace TheManXS.ViewModel.MapBoardVM.TouchExecution
{
    public class ExecuteOneFingerSelect
    {
        GameBoardSplitScreenGrid _gameBoardSplitScreenGrid;
        SKPaint highlightedSq = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red,
        };
        SQ _touchedSQ;
        public ExecuteOneFingerSelect(GameBoardSplitScreenGrid gameBoardSplitScreenGrid)
        {
            _gameBoardSplitScreenGrid = gameBoardSplitScreenGrid;
            ExecuteOneFingerSelectAction();
        }
        private void ExecuteOneFingerSelectAction()
        {
            Coordinate touchPoint = new Coordinate(getTouchPointOnBitMap());
            _gameBoardSplitScreenGrid.ActiveSQ = _touchedSQ = _mapVM.SquareDictionary[touchPoint.SQKey];
            paintSKRect();

            // create side panel here
            _mapVM.GameBoardSplitScreenGrid.AddSideActionPanel(MapBoardVM.Action.ActionPanel.PanelType.SQ);
            
            SKPoint getTouchPointOnBitMap()
            {
                SKPoint touchPointOnScreen = getTouchPointOnScreen();
                float bitmapX = ((touchPointOnScreen.X - _mapVM.MapMatrix.TransX) / _mapVM.MapMatrix.ScaleX);
                float bitmapY = ((touchPointOnScreen.Y - _mapVM.MapMatrix.TransY) / _mapVM.MapMatrix.ScaleY);

                return new SKPoint(bitmapX, bitmapY);
            }
            
            SKPoint getTouchPointOnScreen() => _mapVM.MapTouchList[0].FirstOrDefault(p => p.Type == TouchActionType.Pressed).SKPoint;
            
            void paintSKRect()
            {
                using (SKCanvas gameBoard = new SKCanvas(_mapVM.Map))
                {
                    gameBoard.DrawRect(touchPoint.SKRect,highlightedSq);
                    gameBoard.Save();
                }
            }
        }
    }
}
