using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using IC = TheManXS.Model.InfrastructureStuff.InfrastructureConstants;

namespace TheManXS.Model.InfrastructureStuff
{    
    public class InfrastructureSegment : BoxView
    {
        public enum InfrastructureOrientation { Straight, Diagonal }

        private SQ _fromSQ;
        private SQ _toSQ;
        private Direction _direction;
        private InfrastructureOrientation _orientation;
        private double _degreesPerIncrement = 45;

        // Starts from center of "fromSQ", goes in "Direction" to center of "toSQ"
        private enum Direction { N,NE,E,SE,S,SW,W,NW,Nada }
        public InfrastructureSegment(SQ fromSQ, SQ toSQ, InfrastructureType it)
        {
            _fromSQ = fromSQ;
            _toSQ = toSQ;

            SetDirection();
            SetOrientation();

            double x = fromSQ.Tile.X + (QC.SqSize / 2);
            double y = (-1) * (fromSQ.Row * QC.SqSize + GetY(it));

            RECT = new Rectangle(x, y, IC.Width, GetLength(_orientation));

            this.Rotation = GetRotation();
            this.BackgroundColor = GetColor(it);
            this.CornerRadius = IC.CornerRadius;
        }        
        public Rectangle RECT { get; set; }
        private double GetY(InfrastructureType it)
        {
            switch (it)
            {
                case InfrastructureType.MainTransporationCorridor:
                case InfrastructureType.Secondary:
                    return IC.RoadFromTopOfSQRatio * QC.SqSize;
                case InfrastructureType.Rail:
                    return IC.RailFromTopOfSQRatio * QC.SqSize;
                case InfrastructureType.Pipeline:
                    return IC.PipelineFromTopOfSQRatio * QC.SqSize;
                default:
                    return 0;
            }
        }
        private void SetDirection()
        {
            int rowChange = _fromSQ.Row - _toSQ.Row;
            int colChange = _fromSQ.Col - _toSQ.Col;

            if (rowChange == 0)
            {
                if (colChange > 0) { _direction = Direction.W; }
                else if (colChange < 0) { _direction = Direction.E; }
            }
            else if (rowChange > 0)
            {
                if (colChange > 0) { _direction = Direction.SW; }
                else if (colChange == 0) { _direction = Direction.S; }
                else if (colChange < 0) { _direction = Direction.SE; }
            }
            else if (rowChange < 0)
            {
                if (colChange > 0) { _direction = Direction.NW; }
                else if (colChange == 0) { _direction = Direction.N; }
                else if (colChange < 0) { _direction = Direction.NE; }
            }
            _direction = Direction.Nada;
        }
        private void SetOrientation()
        {
            switch (_direction)
            {
                case Direction.N:
                case Direction.E:
                case Direction.S:
                case Direction.W:
                    _orientation = InfrastructureOrientation.Straight;
                    break;
                case Direction.NE:                
                case Direction.SE:                
                case Direction.SW:                
                case Direction.NW:
                    _orientation = InfrastructureOrientation.Diagonal;
                    break;
                case Direction.Nada:
                default:
                    break;
            }
        }
        private double GetLength(InfrastructureOrientation o)
        {
            switch (o)
            {
                case InfrastructureOrientation.Straight:
                    return IC.LengthStraight;
                case InfrastructureOrientation.Diagonal:
                    return IC.LengthDiagonal;
                default:
                    return 0;
            }
        }
        private double GetRotation() => (int)_direction * _degreesPerIncrement;
        private Color GetColor(InfrastructureType it)
        {
            switch (it)
            {
                case InfrastructureType.MainTransporationCorridor:
                    return IC.MainRoadColor;
                case InfrastructureType.Secondary:
                    return IC.SecondaryRoadColor;                
                case InfrastructureType.Rail:
                    return IC.RailColor;
                case InfrastructureType.Pipeline:
                    return IC.PipeLineColor;
                case InfrastructureType.Hub:                    
                default:
                    return Color.Transparent;
            }
        }
    }
}
