using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    public class StaggeredBorder
    {
        private SKColor BorderColor;
        private SQ _sq;
        private List<SQ> _sqList;

        public StaggeredBorder(SKColor c) { BorderColor = c; }

        public void InitStaggeredBorders(List<SQ> sqList)
        {
            _sqList = sqList;
            foreach (SQ sq in _sqList)
            {
                _sq = sq;
                InitSides();
            }
        }
        private void InitSides()
        {
            //_sq.Tile.OverlayGrid.RemoveOutsideBorders();

            for (int row = -1; row <= 1; row++)
            {
                for (int col = -1; col <= 1; col++)
                {
                    if (row == 0 && col == 0) {; }
                    else if (!IsSqPresent((_sq.Row + row), (_sq.Col + col))) { InitBoxView(row, col); }
                    else if (Math.Abs(row) == 1 && Math.Abs(col) == 1)
                    {
                        if (!IsSqPresent(_sq.Row + row, _sq.Col) || !IsSqPresent(_sq.Row, _sq.Col + col))
                        { InitBoxView(row, col); }
                    }
                }
            }

            bool IsSqPresent(int row, int col) => (Coordinate.DoesSquareExist(row, col)) ?
                _sqList.Any(s => s.Key == Coordinate.GetSQKey(row, col)) : false;

            void InitBoxView(int r, int c)
            {
                r++;
                c++;

                //BoxView bv = new BoxView()
                //{
                //    Color = BorderColor,
                //    Opacity = 0.5,
                //};
                //CompressedLayout.SetIsHeadless(bv, true);
                //_sq.Tile.OverlayGrid.Children.Add(bv, c, r);
            }
        }
    }
}
