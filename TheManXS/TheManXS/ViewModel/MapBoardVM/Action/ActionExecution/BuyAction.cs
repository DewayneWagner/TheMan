using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.ViewModel.MapBoardVM.Action.ActionExecution
{
    class BuyAction : SQAction
    {
        public BuyAction(Game game, ActionPanelGrid.PanelType pt) : base(game,pt)
        {
            TransferOwnershipAndUpdateStatus();
        }        

        void TransferOwnershipAndUpdateStatus()
        {
            if(PanelType == ActionPanelGrid.PanelType.SQ)
            {
                var sq = Game.ActiveSQ;
                var p = Game.ActivePlayer;

                if(PlayerHasEnoughDough(sq.NextActionCost))
                {
                    sq.OwnerName = Game.ActivePlayer.Name;
                    sq.OwnerNumber = Game.ActivePlayer.Number;
                    sq.Status = ST.Unexplored;

                    Game.FinancialValuesList[p.Number].CAPEXThisTurn += sq.NextActionCost;
                }
            }
            else
            {                
                var u = Game.ActiveUnit;
                if (PlayerHasEnoughDough(u.NextActionCost))
                {
                    u.PlayerName = Game.ActivePlayer.Name;
                    u.PlayerNumber = Game.ActivePlayer.Number;
                    u.Status = ST.Unexplored;

                    Game.ActivePlayer.Cash += u.NextActionCost;
                }
               
            }
        }
    }
}
