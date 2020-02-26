using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class BuyAction
    {
        Game _game;
        ActionPanelGrid.PanelType _panelType;

        public BuyAction(Game game, ActionPanelGrid.PanelType pt)
        {
            _game = game;
            _panelType = pt;
            TransferOwnershipAndUpdateStatus();
        }        

        void TransferOwnershipAndUpdateStatus()
        {
            if(_panelType == ActionPanelGrid.PanelType.SQ)
            {
                var sq = _game.ActiveSQ;
                sq.OwnerName = _game.ActivePlayer.Name;
                sq.OwnerNumber = _game.ActivePlayer.Number;
                sq.Status = ST.Unexplored;
            }
            else
            {
                var u = _game.ActiveUnit;
                u.PlayerName = _game.ActivePlayer.Name;
                u.PlayerNumber = _game.ActivePlayer.Number;
                u.Status = ST.Unexplored;
            }
        }
    }
}
