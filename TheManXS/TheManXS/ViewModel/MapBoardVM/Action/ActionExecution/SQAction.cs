using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    abstract class SQAction
    {

        public SQAction(Game game, PanelType panelType)
        {
            Game = game;
            PanelType = PanelType;
        }
        public SQAction(Game game)
        {
            Game = game;
        }
        protected Game Game { get; set; }
        protected PanelType PanelType { get; set; }
        protected PageService PageServices { get; set; } = new PageService();
        protected bool PlayerHasEnoughDough(double actionCost)
        {
            if (Game.ActivePlayer.Cash >= actionCost) { return true; }
            else { return false; }
        }
        private void DisplayLoanView()
        {

        }

    }
}
