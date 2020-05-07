using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos
{
    class Logo
    {
        private SKCanvas _canvas;
        private SKRect _position;

        private const float _cornerRadiusOfOutsideBorderRatio = 0.15f;

        private const float _topPointOfTieKnotFromEdgeXRatio = 0.45f;
        private const float _topPointOfTieKnotFromTopOfShoulderYRatio = 0.055f;
        private const float _verticalThicknessOfTieKnotRatio = 0.05f;
        private const float _bottomPointOfTieKnotFromEdgeXRatio = 0.46f;

        private const float _gapBetweenTieAndSuitJacketRatio = 0.1f;
        private const float _widthOfTieAtWidestPointFromEdgeXRatio = 0.44f;
        
        private const float _cuffOutsideEdgeFromVerticalEdgeRatio = 0.075f;
        private const float _cuffInsideEdgeFromVerticalEdgeRatio = 0.2f;
        private const float _verticalThicknessShoulderRatio = 0.2f;
        private const float _armpitFromVerticalEdgeRatio = 0.3f;
        private const float _outsideEdgeOfShoulderRatio = 0.15f;

        // make this the dynamic variable - with others floating in relation to
        private const float _topOfShoulderRatioFromTop = 0.15f;

        private const float _insideLapelFromVerticalEdgeRatio = 0.4f;
        private const float _topButtonFromTopRatio = 0.65f;
        private const float _waistFromVerticalEdgeRatio = 0.33f;

        private const float _verticalThicknessOfSideTieRatio = 0.75f;
        private const float _widthOfSideOfTiePastKnotFromEdgeRatio = 0.35f;
        private const float _sideTieHeightVariationRatio = 0.025f;
        private const float _strokeWidthRatio = 0.01f;

        private float _verticalThicknessOfSideTie;
        private float _widthOfSideOfTiePastKnotFromEdge;
        private float _sideTieHeightVariation;
        private float _topPointOfTieKnotFromEdge_X;
        private float _topPointOfTieKnotFromTop_Y;
        private float _bottomPointOfTieKnotFromTop_Y;
        private float _bottomPointOfTieKnotFromEdge_X;
        private float _cornerRadiusOfOutsideBorder;
        private float _gapBetweenTieAndSuitJacket;
        private float _widthOfTieAtWidestPointFromEdge_X;
        private float _waistFromEdge_X;
        private float _armpitFromEdge_X;
        private float _armpitFromTop_Y;
        private float _insideCuffFromEdge_X;
        private float _outsideCuffFromEdge_X;
        private float _shoulderFromEdge_X;
        private float _shoulderFromTop_Y;
        private float _insideLapelFromEdge_X;
        private float _topButtonFromTopEdge_Y;

        private SKPoint _bottomPointOfTie;

        SKPaint _suitJacketFillPaint = new SKPaint()
        {
            Color = SKColors.Navy,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
        };

        SKPaint _logoStrokePaint = new SKPaint()
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
        };

        SKPaint _tieFillPaint = new SKPaint()
        {
            Color = SKColors.Crimson,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };
                                
        SKPath _suitJacketPath = new SKPath();
        SKPath _tieKnotPath = new SKPath();
        SKPath _mainTiePath = new SKPath();
        SKPath _sidePartOfTiePath = new SKPath();

        NamePlacard _namePlacard;
        Desk _desk;
        LogoBackground _logoBackground;

        public Logo(SKCanvas canvas, SKRect position)
        {
            _canvas = canvas;
            _position = position;
            InitFields();
            InitPath();
            DrawPath();
        }
        private void InitFields()
        {
            var s = _position.Width;
            _cornerRadiusOfOutsideBorder = _cornerRadiusOfOutsideBorderRatio * s;
            _desk = new Desk(_position, _cornerRadiusOfOutsideBorder);
            _namePlacard = new NamePlacard(_position, _desk.TopOfDesk);
            
            _shoulderFromTop_Y = s * _topOfShoulderRatioFromTop;
            _waistFromEdge_X = s * _waistFromVerticalEdgeRatio;
            _armpitFromEdge_X = s * _armpitFromVerticalEdgeRatio;
            _armpitFromTop_Y = _shoulderFromTop_Y + (_verticalThicknessShoulderRatio * s);
            _insideCuffFromEdge_X = s * _cuffInsideEdgeFromVerticalEdgeRatio;
            _outsideCuffFromEdge_X = s * _cuffOutsideEdgeFromVerticalEdgeRatio;
            _shoulderFromEdge_X = s * _outsideEdgeOfShoulderRatio;
            
            _insideLapelFromEdge_X = s * _insideLapelFromVerticalEdgeRatio;
            _topButtonFromTopEdge_Y = s * _topButtonFromTopRatio;

            _topPointOfTieKnotFromEdge_X = _topPointOfTieKnotFromEdgeXRatio * s;
            _topPointOfTieKnotFromTop_Y = _shoulderFromTop_Y - (_topPointOfTieKnotFromTopOfShoulderYRatio * s);
            
            _bottomPointOfTieKnotFromEdge_X = _bottomPointOfTieKnotFromEdgeXRatio * s;
            _bottomPointOfTieKnotFromTop_Y = _topPointOfTieKnotFromTop_Y + (_verticalThicknessOfTieKnotRatio * s);
                        
            _gapBetweenTieAndSuitJacket = _gapBetweenTieAndSuitJacketRatio * s;
            _widthOfTieAtWidestPointFromEdge_X = _widthOfTieAtWidestPointFromEdgeXRatio * s;

            _verticalThicknessOfSideTie = _verticalThicknessOfTieKnotRatio * s * _verticalThicknessOfSideTieRatio;
            _widthOfSideOfTiePastKnotFromEdge = _widthOfSideOfTiePastKnotFromEdgeRatio * s;
            _sideTieHeightVariation = _sideTieHeightVariationRatio * s;
            _logoStrokePaint.StrokeWidth = _strokeWidthRatio * s;

            _logoBackground = new LogoBackground(_position, _cornerRadiusOfOutsideBorder, _logoStrokePaint.StrokeWidth);
            _bottomPointOfTie = new SKPoint(_position.MidX, (_topButtonFromTopEdge_Y - _gapBetweenTieAndSuitJacket + _position.Top));       
        }
        private void InitPath()
        {
            var p = _position;
            float bottom = _desk.TopOfDesk;
            float top = p.Top + _shoulderFromTop_Y;

            int L = 1;
            int R = 0;

            for(int side = 1; side > -2; side -= 2)
            {
                _suitJacketPath.MoveTo(new SKPoint(p.MidX, bottom));

                _suitJacketPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _waistFromEdge_X)), bottom));

                _suitJacketPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _armpitFromEdge_X)), (p.Top + _armpitFromTop_Y)));

                _suitJacketPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _insideCuffFromEdge_X)), bottom));

                _suitJacketPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _outsideCuffFromEdge_X)), bottom));

                _suitJacketPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _shoulderFromEdge_X)), top));

                _suitJacketPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _insideLapelFromEdge_X)), top));

                _suitJacketPath.LineTo(new SKPoint(p.MidX, (p.Top + _topButtonFromTopEdge_Y)));
               
                // Tie Knot
                _tieKnotPath.MoveTo(new SKPoint(p.MidX, _bottomPointOfTieKnotFromTop_Y + p.Top));

                _tieKnotPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _bottomPointOfTieKnotFromEdge_X)), 
                    (p.Top + _bottomPointOfTieKnotFromTop_Y)));

                _tieKnotPath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _topPointOfTieKnotFromEdge_X)),
                    (p.Top + _topPointOfTieKnotFromTop_Y)));

                _tieKnotPath.LineTo(new SKPoint(p.MidX, p.Top + _topPointOfTieKnotFromTop_Y));

                // main part of tie
                _mainTiePath.MoveTo(_bottomPointOfTie);

                _mainTiePath.LineTo(GetMainTieSidePoint(side));

                _mainTiePath.LineTo(
                    (p.Left * L + p.Right * R + (side * _bottomPointOfTieKnotFromEdge_X)),
                    (p.Top + _bottomPointOfTieKnotFromTop_Y));

                _mainTiePath.LineTo(p.MidX, (p.Top + _bottomPointOfTieKnotFromTop_Y));

                // side of Tie
                _sidePartOfTiePath.MoveTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _topPointOfTieKnotFromEdge_X)),
                    (p.Top + _topPointOfTieKnotFromTop_Y)));

                _sidePartOfTiePath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _widthOfSideOfTiePastKnotFromEdge)),
                    (p.Top + _topPointOfTieKnotFromTop_Y - _sideTieHeightVariation)));

                _sidePartOfTiePath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _widthOfSideOfTiePastKnotFromEdge)),
                    (p.Top + _topPointOfTieKnotFromTop_Y + _verticalThicknessOfSideTie)));

                _sidePartOfTiePath.LineTo(new SKPoint(
                    (p.Left * L + p.Right * R + (side * _bottomPointOfTieKnotFromEdge_X)),
                    (p.Top + _topPointOfTieKnotFromTop_Y + _verticalThicknessOfSideTie)));

                _sidePartOfTiePath.Close();

                L = 0;
                R = 1;
            }
            SKPoint GetMainTieSidePoint(int side)
            {
                double opposite = _position.MidX - (_insideLapelFromEdge_X + _position.Left);
                double adjacent = _topButtonFromTopEdge_Y - _shoulderFromTop_Y;
                double radians = Math.Atan2(opposite, adjacent);
                double angle = radians * (180 / Math.PI);
                double halfWidthTie = _position.MidX - _position.Left - _widthOfTieAtWidestPointFromEdge_X;

                float x = _position.Left * L + _position.Right * R + (side * _widthOfTieAtWidestPointFromEdge_X);
                float y = (float)(_topButtonFromTopEdge_Y - (halfWidthTie / Math.Tan(radians)) - _gapBetweenTieAndSuitJacket + _position.Top);

                return new SKPoint(x,y);
            }
        }
        private void DrawPath()
        {
            _canvas.DrawRoundRect(_logoBackground.RoundRect, _logoBackground.Fill);
            _canvas.DrawRoundRect(_logoBackground.RoundRect, _logoStrokePaint);

            _canvas.DrawPath(_suitJacketPath, _suitJacketFillPaint);
            _canvas.DrawPath(_suitJacketPath, _logoStrokePaint);
            _canvas.DrawPath(_tieKnotPath, _tieFillPaint);
            _canvas.DrawPath(_tieKnotPath, _logoStrokePaint);
            _canvas.DrawPath(_mainTiePath, _tieFillPaint);
            _canvas.DrawPath(_mainTiePath, _logoStrokePaint);
            _canvas.DrawPath(_sidePartOfTiePath, _tieFillPaint);
            _canvas.DrawPath(_sidePartOfTiePath, _logoStrokePaint);

            // Desk
            _canvas.DrawRect(_desk.TopSurface, _desk.DeskPaint);
            _canvas.DrawRect(_desk.TopSurface, _logoStrokePaint);
            _canvas.DrawRect(_desk.Base, _desk.DeskPaint);
            _canvas.DrawRect(_desk.Base, _logoStrokePaint);

            // Name Placard
            _canvas.DrawRect(_namePlacard.SKRect, _namePlacard.Paint);
            _canvas.DrawRect(_namePlacard.SKRect, _logoStrokePaint);
            _canvas.DrawText(_namePlacard.Text, _namePlacard.TextPositionPoint, _namePlacard.TextPaint);

            _canvas.Save();
        }        
    }    
}
