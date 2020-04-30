using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{
    class SpruceTree : Tree
    {
        private const int NumberOfBranches = 4;
        private const float BottomBranchFromBottomOfRectRatio = 0.2f;

        private float _distanceOfBottomBranchFromBottom;
        private float _distanceBetweenBranchesVertical;
        private float _outsideDistance;
        private float _insidePointStartFromEdge;

        public SpruceTree(SKRect rectangleWhereTreeWillBePlaced, SKColor treeBranchesColor) : base(rectangleWhereTreeWillBePlaced, treeBranchesColor) { }

        public override void InitFields()
        {
            _distanceOfBottomBranchFromBottom = BottomBranchFromBottomOfRectRatio * RectangleWhereTreeWillBePlaced.Height;

            _distanceBetweenBranchesVertical = (RectangleWhereTreeWillBePlaced.Height - _distanceOfBottomBranchFromBottom)
                / NumberOfBranches;

            _outsideDistance = RectangleWhereTreeWillBePlaced.Width / 2 / NumberOfBranches;

            _insidePointStartFromEdge = RectangleWhereTreeWillBePlaced.Width / 3;
        }

        public override SKPaint FillPaint
        {
            get
            {
                return new SKPaint()
                {
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true,
                    Color = TreeBranchesColor,
                };
            }
        }

        public override SKPath TreeBranchesPath
        {
            get
            {
                SKPath treeBranchesPath = new SKPath();
                List<SKPoint> points = GetOrderedList();
                treeBranchesPath.MoveTo(points[0]);
                for (int i = 1; i < points.Count; i++) { treeBranchesPath.LineTo(points[i]); }
                //treeBranchesPath.Close();
                return treeBranchesPath;
            }
        }

        public override SKRect TreeTrunkRect
        {
            get
            {
                var r = RectangleWhereTreeWillBePlaced;
                float trunkWidth = r.Width * TrunkWidthRatio;

                float left = r.MidX - (trunkWidth / 2);
                float top = r.Bottom - _distanceOfBottomBranchFromBottom;
                float right = left + trunkWidth;
                float bottom = r.Bottom;

                return new SKRect(left, top, right, bottom);
            }
        }

        public override SKPaint TreeTrunkFill
        {
            get
            {
                return new SKPaint()
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.Brown,
                };
            }
        }

        private List<SKPoint> GetOrderedList()
        {
            List<SKPoint> treePoints = new List<SKPoint>();            
            var r = RectangleWhereTreeWillBePlaced;
            float bottomY = r.Bottom - _distanceOfBottomBranchFromBottom;
            int pointsOnEachSide = NumberOfBranches * 2 - 1; // subtract 1 for topPoint

            List<float> yCoords = getYCoords();

            SKPoint topPoint = new SKPoint(r.MidX, r.Top);            

            int L = 1;
            int R = 0;

            for (int increment = -1; increment < 2; increment += 2)
            {
                float insideStartPoint = getInsideStartPoint();
                float insideIncrement = Math.Abs(insideStartPoint - r.MidX) / NumberOfBranches;

                for (int i = 0; i < pointsOnEachSide; i++)
                {
                    float x = 0;
                    float y = yCoords[i];

                    if (i % 2 == 0) // outside point
                    {
                        double branchNumber = (i / 2);
                        x = r.Left * L + r.Right * R + (_outsideDistance * (float)(Math.Floor(branchNumber) - 1));
                    }
                    else // inside point
                    {
                        x = insideStartPoint + (insideIncrement * (i / 2));
                    }
                    treePoints.Add(new SKPoint(x, y));
                }

                if (increment == -1) { treePoints.Add(topPoint); }
                L = 0;
                R = 1;
                yCoords.Reverse();

                float getInsideStartPoint()
                {
                    if (increment == -1) { return (r.Left + _insidePointStartFromEdge); }
                    else { return (r.Right - _insidePointStartFromEdge); }
                }
            }
            treePoints.Add(treePoints[0]);
            return treePoints;
            List<float> getYCoords()
            {
                List<float> yc = new List<float>();

                for (int ii = 0; ii < NumberOfBranches; ii++)
                {
                    yc.Add(bottomY - _distanceBetweenBranchesVertical * ii);
                    if (ii != 0) { yc.Add(bottomY - _distanceBetweenBranchesVertical * ii); }
                }
                return yc;
            }
        }
        
    }
}
