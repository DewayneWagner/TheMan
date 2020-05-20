using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Structures.CityStructures
{
    abstract class CityStructure
    {
        protected const int NumberOfStreetsHorizontal = 1;
        protected const int NumberOfStreetsVertical = 1;

        protected const int NumberOfHouseRowsPerBlock = 3;
        protected const int NumberOfHouseColumnsPerBlock = 3;
        protected const int NumberOfLowRiseBuildingsPerRowPerBlock = 2;
        protected const int NumberOfLowRiseBuildingsPerColumnPerBlock = 1;
        protected const int NumberOfHighRiseBuildingsPerRowPerBlock = 1;
        protected const int NumberOfHighRiseBuildingsPerColumnPerBlock = 2;

        private float _widthOfStreetRatio = 0.1f;
        private float _dashLengthRatio = 0.01f;

        private float _widthOfStreet;
        private SKPaint _streetPaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
        };
        private SKPaint _dottedYellowLinePaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Yellow,
            StrokeCap = SKStrokeCap.Butt,
        };
        private SKPaint _backgroundColor = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
        };

        private List<SKPath> _streetPaths = new List<SKPath>();
        private SKRect _position;

        public CityStructure(SKCanvas canvas, SKRect position, SKColor companyColor)
        {
            Canvas = canvas;
            _position = position;

            InitFields(companyColor);
            InitBackGround();
            PaintStreets();
        }

        protected SKCanvas Canvas { get; set; }
        protected float BlockSizeVertical
        {
            get
            {
                return (_position.Height - _widthOfStreet * NumberOfStreetsVertical) 
                    / (NumberOfStreetsVertical + 1);                
            }
        }
        protected float BlockSizeHorizontal
        {
            get
            {
                return (_position.Width - _widthOfStreet * NumberOfStreetsHorizontal) /
                    (NumberOfStreetsHorizontal + 1);
            }
        }

        protected List<SKRect> CityBlocks
        {
            get
            {
                List<SKRect> cityBlocks = new List<SKRect>();

                float left, top, right, bottom;

                for (int row = 0; row <= NumberOfStreetsVertical; row++)
                {
                    for (int col = 0; col <= NumberOfStreetsHorizontal; col++)
                    {
                        left = _position.Left + BlockSizeHorizontal * col + _widthOfStreet * col;
                        top = _position.Top + BlockSizeVertical * row + _widthOfStreet * row;
                        right = left + BlockSizeHorizontal;
                        bottom = top + BlockSizeVertical;

                        cityBlocks.Add(new SKRect(left, top, right, bottom));
                    }
                }

                return cityBlocks;
            }
        }
        private SKPath StreetPaths
        {
            get
            {
                SKPath streetPaths = new SKPath();

                for (int row = 0; row < NumberOfStreetsVertical; row++)
                {
                    float y = _position.Top + BlockSizeVertical * (row + 1) + _widthOfStreet * row + _widthOfStreet / 2;

                    streetPaths.MoveTo(new SKPoint(_position.Left, y));
                    streetPaths.LineTo(new SKPoint(_position.Right, y));
                }

                for (int col = 0; col < NumberOfStreetsHorizontal; col++)
                {
                    float x = _position.Left + BlockSizeHorizontal * (col + 1) + _widthOfStreet * col + _widthOfStreet / 2;

                    streetPaths.MoveTo(new SKPoint(x, _position.Top));
                    streetPaths.LineTo(new SKPoint(x, _position.Bottom));
                }

                return streetPaths;
            }
        }

        private void InitFields(SKColor companyColor)
        {
            _streetPaint.StrokeWidth = _widthOfStreet = _widthOfStreetRatio * _position.Width;

            float phase = 0; // this variable makes no perceptible difference???
            _dottedYellowLinePaint.PathEffect = SKPathEffect.CreateDash(getDashIntervals(), phase);

            _backgroundColor.Color = companyColor.WithAlpha(0x50);
        }

        private void InitBackGround() => Canvas.DrawRect(_position, _backgroundColor);

        private float[] getDashIntervals()
        {
            float dashLength = _dashLengthRatio * _position.Width;
            int qDashes = (int)(_position.Width / dashLength);

            float[] dashArray = new float[qDashes];
            int index = 0;

            for (int  d = 0;  d < (qDashes / 2);  d += 2)
            {
                dashArray[index] = dashLength;
                index++;
            }

            return dashArray;
        }

        private void PaintStreets()
        {
            Canvas.DrawPath(StreetPaths, _streetPaint);
            Canvas.DrawPath(StreetPaths, _dottedYellowLinePaint);
            Canvas.Save();
        }
    }
}
