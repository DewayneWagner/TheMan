using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.Services;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardGridVM : BaseViewModel
    {
        public GameBoardGridVM()
        {
            Content = GameBoardGrid = new GameBoardGrid(this);
        }
        public GameBoardGrid GameBoardGrid { get; set; }        
        
    }
}
