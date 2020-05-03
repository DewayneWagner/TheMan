using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains
{
    class TwoPeakMountain : Mountain
    {
        private const float _startingPointFromLeftRatio = 0.1f;
        private const float _startingPointFromTopRatio = 0.9f;
        private const float _leftPeakFromLeftRatio = 0.3f;
        private const float _leftPeakFromTopRatio = 0.1f;
        private const float _valleyBetweenPeaksFromTopRatio = 0.4f;
        private const float _rightPeakFromLeftRatio = 0.66f;
        private const float _rightPeakFromTopRatio = 0.3f;

        private static bool _staticFieldsHaveBeenInitialized;
        private static float _startingPointFromLeft;
        private static float _startingPointFromTop;
        private static float _leftPeakFromLeft;
        private static float _leftPeakFromTop;
        private static float _valleyBetweenPeaksFromTop;
        private static float _valleyBetweenPeaksFromLeft;
        private static float _rightPeakFromLeft;
        private static float _rightPeakFromTop;
        private static int _sqSize;
        
        SKPoint _firstPointOnMountainPath;
        SKPoint _lastPointOnMountainPath;

        public TwoPeakMountain(SKRect rect) : base(rect)
        {
            if (!_staticFieldsHaveBeenInitialized)
            {
                _sqSize = QC.SqSize;
                InitFields();
                _staticFieldsHaveBeenInitialized = true;
            }
        }

        public override SKPath MountainPath
        {
            get
            {
                SKPath mountainPath = new SKPath();
                _firstPointOnMountainPath = new SKPoint((SKRectSQ.Left + _startingPointFromLeft), (SKRectSQ.Top + _startingPointFromTop));
                _lastPointOnMountainPath = new SKPoint((SKRectSQ.Right - _startingPointFromLeft), (SKRectSQ.Top + _startingPointFromTop));
                TopPoint = new SKPoint((SKRectSQ.Left + _rightPeakFromLeft), (SKRectSQ.Top + _rightPeakFromTop));

                mountainPath.MoveTo(_firstPointOnMountainPath);
                mountainPath.LineTo(new SKPoint((SKRectSQ.Left + _leftPeakFromLeft), (SKRectSQ.Top + _leftPeakFromTop)));
                mountainPath.LineTo(new SKPoint((SKRectSQ.Left + _valleyBetweenPeaksFromLeft), (SKRectSQ.Top + _valleyBetweenPeaksFromTop)));
                mountainPath.LineTo(TopPoint);
                mountainPath.LineTo(_lastPointOnMountainPath);
                mountainPath.Close();

                return mountainPath;
            }
        }

        void InitFields()
        {
            _startingPointFromLeft = _startingPointFromLeftRatio * _sqSize;
            _startingPointFromTop = _startingPointFromTopRatio * _sqSize;
            _leftPeakFromLeft = _leftPeakFromLeftRatio * _sqSize;
            _leftPeakFromTop = _leftPeakFromTopRatio * _sqSize;
            _rightPeakFromLeft = _rightPeakFromLeftRatio * _sqSize;
            _rightPeakFromTop = _rightPeakFromTopRatio * _sqSize;
            _valleyBetweenPeaksFromLeft = (_rightPeakFromLeft - _leftPeakFromLeft) / 2 + _leftPeakFromLeft;
            _valleyBetweenPeaksFromTop = _valleyBetweenPeaksFromTopRatio * _sqSize;
        }
    }
}
