using System;
using System.Collections.Generic;
using System.Text;
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
                setNextQuarter();
                _game.CommodityList.AdvancePricing();
            }
        }

        void setNextQuarter()
        {
            string newQuarter = _game.Quarter;
            string year = _game.Quarter.Substring(0, 4);
            string quarter = _game.Quarter.Substring(6);

            bool successfulParseOfQuarter = Int32.TryParse(quarter, out int quarterNumber);
            bool successfulParseOfYear = Int32.TryParse(year, out int yearNumber);

            if (successfulParseOfQuarter && successfulParseOfYear)
            {
                int newQuarterNumber = quarterNumber != 4 ? quarterNumber++ : 1;
                int newYearNumber = newQuarterNumber == 1 ? yearNumber++ : yearNumber;
                newQuarter = Convert.ToString(newYearNumber) + "-Q" + Convert.ToString(newQuarterNumber);
            }
            _game.Quarter = newQuarter;
            _game.GameBoardVM.TitleBar.Quarter = newQuarter;
        }
        
        void setNextPlayer()
        {
            int currentPlayerIndex = _game.PlayerList.IndexOf(_game.ActivePlayer);
            int nextPlayerIndex = currentPlayerIndex == (_game.PlayerList.Count - 1) ? 0 : currentPlayerIndex++;
            _isFullTurnAdvance = nextPlayerIndex == QC.PlayerIndexActual ? true : false;
            _game.ActivePlayer = _game.PlayerList[nextPlayerIndex];
            _game.GameBoardVM.TitleBar.CompanyName = _game.ActivePlayer.Name;
        }
    }
}
