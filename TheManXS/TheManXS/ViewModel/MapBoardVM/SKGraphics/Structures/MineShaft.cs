using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures
{
    class MineShaft
    {
        Game _game;
        SQ _sq;

        private SKPaint _topRectanglePaint = new SKPaint()
        {
            Style = SKPaintStyle.StrokeAndFill,
            StrokeWidth = (int)(QC.SqSize * 0.05),
        };

        private SKPaint _towerStraightLegsPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black,
        };

        private SKPaint _towerAngleLegPaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
        };

        // ratios to screenSize
        private const float _distanceOfStraightLegFromLeftRatio = 0.3f;
        private const float _topRectangleHeightRatio = 0.25f;
        private const float _topRectangleWidthRatio = 0.5f;
        private const float _distanceOfAngledLegFromLeftRatio = 0.8f;
        private const float _widthOfStraightLegRatio = 0.2f;
        private const float _widthOfAngledLegRatio = 0.1f;
        private const float _distanceFromTopRatio = 0.1f;
        private const float _gapBetweenLegsRatio = 0.05f;

        SKRect _topRect;
        SKRect _straightLegRect;
        SKPath _angledLegPath;

        private float _distanceOfStraghtLegFromLeft, _topRectangleHeight,
            _topRectangleWidth, _distanceOfAngledLegFromLeft, _distanceFromEdgesOfSQ, _widthOfStraightLeg,
            _widthOfAngledLeg, _gapBetweenLegs, _leftX, _topY, _bottomY;

        public MineShaft(Game game, SQ sq)
        {
            _game = game;
            _sq = sq;
            InitFields();
            _topRectanglePaint.Color = _game.PlayerList[_sq.OwnerNumber].SKColor;
            InitTopRectangle();
            InitStraightLeg();
            InitAngledLeg();
            PaintMineshaftOnCanvas();
        }
        private void InitFields()
        {
            _distanceOfAngledLegFromLeft = _distanceOfAngledLegFromLeftRatio * QC.SqSize;
            _topRectangleHeight = _topRectangleHeightRatio * QC.SqSize;
            _topRectangleWidth = _topRectangleWidthRatio * QC.SqSize;
            _distanceFromEdgesOfSQ = _distanceFromTopRatio * QC.SqSize;
            _distanceOfStraghtLegFromLeft = _distanceOfStraightLegFromLeftRatio * QC.SqSize;
            _distanceOfAngledLegFromLeft = _distanceOfAngledLegFromLeftRatio * QC.SqSize;
            _widthOfStraightLeg = _widthOfStraightLegRatio * QC.SqSize;
            _widthOfAngledLeg = _widthOfAngledLegRatio * QC.SqSize;
            _leftX = _sq.Col * QC.SqSize;
            _topY = _sq.Row * QC.SqSize;
            _bottomY = (_sq.Row + 1) * QC.SqSize;
            _gapBetweenLegs = QC.SqSize * _gapBetweenLegsRatio;
        }
        private void InitTopRectangle() 
        {
            float left = (float)(_leftX + ((QC.SqSize - _topRectangleWidth) / 2));
            float top = (float)(_topY + _distanceFromEdgesOfSQ);
            float right = (float)(left + _topRectangleWidth);
            float bottom = (float)(_topY + _distanceFromEdgesOfSQ + _topRectangleHeight);

            _topRectanglePaint.StrokeWidth = 3;
            _topRect = new SKRect(left, top, right, bottom);
        }
        private void InitStraightLeg()
        {
            float left = (float)(_distanceOfStraghtLegFromLeft + _leftX);
            float top = (float)(_topY + _distanceFromEdgesOfSQ + _topRectangleHeight);
            float right = (float)(left + _widthOfStraightLeg);
            float bottom = (float)(_bottomY - _distanceFromEdgesOfSQ);

            _straightLegRect = new SKRect(left, top, right, bottom);
        }
        private void InitAngledLeg()
        {
            float x = (float)(_leftX + _distanceOfStraghtLegFromLeft + _widthOfStraightLeg + _gapBetweenLegs);
            float y = (float)(_topY + _distanceFromEdgesOfSQ + _topRectangleHeight);
            SKPoint startPoint = new SKPoint(x, y);

            float x1 = (float)(_leftX + _distanceOfAngledLegFromLeft);
            float y1 = (float)(_bottomY - _distanceFromEdgesOfSQ);
            SKPoint endPoint = new SKPoint(x1, y1);

            _angledLegPath = new SKPath();
            _towerAngleLegPaint.StrokeWidth = _widthOfAngledLeg;

            _angledLegPath.MoveTo(startPoint);
            _angledLegPath.LineTo(endPoint);
            _angledLegPath.Close();
        }
        private void PaintMineshaftOnCanvas()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {                
                canvas.DrawRect(_straightLegRect, _towerStraightLegsPaint);
                canvas.DrawPath(_angledLegPath, _towerAngleLegPaint);
                canvas.DrawRect(_topRect, _topRectanglePaint);
                canvas.Save();
            }
        }
    }
}
