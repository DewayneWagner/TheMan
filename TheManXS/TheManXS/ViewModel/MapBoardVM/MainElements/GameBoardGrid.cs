using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.MapBoardVM.Action;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardGrid : Grid
    {
        GameBoardGridVM _gameBoardGridVM;
        public GameBoardGrid(GameBoardGridVM gameBoardGridVM)
        {
            _gameBoardGridVM = gameBoardGridVM;
        }

        private ActionPanelGrid _actionPanelGrid;
        public ActionPanelGrid ActionPanelGrid
        {
            get => _actionPanelGrid;
            set
            {
                _actionPanelGrid = value;
                _gameBoardGridVM.SetValue(ref _actionPanelGrid, value);
            }
        }

        private MapVM _mapVM;
        public MapVM MapVM
        {
            get => _mapVM;
            set
            {
                _mapVM = value;
                _gameBoardGridVM.SetValue(ref _mapVM, value);
            }
        }
    }
}
