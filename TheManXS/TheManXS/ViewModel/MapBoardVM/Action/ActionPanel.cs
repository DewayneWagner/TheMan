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
        public enum PanelType { SQ, Unit }
        private GameBoardVM _gameBoardVM;
        public ActionPanel(PanelType pt)
        {
            _gameBoardVM = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            CompressedLayout.SetIsHeadless(this, true);
            Content = ActionPanelGrid = new ActionPanelGrid(this, pt);
        }
        public ActionPanelGrid ActionPanelGrid { get; set; }
        public void CloseActionPanel()
        {
            var g = _gameBoardVM.ActualGameBoardVM.GameBoardSplitScreenGrid;
            g.Children.Remove(this);
            g.SideSQActionPanelExists = false;
            g.ColumnDefinitions.RemoveAt(1);
        }
    }    
}
