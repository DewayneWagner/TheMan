using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Logos
{
    class Desk
    {
        SKColor _darkWalnut = new SKColor(121, 77, 46);
        private float _verticalThicknessRatio = 0.075f;
        private float _startPointFromEdgeRatio = 0.01f;

        private float _verticalThickness;
        private float _startPointFromEdge;
        private SKRect _logoRect;
        private float _radiusOfCorners;

        public Desk(SKRect logoRect, float radiusOfCorners)
        {
            _verticalThickness = _verticalThicknessRatio * logoRect.Width;
            _logoRect = logoRect;
            _startPointFromEdge = _startPointFromEdgeRatio * _logoRect.Width;
            _radiusOfCorners = radiusOfCorners;
        }
        public SKPaint DeskPaint
        {
            get
            {
                return new SKPaint()
                {
                    IsAntialias = true,
                    Color = _darkWalnut,
                    Style = SKPaintStyle.Fill,
                };
            }
        }
        public float TopOfDesk => (_logoRect.Bottom - _radiusOfCorners - _verticalThickness);
        public SKRect TopSurface
        {
            get
            {
                float left = _logoRect.Left + _startPointFromEdge;
                float right = _logoRect.Right - _startPointFromEdge;
                float bottom = TopOfDesk + _verticalThickness;

                return new SKRect(left, TopOfDesk, right, bottom);
            }
        }
        public SKRect Base
        {
            get
            {
                float left = _logoRect.Left + _radiusOfCorners;
                float top = TopSurface.Bottom;
                float right = _logoRect.Right - _radiusOfCorners;
                float bottom = _logoRect.Bottom - _startPointFromEdge;

                return new SKRect(left, top, right, bottom);
            }
        }
    }
}
