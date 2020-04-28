using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{
    class ForestConstants
    {
        System.Random rnd = new System.Random();
        public const int HorizontalRowsOfTrees = 3;
        public const float WidthVsHeightRatio = 0.6f;
        public const float TrunkWidthRatio = 0.15f;
        public const float MaxOverhangOfTreeIntoAdjacentSQ = 0.1f;

        public float TreeStrokeWidth => 0.02f * QC.SqSize;
        public float TreeHeight => HorizontalRowsOfTrees / QC.SqSize;
        public float TreeWidth => WidthVsHeightRatio * TreeHeight;

        private int _verticalSpacingLB = (int)(0.1 * QC.SqSize);
        private int _verticalSpacingUB = (int)(0.4 * QC.SqSize);

        public float getVerticalSpacingForNextTree() => (float)rnd.Next(_verticalSpacingLB, _verticalSpacingUB);
        public bool IsTooCloseToEdgeToStartNewTree(float startPointX, SKRect treeRectangle)
        {
            float distanceOfStartPointFromRightEdge = treeRectangle.Right - startPointX;
            float widthOfTreeExtendingIntoAdjacentSQ = treeRectangle.Width - distanceOfStartPointFromRightEdge;
            if (widthOfTreeExtendingIntoAdjacentSQ > (QC.SqSize * MaxOverhangOfTreeIntoAdjacentSQ)) { return true; }
            else { return false; }
        }
    }
}
