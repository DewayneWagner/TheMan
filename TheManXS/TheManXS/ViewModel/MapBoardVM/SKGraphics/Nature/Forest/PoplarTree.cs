using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{
    class PoplarTree : Tree
    {
        private const float BottomBranchFromBottomRatio = 0.3f;
        private const float BottomBranchContactsEdgeFromBottomRatio = 0.4f;
        private const float BranchRisesVerticallyAlongEdgeRatio = 0.9f;

        private SKColor PoplarTrunk = new SKColor(217, 204, 208);
        private SKColor PoplarLeaves = new SKColor(119, 180, 50);

        private float _bottomBranchFromBottom;
        private float _trunkWidth;
        private SKPoint _topPoint;


        public PoplarTree(SKRect rectangleWhereTreeWillBePlaced, SKColor treeBranchesColor) : base(rectangleWhereTreeWillBePlaced, treeBranchesColor) { }
        public override SKPaint FillPaint
        {
            get
            {
                SKPaint fillPaint = new SKPaint()
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = TreeBranchesColor,                    
                };
                return fillPaint;
            }
        }

        public override SKPath TreeBranchesPath
        {
            get
            {
                SKPath p = new SKPath();
                List<SKPoint> pointList = GetListOfPointsForBranches();
                p.MoveTo(pointList[0]);
                foreach (SKPoint point in pointList) { p.LineTo(point); }
                p.Close();
                return p;
            }
        }

        public override SKRect TreeTrunkRect
        {
            get
            {
                var r = RectangleWhereTreeWillBePlaced;

                float left = r.Left + r.Width / 2 - _trunkWidth / 2;
                float top = r.Bottom - _bottomBranchFromBottom;
                float right = r.Right - r.Width / 2 + _trunkWidth / 2;
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
                    Color = PoplarTrunk,
                };
            }
        }

        public override void InitFields()
        {
            var r = RectangleWhereTreeWillBePlaced;
            _bottomBranchFromBottom = BottomBranchFromBottomRatio * r.Height;
            _topPoint = new SKPoint(r.MidX, r.Top);
            _trunkWidth = TrunkWidthRatio * r.Width;
        }
        private List<SKPoint> GetListOfPointsForBranches()
        {
            var r = RectangleWhereTreeWillBePlaced;
            List<SKPoint> list = new List<SKPoint>();

            // start from left point where trunk meets branches, circulate clockwise
            list.Add(new SKPoint(TreeTrunkRect.Left, TreeTrunkRect.Top));
            list.Add(new SKPoint(r.Left, (r.Bottom - (BottomBranchContactsEdgeFromBottomRatio * r.Height))));
            list.Add(new SKPoint(r.Left, (r.Bottom - (BranchRisesVerticallyAlongEdgeRatio * r.Height))));
            list.Add(_topPoint);
            list.Add(new SKPoint(r.Right, (r.Bottom - (BranchRisesVerticallyAlongEdgeRatio * r.Height))));
            list.Add(new SKPoint(r.Right, (r.Bottom - (BottomBranchContactsEdgeFromBottomRatio * r.Height))));
            list.Add(new SKPoint(TreeTrunkRect.Right, TreeTrunkRect.Top));

            return list;
        }
        //void SetGradient()
        //{
        //    SKColor[] colors = getSKColorArray();
        //    SKPoint gradienStartPoint = TreeBranchesPath.Points[0];

        //    float[] colorPosition = new float[4] { 0.2f, 0.4f, 0.6f, 0.8f };

        //    FillPaint.Shader = SKShader.CreateLinearGradient(gradienStartPoint, _topPoint,
        //        colors, colorPosition, SKShaderTileMode.Clamp);            

        //    SKColor[] getSKColorArray()
        //    {
        //        SKColor[] ca = new SKColor[4];
        //        Game g = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];

        //        ca[0] = g.PaletteColors.Where(c => c.Description == "Conklin 2")
        //            .Select(c => c.SKColor)
        //            .FirstOrDefault();

        //        ca[1] = g.PaletteColors.Where(c => c.Description == "Conklin 3")
        //            .Select(c => c.SKColor)
        //            .FirstOrDefault();

        //        ca[2] = g.PaletteColors.Where(c => c.Description == "Conklin 1")
        //            .Select(c => c.SKColor)
        //            .FirstOrDefault();

        //        ca[3] = g.PaletteColors.Where(c => c.Description == "Conklin 4")
        //            .Select(c => c.SKColor)
        //            .FirstOrDefault();

        //        return ca;
        //    }
        //}
    }
}
