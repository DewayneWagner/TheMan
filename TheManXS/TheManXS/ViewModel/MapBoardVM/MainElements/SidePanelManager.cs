using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.MapBoardVM.TouchExecution;
using Xamarin.Forms;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class SidePanelManager
    {
        Game _game;
        public SidePanelManager(Game game)
        {
            _game = game;
            SelectedSQHighlight = new SelectedSQHighlight(true);
        }
        public SelectedSQHighlight SelectedSQHighlight { get; set; }
        public void AddSidePanel(PanelType panelType)
        {
            var g = _game.GameBoardVM;
            if (panelType == PanelType.LoanOptions)
            {

            }
            else
            {
                g.ActionPanelGrid = new ActionPanelGrid(panelType, _game);
                g.SplitScreenGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition()
                { Width = new GridLength(1, GridUnitType.Auto) });
                g.SplitScreenGrid.Children.Add(_game.GameBoardVM.ActionPanelGrid, 1, 0);
                g.SideSQActionPanelExists = true;
            }            
        }
        public void RemoveSidePanel(PanelType panelType)
        {
            var g = _game.GameBoardVM;
            g.SplitScreenGrid.Children.RemoveAt(1);
            g.SplitScreenGrid.ColumnDefinitions.RemoveAt(1);
            g.SideSQActionPanelExists = false;
            g.TouchEffectsEnabled = true;

            if (panelType == PanelType.Unit) { QC.UnitCounter--; }
            SelectedSQHighlight.RemoveSelectionHighlight();
        }
        public void ResetSidePanel(PanelType panelType)
        {
            RemoveSidePanel(panelType);
            _game.GameBoardVM.ActionPanelGrid = new ActionPanelGrid(panelType, _game);
        }
    }
}
