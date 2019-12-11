using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp.Views;
using SkiaSharp;
using TheManXS.ViewModel.MapBoardVM;
using static TheManXS.ViewModel.MapBoardVM.MapConstruct.TileConstructCalc;
using static TheManXS.Model.Settings.SettingsMaster;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Main;
using System.Linq;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class Map
    {
        MapVM _mapVM;

        private System.Random rnd = new System.Random();       

        public Map(MapVM mapVM) 
        { 
            _mapVM = mapVM;
            TerrainColors = new TerrainColors();
            AddTerrainSQsToMap();
            new River(_mapVM, this);
        }
        public TerrainColors TerrainColors { get; set; }
        public void AddTerrainSQsToMap()
        {
            using (DBContext db = new DBContext())
            {
                using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
                {
                    using (SKPaint paint = new SKPaint())
                    {
                        gameboard.Clear();

                        for (int col = 0; col < QC.ColQ; col++)
                        {
                            for (int row = 0; row < QC.RowQ; row++)
                            {
                                SQ sq = db.SQ.Find(Coordinate.GetSQKey(row, col));

                                TileConstructCalc t = new TileConstructCalc(_mapVM, row, col);
                                int q = _mapVM.TerrainType == TerrainTypeE.Mountain ? rnd.Next(1, 500) : rnd.Next(1, 10);
                                SKRect rect = new SKRect(col * QC.SqSize, row * QC.SqSize, (col + 1) * QC.SqSize, (row + 1) * QC.SqSize);

                                // set background
                                paint.Color = TerrainColors.GetBackGroundColor(sq.TerrainType);
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
                                                paint.Color = TerrainColors.GetRandomColor(sq.TerrainType);

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
                }
            }
        }
    }
}
