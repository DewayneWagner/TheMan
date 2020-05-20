using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures
{
    class House
    {
        SKRect _rectangleWhereHouseIsToBePlaced;

        private float _roofDistanceFromEdgeRatio = 0.25f;
        private float _distanceFromEdgeToSideOfMainFloorRatio = 0.4f;

        private float _roofDistanceFromEdge;
        private float _topOfMainFloor;
        private float _distanceFromEdgeToSideOfMainFloor;

        private SKPaint _buildingPaint = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.Black,
            StrokeWidth = 3
        };

        private SKRect _mainFloorRect;
        private SKPath _roofTriangle;
        private SKRect _chimneyRect;

        public House(SKRect rectangleWhereHouseIsToBePlaced, SKCanvas canvas)
        {            
            _rectangleWhereHouseIsToBePlaced = rectangleWhereHouseIsToBePlaced;

            InitFields();
            InitRoofTriangle();
            InitMainFloor();
            PaintHouse(canvas);
        }

        private void InitFields()
        {
            _roofDistanceFromEdge = _rectangleWhereHouseIsToBePlaced.Width * _roofDistanceFromEdgeRatio;
            _distanceFromEdgeToSideOfMainFloor = _distanceFromEdgeToSideOfMainFloorRatio * _rectangleWhereHouseIsToBePlaced.Width;
            _topOfMainFloor = (_rectangleWhereHouseIsToBePlaced.Height / 2) + _rectangleWhereHouseIsToBePlaced.Top;
        }

        private void InitRoofTriangle()
        {

            SKPoint firstPoint = getFirstPoint();
            SKPoint secondPoint = getSecondPoint();
            SKPoint thirdPoint = getThirdPoint();

            _roofTriangle = new SKPath() { FillType = SKPathFillType.EvenOdd };
            _roofTriangle.MoveTo(firstPoint);
            _roofTriangle.LineTo(secondPoint);
            _roofTriangle.LineTo(thirdPoint);
            _roofTriangle.LineTo(firstPoint);
            _roofTriangle.Close();

            SKPoint getFirstPoint()
            {
                float x = _rectangleWhereHouseIsToBePlaced.Left + _roofDistanceFromEdge;
                float y = _topOfMainFloor;
                return new SKPoint(x, y);
            }
            SKPoint getSecondPoint()
            {
                float x = (_rectangleWhereHouseIsToBePlaced.Width / 2 + _rectangleWhereHouseIsToBePlaced.Left);
                float y = _rectangleWhereHouseIsToBePlaced.Top + _roofDistanceFromEdge;
                return new SKPoint(x, y);
            }
            SKPoint getThirdPoint()
            {
                float x = _rectangleWhereHouseIsToBePlaced.Right - _roofDistanceFromEdge;
                float y = _topOfMainFloor;
                return new SKPoint(x, y);
            }
        }
        private void InitMainFloor()
        {
            _mainFloorRect = new SKRect(_rectangleWhereHouseIsToBePlaced.Left + _distanceFromEdgeToSideOfMainFloor,
                _topOfMainFloor,
                _rectangleWhereHouseIsToBePlaced.Right - _distanceFromEdgeToSideOfMainFloor,
                _rectangleWhereHouseIsToBePlaced.Bottom - _roofDistanceFromEdge);
        }

        private void PaintHouse(SKCanvas canvas)
        {
            canvas.DrawPath(_roofTriangle, _buildingPaint);
            canvas.DrawRect(_mainFloorRect, _buildingPaint);
            canvas.Save();
        }
    }
}
