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
        public void setStartPoint(SQ sq, ref SKPath path)
        {
            if (sq.Row == 0) { path.MoveTo(((sq.Col * QC.SqSize) + (QC.SqSize / 2)), 0); }
            else { path.MoveTo(0, ((sq.Row * QC.SqSize) + (QC.SqSize / 2))); }
        }
        public void lineToEndPoint(SQ sq, ref SKPath path)
        {
            if (sq.Row == (QC.RowQ - 1)) { path.LineTo(((sq.Col * QC.SqSize) + (QC.SqSize / 2)), ((QC.RowQ + 1) * QC.SqSize)); }
            else { path.LineTo(((QC.ColQ + 1) * QC.SqSize), ((sq.Row * QC.SqSize) + (QC.SqSize / 2))); }
        }
        public SKPoint GetInfrastructureSKPoint(SQ sq, InfrastructureType it)
        {
            float ratio = _centerPointRatios.GetRatio(it);
            float x = (sq.Col * QC.SqSize) + (QC.SqSize * ratio);
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

            if(isStartOfPath) { path.MoveTo(getEdgePoint()); }
            else if (!isStartOfPath) { path.LineTo(getEdgePoint()); }

            DirectionsCompass getMapEdge()
            {
                if(sq.Row == 0) { return DirectionsCompass.N; }
                else if(sq.Row == QC.RowQ) { return DirectionsCompass.S; }
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
                        x = 0;
                        y = sq.Row * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        break;
                    case DirectionsCompass.S:
                        x = sq.Col * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        y = (QC.RowQ + 1) * QC.SqSize;
                        break;
                    case DirectionsCompass.W:
                        x = (QC.ColQ + 1) * QC.SqSize;
                        y = sq.Row * QC.SqSize + QC.SqSize * _centerPointRatios.GetRatio(it);
                        break;
                    default:
                        break;
                }
                return new SKPoint(x, y);
            }                  
        }
        public bool isMapEdge(SQ sq) => (sq.Row == 0 || sq.Row == QC.RowQ || sq.Col == 0 || sq.Col == QC.ColQ) ? true : false;
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
    public class AdjSQ
    {
        public AdjSQ() {}
        public SQ square { get; set; }
        public bool HasTheSameInfrastructureType { get; set; }
    }
    public class AdjacentSQsList : List<AdjSQ>
    {
        private static int[] R = new int[(int)AdjSqsDirection.Total] { 0, 1, 1, 1 };
        private static int[] C = new int[(int)AdjSqsDirection.Total] { 1, 1, 0, -1 };
        public enum AdjSqsDirection { E, SE, S, SW, Total }
        SQ _sq;
        List<SQ> _sortedList;
        public AdjacentSQsList(SQ sq, List<SQ> sortedList)
        {
            _sq = sq;
            _sortedList = sortedList;
            UpdateList();
        }
        void UpdateList()
        {
            for (int i = 0; i < (int)AdjSqsDirection.Total; i++)
            {
                if(_sortedList.Any(s => s.Row == _sq.Row + R[i] && _sq.Col == _sq.Col + C[i]))
                    { this.Add(new AdjSQ() { square = _sortedList[i], HasTheSameInfrastructureType = true }); }
                else { this.Add(new AdjSQ() { HasTheSameInfrastructureType = false }); }
            }
        }
    }
}
