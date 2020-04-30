using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp.Views;
using SkiaSharp;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{

    abstract class Tree
    {
        System.Random rnd = new System.Random();
        public const int HorizontalRowsOfTrees = 3;
        public const float WidthVsHeightRatio = 0.6f;
        public const float TrunkWidthRatio = 0.2f;
        private const float MaxOverhangOfTreeIntoAdjacentSQRatio = 0.1f;
        public float TreeWidth => WidthVsHeightRatio * (HorizontalRowsOfTrees / QC.SqSize);

        public Tree(SKRect rectangleWhereTreeWillBePlaced, SKColor treeBranchesColor) 
        {
            RectangleWhereTreeWillBePlaced = rectangleWhereTreeWillBePlaced;
            TreeBranchesColor = treeBranchesColor;
            InitFields();
        }

        public SKPaint StrokePaint
        {
            get
            {
                return new SKPaint()
                {
                    Style = SKPaintStyle.Stroke,
                    IsAntialias = true,
                    Color = SKColors.Black,
                    StrokeWidth = (float)(QC.SqSize * 0.02),
                };
            }
        }
        public abstract SKPaint FillPaint { get; }
        public abstract SKPath TreeBranchesPath { get; }
        public abstract SKRect TreeTrunkRect { get; }
        public abstract SKPaint TreeTrunkFill { get; }

        public SKRect RectangleWhereTreeWillBePlaced { get; set; }
        protected SKColor TreeBranchesColor { get; set; }

        public float getVerticalSpacingForNextTree()
        {
            int _verticalSpacingLB = (int)(0.1 * QC.SqSize);
            int _verticalSpacingUB = (int)(0.4 * QC.SqSize);
            return (float)rnd.Next(_verticalSpacingLB, _verticalSpacingUB);
        }

        public bool IsTooCloseToEdgeToStartNewTree(float startPointX, SKRect treeRectangle)
        {
            float distanceOfStartPointFromRightEdge = treeRectangle.Right - startPointX;
            float widthOfTreeExtendingIntoAdjacentSQ = treeRectangle.Width - distanceOfStartPointFromRightEdge;
            if (widthOfTreeExtendingIntoAdjacentSQ > (QC.SqSize * MaxOverhangOfTreeIntoAdjacentSQRatio)) { return true; }
            else { return false; }
        }

        public abstract void InitFields();
    }
}
