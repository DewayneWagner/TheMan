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
        MapVM _mapVM;
        public ActionPanel(PanelType pt, MapVM mapVM)
        {
            _mapVM = mapVM;
            CompressedLayout.SetIsHeadless(this, true);
            Content = ActionPanelGrid = new ActionPanelGrid(this, pt, _mapVM);
        }
        public ActionPanelGrid ActionPanelGrid { get; set; }
        public void CloseActionPanel()
        {
            // need to figure-out where this should live - somehow link to code-behind?
            //_gameBoardSplitScreenGrid.Children.Remove(this);
            //_gameBoardSplitScreenGrid.SideSQActionPanelExists = false;
            //_gameBoardSplitScreenGrid.ColumnDefinitions.RemoveAt(1);
        }
    }    
}
