using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanel : BaseViewModel
    {
        public enum PanelType { SQ, Unit }
        GameBoardSplitScreenGrid _gameBoardSplitScreenGrid;
        public ActionPanel(PanelType pt, GameBoardSplitScreenGrid gameBoardSplitScreenGrid)
        {
            _gameBoardSplitScreenGrid = gameBoardSplitScreenGrid;
            CompressedLayout.SetIsHeadless(this, true);
            Content = ActionPanelGrid = new ActionPanelGrid(this, pt, _gameBoardSplitScreenGrid);
        }
        public ActionPanelGrid ActionPanelGrid { get; set; }
        public void CloseActionPanel()
        {
            _gameBoardSplitScreenGrid.Children.Remove(this);
            _gameBoardSplitScreenGrid.SideSQActionPanelExists = false;
            _gameBoardSplitScreenGrid.ColumnDefinitions.RemoveAt(1);
        }
    }    
}
