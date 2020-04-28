using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Borders
{
    class StartingBorders
    {
        private Game _game;
        private float _strokeWidthRatio = 0.01f;

        private List<SKPath> _listOfAllBorderPaths = new List<SKPath>();

        SKPaint _borderPaint = new SKPaint()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke, 
            Color = SKColors.Black,
        };

        public StartingBorders(Game game)
        {
            _game = game;
            _borderPaint.StrokeWidth = _strokeWidthRatio * QC.SqSize;
            //_borderPaint.PathEffect = SKPathEffect.CreateDash(getDashArray(), 50);
            CreateRowBorders();
            CreateColBorders();
            PaintBorders();
        }
        private float[] getDashArray()
        {
            int qIncrements = 100;
            float lengthOfIncrements = QC.SqSize / qIncrements;
            float[] a = new float[qIncrements];

            for (int i = 0; i < qIncrements; i++)
            {
                a[i] = lengthOfIncrements * i;
            }

            return a;
        }
        void CreateRowBorders()
        {
            for (int row = QC.SqSize; row < (QC.RowQ * QC.SqSize); row += QC.SqSize)
            {
                SKPath borderPath = new SKPath();

                SKPoint startPoint = new SKPoint(0, row);
                SKPoint endPoint = new SKPoint((QC.SqSize * QC.ColQ), row);

                borderPath.MoveTo(startPoint);
                borderPath.LineTo(endPoint);

                _listOfAllBorderPaths.Add(borderPath);
            }            
        }
        void CreateColBorders()
        {
            for (int col = QC.SqSize; col < (QC.ColQ * QC.SqSize); col += QC.SqSize)
            {
                SKPath borderPath = new SKPath();

                SKPoint startPoint = new SKPoint(col, 0);
                SKPoint endPoint = new SKPoint(col, (QC.SqSize * QC.RowQ));

                borderPath.MoveTo(startPoint);
                borderPath.LineTo(endPoint);

                _listOfAllBorderPaths.Add(borderPath);
            }
        }
        void PaintBorders()
        {
            using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
            {
                foreach (SKPath path in _listOfAllBorderPaths)
                {
                    canvas.DrawPath(path, _borderPaint);
                }
                canvas.Save();
            }
        }
    }
}
