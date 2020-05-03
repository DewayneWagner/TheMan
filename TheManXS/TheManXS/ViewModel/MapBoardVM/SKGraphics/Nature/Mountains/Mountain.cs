using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.Style;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Mountains
{
    abstract class Mountain
    {
        System.Random rnd = new System.Random();
        static PaletteColorList pc;

        private const float _strokeWidthRatio = 0.03f;

        public Mountain(SKRect rectangleWhereMountainWillBePlaced)
        {
            SKRectSQ = rectangleWhereMountainWillBePlaced;
            pc = pc?? new PaletteColorList();
        }

        protected SKRect SKRectSQ { get; set; }

        private SKPaint _mountainPaint;
        public SKPaint MountainPaint
        {
            get
            {
                return _mountainPaint = _mountainPaint ?? new SKPaint()
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Shader = GetGradient(),
                };
            }
        }
        public SKPoint TopPoint { get; set; }

        private SKPaint _mountainStroke;
        public SKPaint MountainStroke
        {
            get
            {
                return _mountainStroke = _mountainStroke?? new SKPaint()
                {
                    IsAntialias = true,
                    Color = SKColors.Black,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = _strokeWidthRatio * QC.SqSize,
                };
            }
        }

        public abstract SKPath MountainPath { get; }

        SKShader GetGradient()
        {
            SKColor[] colors = getSKColorArray();

            float x = ((MountainPath.LastPoint.X - MountainPath[0].X) / 2) + MountainPath[0].X;
            float y = ((MountainPath.LastPoint.Y - MountainPath[0].Y) / 2) + MountainPath[0].Y;

            SKPoint gradientStartPoint = new SKPoint(x, y);
            float[] colorPosition = getColorPositionArray();

            return SKShader.CreateLinearGradient(gradientStartPoint,
                TopPoint,
                colors,
                colorPosition,
                SKShaderTileMode.Mirror);

            //MountainPaint.Shader = SKShader.CreateLinearGradient(gradientStartPoint,
            //    TopPoint,
            //    colors,
            //    colorPosition,
            //    SKShaderTileMode.Mirror);

            SKColor[] getSKColorArray()
            {
                SKColor[] colorArray = new SKColor[4];

                colorArray[0] = pc.Where(c => c.Description == "Banff 2")
                    .Select(c => c.SKColor)
                    .FirstOrDefault();

                colorArray[1] = pc.Where(c => c.Description == "Banff 5")
                    .Select(c => c.SKColor)
                    .FirstOrDefault();

                colorArray[2] = pc.Where(c => c.Description == "Banff 1")
                    .Select(c => c.SKColor)
                    .FirstOrDefault();

                colorArray[3] = SKColors.GhostWhite;

                return colorArray;
            }
            float[] getColorPositionArray() { return new float[4] { 0.2f, 0.4f, 0.6f, 0.8f }; }
        }
    }
}
