using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class SuspendAction
    {
        Game _game;
        PanelType _panelType;
        public SuspendAction(Game game, PanelType panelType)
        {
            _game = game;
            _panelType = panelType;
            ExecuteSuspendAction();
        }
        private void ExecuteSuspendAction()
        {
            if (_panelType == PanelType.SQ)
            {
                _game.ActiveSQ.Status = ST.Suspended;
            }
            else
            {
                foreach (SQ sq in _game.ActiveUnit)
                {
                    sq.Status = ST.Suspended;
                }
            }
        }
    }
}
