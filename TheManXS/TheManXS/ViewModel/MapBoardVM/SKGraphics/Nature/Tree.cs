using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views;
using SkiaSharp;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature
{
    
    class Tree
    {
        // constants
        private const int NumberOfBranches = 4;
        private const float BranchInsideVsOutsideRatio = 0.3f;

        // ratios
        private float _treeStrokeWidthRatio = 0.03f;
        private float _strokeWidthRatio = 0.01f;

        // calculated fields
        private float _distanceBetweenBranchesVertical;
        private float _distanceBetweenBranchesHorizontal;
        SKPoint _topPoint;

        // point calculation array
        static float[,] _pointCalculationArray;
        int x = 0, y = 1;
        private int _numberOfPointsOnEachSideOfTruck = NumberOfBranches * 2;

        private SKRect _treeRect;
        private Game _game;

        SKPaint paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
        };
        SKPaint _treeStroke = new SKPaint()
        {
            IsAntialias = true,
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
        };

        public Tree(Game game, SKRect rectangleWhereTreeIsToBePlaced)
        {
            _game = game;
            _treeRect = rectangleWhereTreeIsToBePlaced;
            
            InitFields();
            if(_pointCalculationArray == null) { _pointCalculationArray = getPointCalculationArray(); }
            CreateAndPaintTree();
        }
        private void InitFields()
        {
            _distanceBetweenBranchesVertical = _treeRect.Height / (NumberOfBranches + 1);
            _distanceBetweenBranchesHorizontal = (_treeRect.Width / 2) / (NumberOfBranches + 3);
            paint.StrokeWidth = _treeRect.Width * _treeStrokeWidthRatio;
            paint.Color = _game.PaletteColors.GetRandomColor(Model.ParametersForGame.TerrainTypeE.Forest);
            _topPoint = new SKPoint(_treeRect.MidX, _treeRect.Top);
            _treeStroke.StrokeWidth = _strokeWidthRatio * QC.SqSize;
        }
        private float[,] getPointCalculationArray()
        {
            float[,] p = new float[2, _numberOfPointsOnEachSideOfTruck];
            int yIncrement;
            
            for (int i = 0; i < p.GetLength(1); i++)
            {
                yIncrement = i / 2;
                if (isOutsidePoint(i)) 
                { 
                    p[x, i] = (i * _distanceBetweenBranchesHorizontal);
                    p[y, i] = (yIncrement * _distanceBetweenBranchesVertical);
                }
                else 
                { 
                    p[x, i] = (i * _distanceBetweenBranchesHorizontal * BranchInsideVsOutsideRatio);
                    p[y, i] = (yIncrement * _distanceBetweenBranchesVertical);
                }
            }

            return p;
            bool isOutsidePoint(int i) => i % 2 == 0 ? true : false;
        }
        private void CreateAndPaintTree()
        {
            SKPath _treePath = new SKPath();
            initPath();
            drawPath();

            void initPath()
            {
                SKPoint lastPointOnRightSide = new SKPoint();
                _treePath.MoveTo(_topPoint);

                for (float i = -1; i < 2; i += 2)
                {
                    for (int b = 0; b < _pointCalculationArray.GetLength(1); b++)
                    {
                        float NextX = _treeRect.MidX + (i * _pointCalculationArray[x, b]);
                        float NextY = _treeRect.Top + _pointCalculationArray[y, b];
                        _treePath.LineTo(new SKPoint(NextX, NextY));

                        if (i == -1 && b == (_pointCalculationArray.GetLength(1) - 1))
                        {
                            lastPointOnRightSide = new SKPoint(NextX, NextY);
                        }
                        else if (i == 1 && b == (_pointCalculationArray.GetLength(1) - 1))
                        {
                            _treePath.LineTo(lastPointOnRightSide);
                        }
                    }
                }
                _treePath.Close();
            }
            void drawPath()
            {
                using (SKCanvas canvas = new SKCanvas(_game.GameBoardVM.MapVM.SKBitMapOfMap))
                {
                    canvas.DrawPath(_treePath, paint);
                    canvas.DrawPath(_treePath, _treeStroke);
                    canvas.Save();
                }
            }            
        }
    }
}
