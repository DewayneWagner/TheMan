using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class DevelopAction
    {
        Game _game;
        PanelType _panelType;
        // will need to incorporate turns until development at a later point
        // will need to incorporate budget fluctuations into here later
        public DevelopAction(Game game, PanelType pt)
        {
            _game = game;
            _panelType = pt;
            ExecuteDevelopmentAction();
        }
        private void ExecuteDevelopmentAction()
        {
            if (_panelType == PanelType.SQ)
            {
                _game.ActiveSQ.Status = ST.Producing;
            }
            else
            {
                foreach (SQ sq in _game.ActiveUnit)
                {
                    sq.Status = ST.Producing;
                }
            }
        }        
    }
}
