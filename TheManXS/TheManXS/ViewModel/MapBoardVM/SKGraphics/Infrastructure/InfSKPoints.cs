using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Infrastructure
{
    class InfSKPoints
    {
        SQ _sq;
        IT _it;
        private static Game _game;

        private const float InfCorridorWidthRatio = 0.1f;
        private float _infCorridorWidth;

        private float _incrementFromEdge;
        private float _distanceFromLeftEdgeForTieInPoint;
        private float _distanceFromHorizontalEdgeWater;
        private float _distanceFromVerticalEdgeWater;

        private int _water;
        private int _notWater;

        public InfSKPoints() { }
        // constructor for Squares
        public InfSKPoints(SQ sq, IT it)
        {
            if (_game == null) { _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)]; }
            _sq = sq;
            _it = it;

            _infCorridorWidth = InfCorridorWidthRatio * QC.SqSize;
            _incrementFromEdge = (_infCorridorWidth / 4) * ((int)it + 1);
            _distanceFromHorizontalEdgeWater = _infCorridorWidth / 2;
            _distanceFromVerticalEdgeWater = _infCorridorWidth;

            _water = it == IT.MainRiver || it == IT.Tributary ? 1 : 0;
            _notWater = _water == 1 ? 0 : 1;
        }

        // constructor for tie-in points
        public InfSKPoints(SQ sq, IT it, bool isForTieInPoint)
        {
            _sq = sq;
            _it = it;
            _distanceFromLeftEdgeForTieInPoint = sq.SKRect.MidX + ((int)it - 1);
        }

        // is only used for river or tributary
        public SKPoint NW
        {
            get
            {
                float x = _sq.SKRect.Left + _distanceFromVerticalEdgeWater;
                float y = _sq.SKRect.Top + _distanceFromHorizontalEdgeWater;
                return new SKPoint(x, y);
            }
        }

        // can be used for any infrastructure
        public SKPoint NE
        {
            get
            {
                float x = (_sq.SKRect.Right - _incrementFromEdge) * _notWater + (_sq.SKRect.Right - _distanceFromVerticalEdgeWater) * _water;
                float y = (_sq.SKRect.Top + _incrementFromEdge) * _notWater + (_sq.SKRect.Right - _distanceFromVerticalEdgeWater) * _water;
                return new SKPoint(x, y);
            }
        }

        public SKPoint SE
        {
            get
            {
                float x = _sq.SKRect.Right - _incrementFromEdge;
                float y = _sq.SKRect.Bottom - _incrementFromEdge;
                return new SKPoint(x, y);
            }
        }

        public SKPoint SW
        {
            get
            {
                float x = _sq.SKRect.Left + (_incrementFromEdge * _notWater) + (_distanceFromVerticalEdgeWater * _water);
                float y = _sq.SKRect.Bottom - (_incrementFromEdge * _notWater) + (_distanceFromHorizontalEdgeWater * _water);
                return new SKPoint(x, y);
            }
        }

        public SKPoint TieInPoint
        {
            get
            {
                float x = _sq.SKRect.Left + _distanceFromLeftEdgeForTieInPoint;
                float y = _sq.SKRect.Bottom - _incrementFromEdge;
                return new SKPoint(x, y);
            }
        }

        public SKPoint TeeToTieInPoint
        {
            get
            {
                float x = _sq.SKRect.Left + _distanceFromLeftEdgeForTieInPoint;
                float y = _sq.SKRect.Bottom - _incrementFromEdge;
                return new SKPoint(x, y);
            }
        }
    }    
}
