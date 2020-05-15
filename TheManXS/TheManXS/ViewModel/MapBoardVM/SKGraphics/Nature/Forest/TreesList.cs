using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;
using TheManXS.ViewModel.Style;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.SKGraphics.Nature.Forest
{
    class TreesList : List<Tree>
    {
        System.Random rnd = new System.Random();
        private PaletteColorList _paletteColor;
        SKRect _rect;
        private static int _rowQ;
        private static float _verticalOverlapRatio;

        public TreesList(SKRect SqWhereForestIsToGo, PaletteColorList pc)
        {
            _paletteColor = pc;
            _rect = SqWhereForestIsToGo;
            if(_rowQ == 0) { SetRowQ(); }
            InitListOfTreesForSQ();
        }
        private void SetRowQ()
        {
            Game game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _rowQ = (int)game.ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.NumberOfTreesPerSideOfSQ);
            _verticalOverlapRatio = (float)game.ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.TreeVerticalOverlapRatio);
        }

        public void InitListOfTreesForSQ()
        {
            float heightOfTree = QC.SqSize / (_rowQ - (_rowQ * _verticalOverlapRatio) + _verticalOverlapRatio);
            float widthOfTree = heightOfTree * Tree.WidthVsHeightRatio;
            int maxColumns = (int)Math.Floor(QC.SqSize / widthOfTree);
            float left, top, right, bottom;

            int rand = 0;
            float offset = 0;
            int colQ = 0;

            for (int row = 0; row < _rowQ; row++)
            {
                if (row % 2 == 0)
                {
                    offset = 0;
                    colQ = maxColumns;
                }
                else
                {
                    offset = widthOfTree * 0.5f;
                    colQ = (maxColumns - 1);
                }

                for (int col = 0; col < colQ; col++)
                {
                    left = _rect.Left + col * widthOfTree + offset;
                    top = _rect.Top + row * ((1 - _verticalOverlapRatio) * heightOfTree);
                    right = left + widthOfTree;
                    bottom = top + heightOfTree;

                    SKRect rect = new SKRect(left, top, right, bottom);
                    SKColor treeBranchColor = _paletteColor.GetRandomColor(TerrainTypeE.Forest);
                    rand = rnd.Next(0, 5);

                    if (rand == 0 || rand < 4) { this.Add(new SpruceTree(rect, treeBranchColor)); }
                    else { this.Add(new PoplarTree(rect, treeBranchColor)); }
                }
            }
        }
    }
}
