using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Scroll;

namespace TheManXS.ViewModel.MapBoardVM.Map
{
    public class GameBoard : AbsoluteLayout
    {
        private ActualGameBoardVM _actualGameBoardVM;
        public GameBoard(ActualGameBoardVM a)
        {
            CompressedLayout.SetIsHeadless(this, true);
            _actualGameBoardVM = a;
            FocusedGameBoard = new FocusedABS(_actualGameBoardVM);
            InitGameBoard();
            AddFocusedTiles();
        }
        private FocusedABS _focusedGameBoard;
        public FocusedABS FocusedGameBoard
        {
            get => _focusedGameBoard;
            set
            {
                _focusedGameBoard = value;
                _actualGameBoardVM.SetValue(ref _focusedGameBoard, value);
            }
        }
        private void InitGameBoard()
        {
            Rectangle rect = new Rectangle(0, 0, QC.SqSize * QC.ColQ, QC.SqSize * QC.RowQ);
            BoxView bv = new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Black
            };
            this.Children.Add(bv, rect);
        }
        private void AddFocusedTiles()
        {
            foreach(Tile tile in FocusedGameBoard)
            {
                Rectangle rect = new Rectangle(tile.XCoord, tile.YCoord, QC.SqSize, QC.SqSize);
                this.Children.Add(tile, rect);
                tile.MapIndex = Children.IndexOf(tile);
            }
        }
    }
}
