using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp.Views;
using SkiaSharp;
using TheManXS.ViewModel.MapBoardVM;
using static TheManXS.ViewModel.MapBoardVM.MapConstruct.TileConstructCalc;
using TheManXS.Model.ParametersForGame;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Model.Main;
using System.Linq;
using TheManXS.Model.Map.Surface;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class Map
    {
        Game _game;

        private System.Random rnd = new System.Random();
        private SQ sq;
        private SKRect rect;
        private TileConstructCalc t;

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

        public Map(Game game) 
        {
            _game = game;
            AddTerrainSQsToMap();
        }
        public void AddTerrainSQsToMap()
        {
            using (SKCanvas gameboard = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {                    
                gameboard.Clear();

                // this loop is slot - 5-15 seconds
                for (int col = 0; col < QC.ColQ; col++)
                {
                    for (int row = 0; row < QC.RowQ; row++)
                    {
                        sq = getSQ(row, col);
                        rect = getSKRect();
                        setTileFormat();

                        if (sq.TerrainType == TerrainTypeE.Grassland) { standardTilePaint.BlendMode = SKBlendMode.Hue; }
                        else if (sq.TerrainType == TerrainTypeE.Mountain) { standardTilePaint.BlendMode = SKBlendMode.Screen; }
                            
                        if (sq.TerrainType != TerrainTypeE.Forest) { gameboard.DrawRect(rect, standardTilePaint); }
                    }
                }
                gameboard.Save();

                void setTileFormat()
                {
                    t = new TileConstructCalc(sq.Row, sq.Col, _game);
                    switch (t.GetFormat(sq.TerrainType))
                    {
                        case sqFormats.LinearGradient:
                            setLinearGradient();
                            break;
                        case sqFormats.SolidColor:
                            standardTilePaint.Color = _game.PaletteColors.GetRandomColor(sq.TerrainType);
                            break;
                        case sqFormats.SweepGradient:
                            setSweepGradient();
                            break;
                        case sqFormats.VerticalSplit:
                            setVerticalSplit();
                            break;
                        case sqFormats.HorizontalSplit:
                            setHorizontalSplit();
                            break;
                        case sqFormats.Pixelly:
                            setPixelly(gameboard);
                            break;
                    }
                }
            }

            SQ getSQ(int row, int col) => _game.SquareDictionary[Coordinate.GetSQKey(row, col)];
            SKRect getSKRect() => new SKRect(sq.Col * QC.SqSize, sq.Row * QC.SqSize, (sq.Col + 1) * QC.SqSize, (sq.Row + 1) * QC.SqSize);
            int getQ(TerrainTypeE tt) => tt == TerrainTypeE.Mountain ? rnd.Next(25, 50) : rnd.Next(5, 10);
                
            void setLinearGradient()
            {
                int q = getQ(sq.TerrainType);
                Tuple<SKPoint, SKPoint> points = t.GetGradientPoints();
                standardTilePaint.Shader = SKShader.CreateLinearGradient(
                    points.Item1,
                    points.Item2,
                    t.GetGradientColors(q, sq.TerrainType),
                    t.GetColorPosition(q),
                    SKShaderTileMode.Clamp);
            }

            void setSweepGradient()
            {
                int q = getQ(sq.TerrainType);
                standardTilePaint.Shader = SKShader.CreateSweepGradient(t.GetCenterPoint(),
                    t.GetGradientColors(q, sq.TerrainType), t.GetColorPosition(q));
            }

            void setVerticalSplit()
            {
                int q = getQ(sq.TerrainType);

                float x = (float)(rnd.Next(sq.Col * QC.SqSize, (sq.Col + 1) * QC.SqSize));
                standardTilePaint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(x, (sq.Row * QC.SqSize)),
                    new SKPoint(x, ((sq.Row + 1) * QC.SqSize)),
                    t.GetGradientColors(q,sq.TerrainType),
                    t.GetColorPosition(q),
                    SKShaderTileMode.Clamp);
            }

            void setHorizontalSplit()
            {
                int q = getQ(sq.TerrainType);
                float y = (float)(rnd.Next(sq.Row * QC.SqSize, ((sq.Row + 1) * QC.SqSize)));
                standardTilePaint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(sq.Col * QC.SqSize, y),
                    new SKPoint((sq.Col + 1) * QC.SqSize, y),
                    t.GetGradientColors(q, sq.TerrainType),
                    t.GetColorPosition(q),
                    SKShaderTileMode.Clamp);
            }

            void setPixelly(SKCanvas gameboard)
            {
                int sqsPerSide = 25;
                float miniSqSize = QC.SqSize / sqsPerSide;
                int colPosition = QC.SqSize * sq.Col;
                int rowPosition = QC.SqSize * sq.Row;

                for (int miniRow = 0; miniRow < sqsPerSide; miniRow++)
                {
                    for (int miniCol = 0; miniCol < sqsPerSide; miniCol++)
                    {
                        miniPaint.Color = _game.PaletteColors.GetRandomColor(TerrainTypeE.Forest);

                        SKRect miniRect = new SKRect(
                            ((miniCol * miniSqSize) + colPosition),
                            ((miniRow * miniSqSize) + rowPosition),
                            (((miniCol + 1) * miniSqSize) + colPosition),
                            (((miniRow + 1) * miniSqSize) + rowPosition));
                        gameboard.DrawRect(miniRect, miniPaint);
                    }
                }
            }
            
        }
    }
}
