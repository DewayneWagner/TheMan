using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos
{
    class NamePlacard
    {
        private const float _widthRatio = 0.4f;
        private const float _heightRatio = 0.1f;
        private const float _textHeightRatio = 0.75f;
        private const float _distanceFromLeftEdgeRatio = 0.55f;

        private float _left;
        private float _widthOfPlacard;
        private float _heightOfPlacard;
        private float _textHeight;
        private float _yCoordTopOfTable;
        public NamePlacard(SKRect logoRect, float yCoordTopOfTable)
        {
            _yCoordTopOfTable = yCoordTopOfTable;
            _left = _distanceFromLeftEdgeRatio * logoRect.Width + logoRect.Left;
            _widthOfPlacard = logoRect.Width * _widthRatio;
            _heightOfPlacard = logoRect.Width * _heightRatio;
            _textHeight = _heightOfPlacard * _textHeightRatio;
        }
        public SKPaint Paint
        {
            get
            {
                return new SKPaint()
                {
                    IsAntialias = true,
                    Color = SKColors.Gold,
                    Style = SKPaintStyle.Fill,                    
                };
            }
        }
        public SKPaint TextPaint
        {
            get
            {
                return new SKPaint()
                {
                    IsAntialias = true,
                    Color = SKColors.Black,
                    TextSize = _textHeight,
                    TextAlign = SKTextAlign.Center,
                    FakeBoldText = true,
                    Style = SKPaintStyle.Stroke,
                };
            }
        }
        public SKRect SKRect
        {
            get
            {
                float left = _left;
                float top = _yCoordTopOfTable - _heightOfPlacard;
                float right = _left + _widthOfPlacard;
                float bottom = _yCoordTopOfTable;
                return new SKRect(left, top, right, bottom);
            }
        }
        public string Text => "THE MAN";
        public SKPoint TextPositionPoint
        {
            get
            {
                float x = SKRect.MidX;
                float y = SKRect.Bottom - (_heightOfPlacard - _textHeight);
                return new SKPoint(x, y);
            }
        }
    }
}
