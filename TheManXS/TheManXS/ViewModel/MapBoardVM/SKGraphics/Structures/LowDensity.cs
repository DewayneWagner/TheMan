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

        private float _streetWidth;
        private float _houseDistanceFromEdge;
        private float _interiorBlockSideSize;

        private float _streetWidthSizeRatio = 0.05f;
        private float _houseDistanceFromEdgeRatio = 0.1f;

        Game _game;
        SQ _sq;

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
            _streetWidth = _streetWidthSizeRatio * QC.SqSize;
            _houseDistanceFromEdge = (_houseDistanceFromEdgeRatio * (QC.SqSize - _streetWidth * (NumberOfHousesPerSQ/2 - 1)));

            _interiorBlockSideSize = (QC.SqSize 
                - (_numberOfHousesPerSide * 2 * _houseDistanceFromEdge)
                - (_streetWidth * _numberOfStreetsEachDirection)) 
                / (_numberOfHousesPerSide);
        }
        private void CreateBackgroundSKRectWithOwnerColor()
        {
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
                    float Y = _sq.SKRect.Top  + (i * distanceBetweenRoads);
                    _streetspath.MoveTo(new SKPoint(_sq.SKRect.Left, Y));
                    _streetspath.LineTo(new SKPoint(_sq.SKRect.Right, Y));
                }
            }
            void initVerticalRoads()
            {
                for (int i = 1; i <= numberOfStreetsEachDirection; i++)
                {
                    float X = _sq.SKRect.Left + (i * distanceBetweenRoads);
                    _streetspath.MoveTo(new SKPoint(X, _sq.SKRect.Top));
                    _streetspath.LineTo(new SKPoint(X, _sq.SKRect.Bottom));
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
                    float left = _sq.SKRect.Left +
                        _houseDistanceFromEdge + 
                        (_houseDistanceFromEdge * col * 2) + 
                        (_streetWidth * col) + 
                        (_interiorBlockSideSize * col);
                    float right = left + _interiorBlockSideSize;

                    float top = _sq.SKRect.Top +
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
                canvas.DrawRect(_sq.SKRect, _backgroundPaint);
                canvas.DrawPath(_streetspath, _streetsPaint);
                canvas.Save();
            }
        }
    }
}
