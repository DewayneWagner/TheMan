using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class InfrastructurePaintTypes : List<SKPaint>
    {
        public InfrastructurePaintTypes()
        {
            this[(int)InfrastructureType.MainRiver] = GetMainRiverSKPaint();
            this[(int)InfrastructureType.Tributary] = GetTributarySKPaint();
            this[(int)InfrastructureType.Road] = GetRoadSKPaint();
            this[(int)InfrastructureType.Pipeline] = GetPipelineSKPaint();
            this[(int)InfrastructureType.RailRoad] = GetRailroadSKPaint();
            this[(int)InfrastructureType.Hub] = GetHubSKPaint();
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
                StrokeWidth = QC.SqSize / 10,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetPipelineSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = QC.SqSize / 10,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetRoadSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkBlue,
                StrokeWidth = QC.SqSize / 10,
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
    }
}
