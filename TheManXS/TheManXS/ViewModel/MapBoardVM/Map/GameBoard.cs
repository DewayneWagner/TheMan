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
    public class GameBoard : Grid
    {
        private ActualGameBoardVM _actualGameBoardVM;
        public GameBoard(ActualGameBoardVM a)
        {
            CompressedLayout.SetIsHeadless(this, true);
            RowSpacing = 1;
            ColumnSpacing = 1;

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
        public AbsoluteLayout this[int row, int col]
        {
            get => this[row, col];
            set => this[row, col] = value;
        }
        private void InitGameBoard()
        {
            for (int row = 0; row < QC.RowQ; row++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

                if(row == 0)
                {
                    for (int col = 0; col < QC.ColQ; col++)
                    {
                        ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                    }
                }                
            }
        }
        private void AddFocusedTiles()
        {
            foreach (Tile tile in FocusedGameBoard)
            {
                this.Children.Add(tile, tile.Col, tile.Row);
            }
        }
    }
}
