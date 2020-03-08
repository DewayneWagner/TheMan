using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main
{
    public class EndTurnAction
    {
        Game _game;
        bool _isFullTurnAdvance;
        public EndTurnAction(Game game)
        {
            _game = game;
            setNextPlayer();

            if (_isFullTurnAdvance)
            {
                _game.TurnNumber++;
                setNextQuarter();
                _game.CommodityList.AdvancePricing();
                _game.FinancialValuesList = new FinancialValuesList(_game);
                _game.GameBoardVM.TickerVM.UpdateTicker();
            }
        }

        void setNextQuarter()
        {
            string newQuarter = new QuarterCalc(_game).GetNextQuarter();
            _game.Quarter = newQuarter;
            _game.GameBoardVM.TitleBar.Quarter = newQuarter;
            _isFullTurnAdvance = false;
        }
        
        void setNextPlayer()
        {
            int nextPlayerNumber = _game.ActivePlayer.Number + 1;
            if (nextPlayerNumber == QC.PlayerQ) { nextPlayerNumber = 0; }

            _game.ActivePlayer = _game.PlayerList[nextPlayerNumber];
            _isFullTurnAdvance = nextPlayerNumber == 0 ? true : false;
            _game.GameBoardVM.TitleBar.CompanyName = _game.ActivePlayer.Name;
            _game.GameBoardVM.TitleBar.CurrentPlayerCash = _game.PlayerList[_game.ActivePlayer.Number].Cash.ToString("c0");
        }
    }
}
