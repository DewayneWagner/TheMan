using SkiaSharp;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{

    abstract class Tree
    {
        System.Random rnd = new System.Random();
        public const float WidthVsHeightRatio = 0.6f;
        public const float TrunkWidthRatio = 0.2f;
        private const float MaxOverhangOfTreeIntoAdjacentSQRatio = 0.25f;

        public Tree(SKRect rectangleWhereTreeWillBePlaced, SKColor treeBranchesColor)
        {
            RectangleWhereTreeWillBePlaced = rectangleWhereTreeWillBePlaced;
            TreeBranchesColor = treeBranchesColor;
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
                    StrokeWidth = (float)(QC.SqSize * 0.01),
                };
            }
        }
        public abstract SKPaint FillPaint { get; }
        public abstract SKPath TreeBranchesPath { get; }
        public abstract SKRect TreeTrunkRect { get; }
        public abstract SKPaint TreeTrunkFill { get; }

        public SKRect RectangleWhereTreeWillBePlaced { get; set; }
        protected SKColor TreeBranchesColor { get; set; }

    }
}
