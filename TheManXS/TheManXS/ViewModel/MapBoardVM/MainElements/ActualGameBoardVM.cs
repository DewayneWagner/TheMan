using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Map;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class ActualGameBoardVM : BaseViewModel
    {
        public ActualGameBoardVM(bool isForSettingUpNewGameBoard) { }
        public ActualGameBoardVM()
        {
            GameBoardVM g = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            g.ActualGameBoardVM = this;
            
            GameBoardSplitScreenGrid = new GameBoardSplitScreenGrid(this);

            Content = GameBoardSplitScreenGrid;

            GameBoardSplitScreenGrid.MapScrollView.InitChildrenClasses(this);
        }
        public GameBoardSplitScreenGrid GameBoardSplitScreenGrid { get; set; }
    }
       
}
