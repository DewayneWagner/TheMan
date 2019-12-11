using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp.Views;
using SkiaSharp;
using static TheManXS.Model.Settings.SettingsMaster;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class River
    {
        private bool[,] riverSQs = new bool[QC.ColQ, QC.RowQ];
        private System.Random rnd = new System.Random();
        private MapVM _mapVM;
        private Map _map;

        public River(MapVM mapVM, Map map)
        {
            _mapVM = mapVM;
            _map = map;
            InitAllRivers();
        }

        private void InitAllRivers()
        {
            using (SKCanvas gameboard = new SKCanvas(_mapVM.Map))
            {
                float bankWidth = (float)(QC.SqSize * 0.3);
                float riverWidth = (float)(QC.SqSize * 0.2);

                SKPaint riverBank = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = _map.TerrainColors.GetRandomColor(TerrainTypeE.Sand),
                    StrokeWidth = bankWidth,
                    StrokeJoin = SKStrokeJoin.Round,
                };

                SKPaint water = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = _map.TerrainColors.GetRandomColor(TerrainTypeE.River),
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




        public River()
        {
            InitArray();
        }
        private void InitArray()
        {
            int row = rnd.Next((int)(0.4 * QC.RowQ), (int)(0.6 * QC.RowQ));
            for (int col = 0; col < QC.ColQ; col++)
            {
                riverSQs[col, row] = true;

                if (row == 0) { row += 1; }
                else if (row == QC.RowQ) { row -= 1; }
                else { row += rnd.Next(-1, 2); }
            }
        }
        public bool IsRiver(int col, int row) => riverSQs[col, row];
        public int GetNextRiverRow(int col)
        {
            if (col <= QC.ColQ) { for (int row = 0; row < QC.RowQ; row++) { if (col != QC.ColQ && riverSQs[col, row]) { return row; } } }
            return 0;
        }
        public Tuple<SKPoint, SKPoint> GetCenterSKPoints(int fc, int fr, int tc, int tr) =>
            new Tuple<SKPoint, SKPoint>(new SKPoint(getX(fc), getY(fr)), new SKPoint(getX(tc), getY(tr)));
        private int getX(int c)
        {
            if (c == 0) { return 0; }
            else if (c == QC.ColQ) { return QC.SqSize * QC.ColQ; }
            else { return (c * QC.SqSize) + QC.SqSize / 2; }
        }
        private int getY(int r) => QC.SqSize * r + QC.SqSize / 2;
    }
}
