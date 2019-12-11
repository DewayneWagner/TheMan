using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using SkiaSharp.Views;
using SkiaSharp;

namespace TheManXS.ViewModel.MapBoardVM.MapConstruct
{
    public class River
    {
        private bool[,] riverSQs = new bool[QC.ColQ, QC.RowQ];
        private System.Random rnd = new System.Random();
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
