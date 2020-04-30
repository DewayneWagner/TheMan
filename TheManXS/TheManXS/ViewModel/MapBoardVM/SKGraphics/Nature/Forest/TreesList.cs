using SkiaSharp;
using System;
using System.Collections.Generic;
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
        public TreesList(List<KeyValuePair<int,SQ>> listOfAllForestSQs, SKBitmap gameBoard, PaletteColorList paletteColor)
        {
            InitListWithAllTrees(paletteColor, listOfAllForestSQs);
            DrawAllTreesOnCanvas(gameBoard);
        }
        void InitListWithAllTrees(PaletteColorList pc, List<KeyValuePair<int, SQ>> listOfAllForestSQ)
        {
            float verticalOverlapRatio = 0.5f;
            int rowQ = 4;
            float heightOfTree = QC.SqSize / (rowQ - (rowQ * verticalOverlapRatio) + verticalOverlapRatio);
            float widthOfTree = heightOfTree * Tree.WidthVsHeightRatio;
            int maxColumns = (int)Math.Floor(QC.SqSize / widthOfTree);
            float left, top, right, bottom;
            int rand = 0;
            float offset = 0;
            int colQ = 0;

            foreach (KeyValuePair<int,SQ> item in listOfAllForestSQ)
            {
                for (int row = 0; row < rowQ; row++)
                {
                    if(row % 2 == 0)
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
                        left = item.Value.Col * QC.SqSize + col * widthOfTree + offset;
                        top = item.Value.Row * QC.SqSize + row * ((1 - verticalOverlapRatio) * heightOfTree);
                        right = left + widthOfTree;
                        bottom = top + heightOfTree;

                        SKRect rect = new SKRect(left, top, right, bottom);
                        SKColor treeBranchColor = pc.GetRandomColor(TerrainTypeE.Forest);
                        rand = rnd.Next(0, 3);

                        if(rand == 0 || rand == 1) { this.Add(new SpruceTree(rect, treeBranchColor)); }
                        else { this.Add(new PoplarTree(rect, treeBranchColor)); }
                    }
                }
            }
        }
        void DrawAllTreesOnCanvas(SKBitmap map)
        {
            using (SKCanvas canvas = new SKCanvas(map))
            {
                foreach (Tree tree in this)
                {
                    canvas.DrawPath(tree.TreeBranchesPath, tree.FillPaint);
                    canvas.DrawPath(tree.TreeBranchesPath, tree.StrokePaint);
                    canvas.DrawRect(tree.TreeTrunkRect, tree.TreeTrunkFill);
                    canvas.DrawRect(tree.TreeTrunkRect, tree.StrokePaint);
                }
                canvas.Save();
            }
        }        
    }
}
