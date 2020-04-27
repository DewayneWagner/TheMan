using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures
{
    class LowDensity
    {
        private const int NumberOfHousesPerSQ = 4; // must be perfect square, minimum of 4
        private int _numberOfHousesPerSide = (int)Math.Sqrt(NumberOfHousesPerSQ);
        private float _numberOfStreetsEachDirection = (NumberOfHousesPerSQ / 2 - 1);

        private float _left;
        private float _right;
        private float _top;
        private float _bottom;
        private float _streetWidth;
        private float _houseDistanceFromEdge;
        private float _interiorBlockSideSize;

        private float _streetWidthSizeRatio = 0.05f;
        private float _houseDistanceFromEdgeRatio = 0.1f;

        Game _game;
        SQ _sq;

        SKRect _backgroundRect;
        SKPaint _backgroundPaint;
        SKPath _streetspath;
        SKPaint _streetsPaint;

        public LowDensity(Game game, SQ sq)
        {
            _game = game;
            _sq = sq;

            InitFields();
            CreateBackgroundSKRectWithOwnerColor();
            DrawStreets();            
            PaintEverything();
            InitHouses();
        }
        private void InitFields()
        {
            _left = _sq.Col * QC.SqSize;
            _right = (_sq.Col + 1) * QC.SqSize;
            _top = _sq.Row * QC.SqSize;
            _bottom = (_sq.Row + 1) * QC.SqSize;

            _streetWidth = _streetWidthSizeRatio * QC.SqSize;
            _houseDistanceFromEdge = (_houseDistanceFromEdgeRatio * (QC.SqSize - _streetWidth * (NumberOfHousesPerSQ/2 - 1)));

            _interiorBlockSideSize = (QC.SqSize 
                - (_numberOfHousesPerSide * 2 * _houseDistanceFromEdge)
                - (_streetWidth * _numberOfStreetsEachDirection)) 
                / (_numberOfHousesPerSide);

            
        }
        private void CreateBackgroundSKRectWithOwnerColor()
        {
            _backgroundRect = new SKRect(_left, _top, _right, _bottom);
            _backgroundPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = _game.PlayerList[_sq.OwnerNumber].SKColor,
            };
        }
        private void DrawStreets()
        {
            int numberOfStreetsEachDirection = (int)Math.Sqrt(NumberOfHousesPerSQ) - 1;
            float distanceBetweenRoads = QC.SqSize / (numberOfStreetsEachDirection + 1);
            _streetspath = new SKPath();

            initStreetsPaint();
            initHorizontalRoads();
            initVerticalRoads();

            void initStreetsPaint()
            {
                _streetsPaint = new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = _streetWidth,
                    Color = SKColors.Black
                };
            }            
            void initHorizontalRoads()
            {                
                for (int i = 1; i <= numberOfStreetsEachDirection; i++)
                {
                    float Y = _top + (i * distanceBetweenRoads);
                    _streetspath.MoveTo(new SKPoint(_left, Y));
                    _streetspath.LineTo(new SKPoint(_right, Y));
                }
            }
            void initVerticalRoads()
            {
                for (int i = 1; i <= numberOfStreetsEachDirection; i++)
                {
                    float X = _left + (i * distanceBetweenRoads);
                    _streetspath.MoveTo(new SKPoint(X, _top));
                    _streetspath.LineTo(new SKPoint(X, _bottom));
                }
            }
        }
        private void InitHouses()
        {
            int numberOfHousesEachDirection = (NumberOfHousesPerSQ / 2);

            for (int row = 0; row < numberOfHousesEachDirection; row++)
            {
                for (int col = 0; col < numberOfHousesEachDirection; col++)
                {
                    float left = _left +
                        _houseDistanceFromEdge + 
                        (_houseDistanceFromEdge * col * 2) + 
                        (_streetWidth * col) + 
                        (_interiorBlockSideSize * col);
                    float right = left + _interiorBlockSideSize;

                    float top = _top +
                        _houseDistanceFromEdge +
                        (_houseDistanceFromEdge * row * 2) +
                        (_streetWidth * row) +
                        (_interiorBlockSideSize * row);
                    float bottom = top + _interiorBlockSideSize;

                    SKRect rect = new SKRect(left, top, right, bottom);
                    new House(rect, _game);
                }
            }
        }
        private void PaintEverything()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                canvas.DrawRect(_backgroundRect, _backgroundPaint);
                canvas.DrawPath(_streetspath, _streetsPaint);
                canvas.Save();
            }
        }
    }
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
