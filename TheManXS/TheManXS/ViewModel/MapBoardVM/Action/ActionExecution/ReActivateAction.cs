using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.MapBoardVM.Action.ActionPanelGrid;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class ReActivateAction
    {
        Game _game;
        PanelType _panelType;
        public ReActivateAction(Game game, PanelType panelType)
        {
            _game = game;
            _panelType = panelType;
            ExecuteReactivateAction();
        }
        private void ExecuteReactivateAction()
        {
            if (_panelType == PanelType.SQ)
            {
                _game.GameBoardVM.MapVM.ActiveSQ.Status = ST.Producing;
            }
            else
            {
                foreach (SQ sQ in _game.GameBoardVM.MapVM.ActiveUnit)
                {
                    sQ.Status = ST.Producing;
                }
            }
        }
    }
}
