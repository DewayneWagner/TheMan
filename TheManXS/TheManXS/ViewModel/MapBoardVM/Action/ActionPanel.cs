using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanel : BaseViewModel
    {
        private GameBoardVM _gameBoardVM;
        public ActionPanel()
        {
            _gameBoardVM = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            ActionPanelGrid = new ActionPanelGrid(this);

            ActionPanelGrid.BackgroundColor = Color.White;
            ActionPanelGrid.VerticalOptions = LayoutOptions.FillAndExpand;
            
            Content = ActionPanelGrid;
        }
        public ActionPanelGrid ActionPanelGrid { get; set; }
        public void CloseActionPanel()
        {
            var g = _gameBoardVM.ActualGameBoardVM.GameBoardSplitScreenGrid;
            g.Children.Remove(this);
            g.SidePanelExists = false;
            g.ColumnDefinitions.RemoveAt(1);
        }
    }    
}
