using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM;
using QC = TheManXS.Model.Settings.QuickConstants;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class PaintTypes : List<SKPaint>
    {
        public PaintTypes()
        {
            for (int i = 0; i < (int)IT.Total; i++) { this.Add(new SKPaint()); }

            this[(int)IT.MainRiver] = GetMainRiverSKPaint();
            this[(int)IT.Tributary] = GetTributarySKPaint();
            this[(int)IT.Road] = GetRoadSKPaint();
            this[(int)IT.Pipeline] = GetPipelineSKPaint();
            this[(int)IT.RailRoad] = GetRailroadSKPaint();
            this[(int)IT.Hub] = GetHubSKPaint();
        }

        private SKPaint GetHubSKPaint()
        {
            return new SKPaint
            {
                Color = SKColors.White,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 1,
            };
        }
        private SKPaint GetRailroadSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkGray,
                StrokeWidth = QC.SqSize * 0.1f,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetPipelineSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = QC.SqSize * 0.1f,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetRoadSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkBlue,
                StrokeWidth = QC.SqSize * 0.1f,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetTributarySKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(12, 163, 218),
                StrokeWidth = QC.SqSize * 0.1f,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetMainRiverSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(12, 163, 218),
                StrokeWidth = QC.SqSize * 0.2f,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint sand = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2,
        };
    }
}
