using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using System.Linq;
using Xamarin.Forms;
using TheManXS.Model.Company;
using TheManXS.Model.Map.Surface;

namespace TheManXS.ViewModel.MapBoardVM.Tiles
{
    public class StaggeredBorder
    {        
        private enum Sides {W,NW,N,NE,E,SE,S,SW,Total}
        public StaggeredBorder(Color c) { BorderColor = c; }
        private Color BorderColor;
        private SQ _sq;
        private List<SQ> _sqList;
        public void InitStaggeredBorders(List<SQ> sqList)
        {
            _sqList = sqList;
            foreach (SQ sq in _sqList)
            {
                _sq = sq;
                InitSidesOfOverLayGrid(GetSideBools(_sqList));
            }
        }
        private bool[,] GetSideBools()
        {
            bool[,] _borderIsActive = new bool[3, 3];




            return _borderIsActive;
        }
        private bool[] GetSideBools(List<SQ> sqList)
        {
            bool[] _activeBorders = new bool[(int)Sides.Total];

            // 0
            _activeBorders[D.NW] = _activeBorders[D.N] || _activeBorders[D.W] ||
               sqList.Any(s => s.Key == Coordinate.GetSQKey((_sq.Row - 1),(_sq.Col - 1))) ? true : false;
            // 1
            _activeBorders[D.W] = sqList.Any(s => s.Key == Coordinate.GetSQKey(_sq.Row, (_sq.Col - 1)));
            // 2
            _activeBorders[D.SW] = _activeBorders[D.S] || _activeBorders[D.W] ||
                sqList.Any(s => s.Key == Coordinate.GetSQKey((_sq.Row + 1),(_sq.Col - 1)));
            // 3
            _activeBorders[D.N] = sqList.Any(s => s.Key == Coordinate.GetSQKey((_sq.Row - 1), (_sq.Col)));      
            // 4                        
            _activeBorders[D.S] = sqList.Any(s => s.Key == Coordinate.GetSQKey((_sq.Row + 1), _sq.Col));
            // 5
            _activeBorders[D.NE] = _activeBorders[D.N] || _activeBorders[D.E] ||
                sqList.Any(s => s.Key == Coordinate.GetSQKey((_sq.Row - 1), (_sq.Col + 1))) ? true : false;
            // 6
            _activeBorders[D.E] = sqList.Any(s => s.Key == Coordinate.GetSQKey(_sq.Row,(_sq.Col + 1)));            
            // 7
            _activeBorders[D.SE] = _activeBorders[D.S] || _activeBorders[D.E] ||
                sqList.Any(s => s.Key == Coordinate.GetSQKey((_sq.Row + 1),(_sq.Col + 1)));            

            return _activeBorders;
        }
        private void InitSidesOfOverLayGrid(bool[] sqSides)
        {
            _sq.Tile.OverlayGrid.RemoveOutsideBorders();
            int index = 0;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (row == 1 && col == 1) { ; }
                    else if (!sqSides[index])
                    {                        
                        BoxView bv = new BoxView()
                        {
                            Color = BorderColor,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };
                        _sq.Tile.OverlayGrid.Children.Add(bv, col, row);
                        index++;
                    }                    
                }
            }
        }
    }
    public class D // directions
    {
        public static int NW = 0;
        public static int W = 1;
        public static int SW = 2;
        public static int N = 3;
        public static int S = 4;
        public static int NE = 5;
        public static int E = 6;
        public static int SE = 7;
    }
}
