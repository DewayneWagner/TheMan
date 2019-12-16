using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class PathCalculations
    {
        CenterPointRatios _centerPointRatios;
        public PathCalculations()
        {
            _centerPointRatios = new CenterPointRatios();
        }
        public float GetX(int col) => (col * QC.SqSize) + QC.SqSize / 2;
        public float GetY(int row) => (QC.SqSize * row) + QC.SqSize / 2;
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
            float x = (sq.Col * QC.SqSize) + (QC.SqSize / 2);
            float y = (sq.Row * QC.SqSize) + (QC.SqSize * _centerPointRatios.GetRatio(it));
            return new SKPoint(x, y);
        }
        public SKRect GetHubRect(SQ sq) => new SKRect(
            (sq.Col * QC.SqSize) + (QC.SqSize * 0.25f),
            (sq.Row * QC.SqSize) + (QC.SqSize * 0.25f),
            (sq.Col * QC.SqSize) + (QC.SqSize * 0.75f),
            (sq.Row * QC.SqSize) + (QC.SqSize * 0.75f));
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
