using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures
{
    class LowRiseBuilding
    {
        SKRect _position;
        private const float _sideOfBuildingFromEdgeRatio = 0.05f;
        private const float _topOfBuildingFromTopRatio = 0.05f;
        private const float _spaceBetweenWindowsHorizontalRatio = 0.025f;
        private const float _spaceBetweenWindowsVerticalRatio = 0.075f;

        private const int _numberOfWindowsWide = 10;
        private const int _numberOfWindowsHigh = 4;

        private float _sideOfBuildingFromEdge;
        private float _topOfBuildingFromTop;
        private float _buildingWidth;
        private float _buildingHeight;
        private float _windowWidth;
        private float _windowHeight;
        private float _spaceBetweenWindowsHorizontal;
        private float _spaceBetweenWindowsVertical;

        private SKPaint buildingPaint = new SKPaint()
        {
            IsAntialias = true,
            Color = SKColors.DarkRed,
            Style = SKPaintStyle.Fill,
        };

        private SKPaint windowPaint = new SKPaint()
        {
            IsAntialias = true,
            Color = SKColors.GhostWhite,
            Style = SKPaintStyle.Fill,
        };

        private SKPaint strokePaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            IsAntialias = true,
        };

        private List<SKRect> _listOfAllWindowRects = new List<SKRect>();
        private SKPath _outsideOfBuildingPath = new SKPath();
        private SKRect _buildingRect = new SKRect();
        public LowRiseBuilding(SKRect position, SKCanvas canvas)
        {
            _position = position;

            InitFields();
            InitOutsideOfBuildingRect();
            PaintEverything(canvas);
        }
        private void InitFields()
        {
            _sideOfBuildingFromEdge = _sideOfBuildingFromEdgeRatio * _position.Width;
            _topOfBuildingFromTop = _topOfBuildingFromTopRatio * _position.Width;
            _buildingWidth = _position.Width - _sideOfBuildingFromEdge * 2;
            _buildingHeight = _position.Height - _topOfBuildingFromTop * 2;
            _spaceBetweenWindowsHorizontal = _spaceBetweenWindowsHorizontalRatio * _position.Width;
            _spaceBetweenWindowsVertical = _spaceBetweenWindowsVerticalRatio * _position.Height;

            _windowHeight = (_buildingHeight - _spaceBetweenWindowsVertical * (_numberOfWindowsHigh + 1)) / _numberOfWindowsHigh;
            _windowWidth = (_buildingWidth - _spaceBetweenWindowsHorizontal * (_numberOfWindowsWide + 1)) / _numberOfWindowsWide;
        }
        private void InitOutsideOfBuildingRect()
        {
            float left = _position.Left + _sideOfBuildingFromEdge;
            float top = _position.Top + _topOfBuildingFromTop;
            float right = left + _buildingWidth;
            float bottom = top + _buildingHeight;

            _buildingRect = new SKRect(left, top, right, bottom);

            for (int windowRows = 1; windowRows <= _numberOfWindowsHigh; windowRows++)
            {
                for (int windowcols = 1; windowcols <= _numberOfWindowsWide; windowcols++)
                {
                    float leftW = _buildingRect.Left + _spaceBetweenWindowsHorizontal * windowcols + _windowWidth * (windowcols - 1);
                    float topW = _buildingRect.Top + _spaceBetweenWindowsVertical * windowRows + _windowHeight * (windowRows - 1);
                    float rightW = leftW + _windowWidth;
                    float bottomW = topW + _windowHeight;

                    _listOfAllWindowRects.Add(new SKRect(leftW, topW, rightW, bottomW));
                }
            }
        }
        
        private void PaintEverything(SKCanvas canvas)
        {
            canvas.DrawRect(_buildingRect, buildingPaint);

            foreach (SKRect window in _listOfAllWindowRects)
            {
                canvas.DrawRect(window, windowPaint);
            }

            canvas.Save();
        }
    }
}
