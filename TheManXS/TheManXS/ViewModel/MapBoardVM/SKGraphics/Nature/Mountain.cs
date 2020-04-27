using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature
{
    class Mountain
    {
        Game _game;
        SQ _sq;

        private float _startingPointFromLeftRatio = 0.1f;
        private float _startingPointFromTopRatio = 0.9f;
        private float _leftPeakFromLeftRatio = 0.3f;
        private float _leftPeakFromTopRatio = 0.1f;
        private float _valleyBetweenPeaksFromTopRatio = 0.4f;
        private float _rightPeakFromLeftRatio = 0.66f;
        private float _rightPeakFromTopRatio = 0.3f;
        private float _widthOfLineRatio = 0.02f;

        private float _startingPointFromLeft;
        private float _startingPointFromTop;
        private float _leftPeakFromLeft;
        private float _leftPeakFromTop;
        private float _valleyBetweenPeaksFromTop;
        private float _valleyBetweenPeaksFromLeft;
        private float _rightPeakFromLeft;
        private float _rightPeakFromTop;

        private float left, top, right;

        private SKPaint _mountainPaint = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
        };

        private SKPath _mountainPath = new SKPath();
        SKPoint _firstPointOnMountainPath;
        SKPoint _lastPointOnMountainPath;
        SKPoint _peakOfMountainPath;
        System.Random rnd = new System.Random();

        public Mountain(Game game, SQ sq)
        {
            _game = game;
            _sq = sq;

            InitFields();
            InitPath();
            SetGradient();                       
            PaintMountain();
        }
        void InitFields()
        {
            _startingPointFromLeft = _startingPointFromLeftRatio * QC.SqSize;
            _startingPointFromTop = _startingPointFromTopRatio * QC.SqSize;
            _leftPeakFromLeft = _leftPeakFromLeftRatio * QC.SqSize;
            _leftPeakFromTop = _leftPeakFromTopRatio * QC.SqSize;
            _rightPeakFromLeft = _rightPeakFromLeftRatio * QC.SqSize;
            _rightPeakFromTop = _rightPeakFromTopRatio * QC.SqSize;
            _valleyBetweenPeaksFromLeft = (_rightPeakFromLeft - _leftPeakFromLeft) / 2 + _leftPeakFromLeft;
            _valleyBetweenPeaksFromTop = _valleyBetweenPeaksFromTopRatio * QC.SqSize;
            _mountainPaint.StrokeWidth = _widthOfLineRatio * QC.SqSize;

            left = _sq.Col * QC.SqSize;
            top = _sq.Row * QC.SqSize;
            right = (_sq.Col + 1) * QC.SqSize;
        }
        void InitPath()
        {
            _firstPointOnMountainPath = new SKPoint((left + _startingPointFromLeft), (top + _startingPointFromTop));
            _lastPointOnMountainPath = new SKPoint((right - _startingPointFromLeft), (top + _startingPointFromTop));
            _peakOfMountainPath = new SKPoint((left + _rightPeakFromLeft), (top + _rightPeakFromTop));

            _mountainPath.MoveTo(_firstPointOnMountainPath);
            _mountainPath.LineTo(new SKPoint((left + _leftPeakFromLeft), (top + _leftPeakFromTop)));
            _mountainPath.LineTo(new SKPoint((left + _valleyBetweenPeaksFromLeft), (top + _valleyBetweenPeaksFromTop)));
            _mountainPath.LineTo(_peakOfMountainPath);
            _mountainPath.LineTo(_lastPointOnMountainPath);
            _mountainPath.Close();
        }
        void PaintMountain()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                canvas.DrawPath(_mountainPath, _mountainPaint);
                canvas.Save();
            }
        }
        void SetGradient()
        {
            float halfSQ = QC.SqSize / 2;
            //SKPoint center = new SKPoint((_sq.Col * QC.SqSize + halfSQ), (_sq.Row * QC.SqSize + halfSQ));
            SKColor[] colors = getSKColorArray();

            float x = ((_lastPointOnMountainPath.X - _firstPointOnMountainPath.X) / 2) + _firstPointOnMountainPath.X;
            float y = ((_lastPointOnMountainPath.Y - _firstPointOnMountainPath.Y) / 2) + _firstPointOnMountainPath.Y;

            SKPoint gradientStartPoint = new SKPoint(x, y);
            float distanceBetweenStartGradientAndPeak = gradientStartPoint.Y - _peakOfMountainPath.Y;
            float[] colorPosition = getColorPositionArray();

            _mountainPaint.Shader = SKShader.CreateLinearGradient(gradientStartPoint,
                _peakOfMountainPath,
                colors,
                colorPosition,
                SKShaderTileMode.Clamp);

            SKColor[] getSKColorArray()
            {
                SKColor[] colorArray = new SKColor[4];

                colorArray[0] = _game.PaletteColors
                    .Where(c => c.Description == "Banff 2")
                    .Select(c => c.SKColor)
                    .FirstOrDefault();

                colorArray[1] = _game.PaletteColors
                    .Where(c => c.Description == "Banff 5")
                    .Select(c => c.SKColor)
                    .FirstOrDefault();

                colorArray[2] = _game.PaletteColors
                    .Where(c => c.Description == "Banff 1")
                    .Select(c => c.SKColor)
                    .FirstOrDefault();

                colorArray[3] = SKColors.GhostWhite;

                return colorArray;
            }
            float[] getColorPositionArray()
            {
                float[] a = new float[4]
                {
                    0.2f, 0.4f, 0.6f, 0.8f,
                };
                return a;
            }
        }
    }
}
