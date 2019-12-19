using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class PathCalculations
    {
        CenterPointRatios _centerPointRatios;
        public PathCalculations()
        {
            _centerPointRatios = new CenterPointRatios();
        }
        public SKPoint GetInfrastructureSKPoint(SQ sq, InfrastructureType it)
        {
            float ratio = _centerPointRatios.GetRatio(it);
            float x = (sq.Col * QC.SqSize) + (QC.SqSize / 2);
            float y = (sq.Row * QC.SqSize) + (QC.SqSize * ratio);
            return new SKPoint(x, y);
        }
        public SKRect GetHubRect(SQ sq) => new SKRect(
            (sq.Col * QC.SqSize) + (QC.SqSize * 0.25f),
            (sq.Row * QC.SqSize) + (QC.SqSize * 0.25f),
            (sq.Col * QC.SqSize) + (QC.SqSize * 0.75f),
            (sq.Row * QC.SqSize) + (QC.SqSize * 0.75f));

        public void ProcessMapEdge(SQ sq, ref SKPath path, InfrastructureType it)
        {
            DirectionsCompass d = getMapEdge();
            bool isStartOfPath = (sq.Row == 0 || sq.Col == 0) ? true : false;
            SKPoint edgePoint = getEdgePoint();

            path.MoveTo(edgePoint);
            path.LineTo(GetInfrastructureSKPoint(sq, it));
            
            DirectionsCompass getMapEdge()
            {
                if(sq.Row == 0) { return DirectionsCompass.N; }
                else if(sq.Row == (QC.RowQ - 1)) { return DirectionsCompass.S; }
                else if(sq.Col == 0) { return DirectionsCompass.W; }
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
        public bool IsMapEdge(SQ sq) => (sq.Row == 0 || sq.Row == (QC.RowQ - 1) || sq.Col == 0 || sq.Col == (QC.ColQ - 1)) ? true : false;
    }
    public class CenterPointRatios
    {
        float[] _centerPointRatios = new float[(int)InfrastructureType.Total];

        public CenterPointRatios()
        {
            _centerPointRatios[(int)InfrastructureType.MainRiver] = 0.5f;
            _centerPointRatios[(int)InfrastructureType.Pipeline] = 0.4f;
            _centerPointRatios[(int)InfrastructureType.RailRoad] = 0.6f;
            _centerPointRatios[(int)InfrastructureType.Road] = 0.7f;
            _centerPointRatios[(int)InfrastructureType.Tributary] = 0.5f;
        }
        public float GetRatio(InfrastructureType it) => _centerPointRatios[(int)it];
    }    
}
