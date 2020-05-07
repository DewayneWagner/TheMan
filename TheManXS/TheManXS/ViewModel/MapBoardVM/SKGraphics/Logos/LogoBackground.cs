using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos
{
    class LogoBackground
    {
        private SKRect _rect;
        private float _radiusOfCorners;
        private float _strokeThickness;

        public LogoBackground(SKRect rect, float radiusOfCorners, float strokeThickness)
        {
            _rect = rect;
            _radiusOfCorners = radiusOfCorners;
            _strokeThickness = strokeThickness;
        }

        public SKPaint Stroke
        {
            get
            {
                return new SKPaint()
                {
                    Color = SKColors.Black,
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = _strokeThickness,
                };
            }
        }

        public SKPaint Fill
        {
            get
            {
                return new SKPaint()
                {
                    Color = new SKColor(109, 124, 135),
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                };
            }
        }
        public SKRoundRect RoundRect => new SKRoundRect(_rect, _radiusOfCorners, _radiusOfCorners);
    }
}
