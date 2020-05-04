using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.City
{
    class House
    {
        Game _game;
        SKRect _rectangleWhereHouseIsToBePlaced;

        private float _roofDistanceFromEdgeRatio = 0.05f;
        private float _distanceFromEdgeToSideOfMainFloorRatio = 0.2f;

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

        public House(SKRect rectangleWhereHouseIsToBePlaced, Game game)
        {
            _game = game;
            _rectangleWhereHouseIsToBePlaced = rectangleWhereHouseIsToBePlaced;

            InitFields();
            InitRoofTriangle();
            InitMainFloor();
            PaintHouse();
        }

        private void InitFields()
        {
            _roofDistanceFromEdge = QC.SqSize * _roofDistanceFromEdgeRatio;
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

        private void PaintHouse()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                canvas.DrawPath(_roofTriangle, _buildingPaint);
                canvas.DrawRect(_mainFloorRect, _buildingPaint);
                canvas.Save();
            }
        }
    }
}
