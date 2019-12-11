using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp.Views;
using SkiaSharp;
using TheManXS.ViewModel.MapBoardVM;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class Map
    {
        MapVM _mapVM;
        private System.Random rnd = new System.Random();
        public Map(MapVM mapVM) { _mapVM = mapVM; }
        public void InitMap()
        {
            TerrainColors tc = new TerrainColors();
            _mapVM.Map = new SKBitmap((QC.SqSize * QC.ColQ), (QC.SqSize * QC.RowQ));

            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                gameboard.Clear();
                for (int col = 0; col < QC.ColQ; col++)
                {
                    for (int row = 0; row < QC.RowQ; row++)
                    {
                        using (SKPaint paint = new SKPaint())
                        {
                            TileConstructCalc t = new TileConstructCalc(_mapVM, row, col);
                            int q = _mapVM.TerrainType == TerrainTypeE.Mountain ? rnd.Next(1, 500) : rnd.Next(1, 10);
                            SKRect rect = new SKRect(col * QC.SqSize, row * QC.SqSize, (col + 1) * QC.SqSize, (row + 1) * QC.SqSize);

                            // set background
                            paint.Color = tc.GetBackGroundColor(_mapVM.TerrainType);
                            paint.Style = SKPaintStyle.Fill;
                            paint.IsAntialias = true;

                            switch (t.GetFormat())
                            {
                                case sqFormats.LinearGradient:                                    
                                    Tuple<SKPoint, SKPoint> points = t.GetGradientPoints();
                                    paint.Shader = SKShader.CreateLinearGradient(
                                        points.Item1,
                                        points.Item2,
                                        t.GetGradientColors(q),
                                        t.GetColorPosition(q),
                                        SKShaderTileMode.Clamp);
                                    break;

                                case sqFormats.SolidColor:
                                    paint.Color = tc.GetRandomColor(_mapVM.TerrainType);
                                    break;

                                case sqFormats.SweepGradient:
                                    paint.Shader = SKShader.CreateSweepGradient(t.GetCenterPoint(),
                                        t.GetGradientColors(q), t.GetColorPosition(q));
                                    break;

                                case sqFormats.VerticalSplit:
                                    float x = (float)(rnd.Next(col * QC.SqSize, (col + 1) * QC.SqSize));
                                    paint.Shader = SKShader.CreateLinearGradient(
                                        new SKPoint(x, (row * QC.SqSize)),
                                        new SKPoint(x, ((row + 1) * QC.SqSize)),
                                        t.GetGradientColors(q),
                                        t.GetColorPosition(q),
                                        SKShaderTileMode.Clamp);
                                    break;

                                case sqFormats.HorizontalSplit:
                                    float y = (float)(rnd.Next(row * QC.SqSize, ((row + 1) * QC.SqSize)));
                                    paint.Shader = SKShader.CreateLinearGradient(
                                        new SKPoint(col * QC.SqSize, y),
                                        new SKPoint((col + 1) * QC.SqSize, y),
                                        t.GetGradientColors(q),
                                        t.GetColorPosition(q),
                                        SKShaderTileMode.Clamp);
                                    break;

                                case sqFormats.Pixelly:
                                    int sqsPerSide = 25;
                                    int qSQs = (int)Math.Pow(sqsPerSide, 2);
                                    float miniSqSize = QC.SqSize / sqsPerSide;

                                    for (int miniRow = 0; miniRow < sqsPerSide; miniRow++)
                                    {
                                        for (int miniCol = 0; miniCol < sqsPerSide; miniCol++)
                                        {
                                            paint.Color = tc.GetRandomColor(_mapVM.TerrainType);

                                            SKRect miniRect = new SKRect(
                                                ((miniCol * miniSqSize) + (QC.SqSize * col)),
                                                ((miniRow * miniSqSize) + (QC.SqSize * row)),
                                                (((miniCol + 1) * miniSqSize) + (QC.SqSize * col)),
                                                (((miniRow + 1) * miniSqSize) + (QC.SqSize * row)));

                                            gameboard.DrawRect(miniRect, paint);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (_mapVM.TerrainType == TerrainTypeE.Forest)
                            {
                                paint.BlendMode = SKBlendMode.ColorDodge;
                                gameboard.DrawRect(rect, paint);
                            }
                            else if (_mapVM.TerrainType == TerrainTypeE.Grassland)
                            {
                                paint.BlendMode = SKBlendMode.Hue;
                                gameboard.DrawRect(rect, paint);
                            }
                            else if (_mapVM.TerrainType == TerrainTypeE.Mountain)
                            {
                                paint.BlendMode = SKBlendMode.Screen;
                                gameboard.DrawRect(rect, paint);
                            }
                        }
                    }
                }
                float bankWidth = (float)(QC.SqSize * 0.3);
                float riverWidth = (float)(QC.SqSize * 0.2);

                SKPaint riverBank = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = tc.GetRandomColor(TerrainTypeE.Sand),
                    StrokeWidth = bankWidth,
                    StrokeJoin = SKStrokeJoin.Round,
                };

                SKPaint water = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = tc.GetRandomColor(TerrainTypeE.River),
                    StrokeWidth = riverWidth,
                    StrokeJoin = SKStrokeJoin.Round,
                    StrokeCap = SKStrokeCap.Round,
                };

                River river = new River();
                SKPath riverPath = new SKPath();
                riverPath.Convexity = SKPathConvexity.Convex;
                Tuple<SKPoint, SKPoint> centerPoints;

                for (int column = 0; column <= QC.ColQ; column++)
                {
                    centerPoints = river.GetCenterSKPoints(column, river.GetNextRiverRow(column),
                        (column + 1), river.GetNextRiverRow(column + 1));

                    if (column == 0) { riverPath.MoveTo(centerPoints.Item1); }

                    if (column == QC.ColQ)
                    {
                        int startY = (int)centerPoints.Item2.Y;
                        int lastY = startY + (rnd.Next(-1, 2) * QC.SqSize);
                        centerPoints = river.GetCenterSKPoints((QC.ColQ * QC.SqSize), startY, (QC.ColQ * QC.SqSize + QC.SqSize), lastY);
                    }
                    if (centerPoints.Item1.Y == centerPoints.Item2.Y) { riverPath.LineTo(centerPoints.Item2); }
                    else { riverPath.ArcTo(centerPoints.Item1, centerPoints.Item2, (float)QC.SqSize / (rnd.Next(2, 5))); }
                }

                gameboard.DrawPath(riverPath, riverBank);
                gameboard.DrawPath(riverPath, water);

                riverPath.Close();
                gameboard.Save();
            }
        }
    }
}
