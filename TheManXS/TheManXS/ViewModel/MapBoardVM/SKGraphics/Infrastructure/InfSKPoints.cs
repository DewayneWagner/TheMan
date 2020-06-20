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

        public InfSKPoints(SQ sq, IT it)
        {
            if (_game == null) { _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)]; }
            _sq = sq;
            _it = it;

            _infCorridorWidth = InfCorridorWidthRatio * QC.SqSize;
            _incrementFromEdge = (_infCorridorWidth / 4) * ((int)it + 1);
            _distanceFromHorizontalEdgeWater = _infCorridorWidth / 2;
            _distanceFromVerticalEdgeWater = _infCorridorWidth;
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

        public SKPoint SE
        {
            get
            {
                float x = _sq.SKRect.Right - _incrementFromEdge;
                float y = _sq.SKRect.Bottom - _incrementFromEdge;
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
