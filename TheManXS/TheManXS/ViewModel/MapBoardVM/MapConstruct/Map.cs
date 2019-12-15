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

        SKPaint miniPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            BlendMode = SKBlendMode.ColorDodge,
            IsAntialias = true,
        };

        SKPaint standardTilePaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
        };

        public Map(MapVM mapVM) 
        { 
            _mapVM = mapVM;
            TerrainColors = new TerrainColors();
            AddTerrainSQsToMap();
            new RiverBuilder(_mapVM);
        }
        public TerrainColors TerrainColors { get; set; }
        public void AddTerrainSQsToMap()
        {
            using (DBContext db = new DBContext())
            {
                using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
                {                    
                    gameboard.Clear();

                    for (int col = 0; col < QC.ColQ; col++)
                    {
                        for (int row = 0; row < QC.RowQ; row++)
                        {
                            SQ sq = db.SQ.Find(Coordinate.GetSQKey(row, col));

                            TileConstructCalc t = new TileConstructCalc(_mapVM, row, col);
                            int q = sq.TerrainType == TerrainTypeE.Mountain ? rnd.Next(25,50) : rnd.Next(5, 10);
                            SKRect rect = new SKRect(col * QC.SqSize, row * QC.SqSize, (col + 1) * QC.SqSize, (row + 1) * QC.SqSize);

                            switch (t.GetFormat(sq.TerrainType))
                            {
                                case sqFormats.LinearGradient:
                                    Tuple<SKPoint, SKPoint> points = t.GetGradientPoints();
                                    standardTilePaint.Shader = SKShader.CreateLinearGradient(
                                        points.Item1,
                                        points.Item2,
                                        t.GetGradientColors(q,sq.TerrainType),
                                        t.GetColorPosition(q),
                                        SKShaderTileMode.Clamp);
                                    break;

                                case sqFormats.SolidColor:
                                    standardTilePaint.Color = TerrainColors.GetRandomColor(sq.TerrainType);
                                    break;

                                case sqFormats.SweepGradient:
                                    standardTilePaint.Shader = SKShader.CreateSweepGradient(t.GetCenterPoint(),
                                        t.GetGradientColors(q,sq.TerrainType), t.GetColorPosition(q));
                                    break;

                                case sqFormats.VerticalSplit:
                                    float x = (float)(rnd.Next(col * QC.SqSize, (col + 1) * QC.SqSize));
                                    standardTilePaint.Shader = SKShader.CreateLinearGradient(
                                        new SKPoint(x, (row * QC.SqSize)),
                                        new SKPoint(x, ((row + 1) * QC.SqSize)),
                                        t.GetGradientColors(q,sq.TerrainType),
                                        t.GetColorPosition(q),
                                        SKShaderTileMode.Clamp);
                                    break;

                                case sqFormats.HorizontalSplit:
                                    float y = (float)(rnd.Next(row * QC.SqSize, ((row + 1) * QC.SqSize)));
                                    standardTilePaint.Shader = SKShader.CreateLinearGradient(
                                        new SKPoint(col * QC.SqSize, y),
                                        new SKPoint((col + 1) * QC.SqSize, y),
                                        t.GetGradientColors(q,sq.TerrainType),
                                        t.GetColorPosition(q),
                                        SKShaderTileMode.Clamp);
                                    break;

                                case sqFormats.Pixelly:
                                    int sqsPerSide = 25;
                                    float miniSqSize = QC.SqSize / sqsPerSide;

                                    for (int miniRow = 0; miniRow < sqsPerSide; miniRow++)
                                    {
                                        for (int miniCol = 0; miniCol < sqsPerSide; miniCol++)
                                        {
                                            miniPaint.Color = TerrainColors.GetRandomColor(TerrainTypeE.Forest);

                                            SKRect miniRect = new SKRect(
                                                ((miniCol * miniSqSize) + (QC.SqSize * col)),
                                                ((miniRow * miniSqSize) + (QC.SqSize * row)),
                                                (((miniCol + 1) * miniSqSize) + (QC.SqSize * col)),
                                                (((miniRow + 1) * miniSqSize) + (QC.SqSize * row)));                                                
                                            gameboard.DrawRect(miniRect, miniPaint);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (sq.TerrainType == TerrainTypeE.Grassland) { standardTilePaint.BlendMode = SKBlendMode.Hue; }
                            else if (sq.TerrainType == TerrainTypeE.Mountain) { standardTilePaint.BlendMode = SKBlendMode.Screen; }
                            if (sq.TerrainType != TerrainTypeE.Forest) { gameboard.DrawRect(rect, standardTilePaint); }
                            
                        }
                    }
                    gameboard.Save();
                }
            }
        }
    }
}
