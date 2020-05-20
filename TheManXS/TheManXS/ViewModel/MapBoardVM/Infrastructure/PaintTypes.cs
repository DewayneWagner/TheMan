using SkiaSharp;
using System.Collections.Generic;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Infrastructure
{
    public class PaintTypes : List<SKPaint>
    {
        private const float RiverWidthRatio = 0.15f;
        private const float TributaryWidthRatio = 0.05f;
        private const float SandWidthOverWater = 0.05f;
        private const float InfrastructureWidthRatio = 0.1f;

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
                Color = SKColors.Crimson,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
            };
        }
        private SKPaint GetRailroadSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkGray,
                StrokeWidth = QC.SqSize * InfrastructureWidthRatio,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetPipelineSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = QC.SqSize * InfrastructureWidthRatio,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetRoadSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkBlue,
                StrokeWidth = QC.SqSize * InfrastructureWidthRatio,
                StrokeCap = SKStrokeCap.Round,
            };
        }
        private SKPaint GetTributarySKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(12, 163, 218),
                StrokeWidth = QC.SqSize * TributaryWidthRatio,
            };
        }
        private SKPaint GetMainRiverSKPaint()
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(12, 163, 218),
                StrokeWidth = QC.SqSize * RiverWidthRatio,
            };
        }
        public SKPaint GetSandPaint(IT it)
        {
            SKPaint sandPaint = new SKPaint()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(194, 178, 128),
            };
            if (it == IT.MainRiver) { sandPaint.StrokeWidth = (QC.SqSize * (RiverWidthRatio + SandWidthOverWater)); }
            else { sandPaint.StrokeWidth = (QC.SqSize * (TributaryWidthRatio + SandWidthOverWater)); }
            return sandPaint;
        }
    }
}
