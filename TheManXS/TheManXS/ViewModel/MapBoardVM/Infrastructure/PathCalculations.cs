using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using TheManXS.Model.InfrastructureStuff;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class PathCalculations
    {
        CenterPointRatios _centerPointRatios;
        public PathCalculations() { _centerPointRatios = new CenterPointRatios(); }

        public SKPoint GetInfrastructureSKPoint(SQInfrastructure sq, IT it)
        {
            float ratio = _centerPointRatios.GetRatio(it);
            float x = (sq.Col * QC.SqSize) + (QC.SqSize / 2);
            float y = (sq.Row * QC.SqSize) + (QC.SqSize * ratio);
            return new SKPoint(x, y);
        }

        public SKRect GetHubRect(SQInfrastructure sq) => new SKRect(
            (sq.Col * QC.SqSize) + (QC.SqSize * 0.25f),
            (sq.Row * QC.SqSize) + (QC.SqSize * 0.25f),
            (sq.Col * QC.SqSize) + (QC.SqSize * 0.75f),
            (sq.Row * QC.SqSize) + (QC.SqSize * 0.75f));

        public SKPoint GetEdgePoint(SQInfrastructure sq, IT it)
        {
            DirectionsCompass d = getMapEdge();
            bool isStartOfPath = (sq.Row == 0 || sq.Col == 0) ? true : false;
            return getEdgePoint();

            DirectionsCompass getMapEdge()
            {
                if (sq.Row == 0) { return DirectionsCompass.N; }
                else if (sq.Row == (QC.RowQ - 1)) { return DirectionsCompass.S; }
                else if (sq.Col == 0) { return DirectionsCompass.W; }
                else { return DirectionsCompass.E; }
            }

            SKPoint getEdgePoint()
            {
                float x = 0, y = 0;

                switch (d)
                {
                    case DirectionsCompass.N:
                        x = sq.Col * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        y = 0;
                        break;
                    case DirectionsCompass.E:
                        x = (QC.ColQ + 1) * QC.SqSize;
                        y = sq.Row * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        break;
                    case DirectionsCompass.S:
                        x = sq.Col * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        y = (QC.RowQ + 1) * QC.SqSize;
                        break;
                    case DirectionsCompass.W:
                        x = 0;
                        y = sq.Row * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        break;
                    default:
                        break;
                }
                return new SKPoint(x, y);
            }
        }

        public void ProcessMapEdge(SQInfrastructure sq, ref SKPath path, IT it)
        {
            DirectionsCompass d = getMapEdge();
            bool isStartOfPath = (sq.Row == 0 || sq.Col == 0) ? true : false;
            SKPoint edgePoint = getEdgePoint();

            path.MoveTo(edgePoint);
            path.LineTo(GetInfrastructureSKPoint(sq, it));

            DirectionsCompass getMapEdge()
            {
                if (sq.Row == 0) { return DirectionsCompass.N; }
                else if (sq.Row == (QC.RowQ - 1)) { return DirectionsCompass.S; }
                else if (sq.Col == 0) { return DirectionsCompass.W; }
                else { return DirectionsCompass.E; }
            }

            SKPoint getEdgePoint()
            {
                float x = 0, y = 0;

                switch (d)
                {
                    case DirectionsCompass.N:
                        x = sq.Col * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        y = 0;
                        break;
                    case DirectionsCompass.E:
                        x = (QC.ColQ + 1) * QC.SqSize;
                        y = sq.Row * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        break;
                    case DirectionsCompass.S:
                        x = sq.Col * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        y = (QC.RowQ + 1) * QC.SqSize;
                        break;
                    case DirectionsCompass.W:
                        x = 0;
                        y = sq.Row * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        break;
                    default:
                        break;
                }
                return new SKPoint(x, y);
            }
        }
        public bool IsMapEdge(SQInfrastructure sq) => (sq.Row == 0 || sq.Row == (QC.RowQ - 1) 
            || sq.Col == 0 || sq.Col == (QC.ColQ - 1)) ? true : false;
        
        class CenterPointRatios
        {
            float[] _centerPointRatios = new float[(int)IT.Total];

            public CenterPointRatios()
            {
                _centerPointRatios[(int)IT.MainRiver] = 0.5f;
                _centerPointRatios[(int)IT.Pipeline] = 0.4f;
                _centerPointRatios[(int)IT.RailRoad] = 0.6f;
                _centerPointRatios[(int)IT.Road] = 0.7f;
                _centerPointRatios[(int)IT.Tributary] = 0.5f;
            }
            public float GetRatio(IT it) => _centerPointRatios[(int)it];
        }
    }
}
