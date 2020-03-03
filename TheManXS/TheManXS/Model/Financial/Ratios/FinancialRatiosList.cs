using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.Model.Financial.Ratios
{
    class FinancialRatiosList : List<FinancialRatios>
    {
        public FinancialRatiosList(Player player, int numberOfTurns)
        {
            // gets multiple turns for one player

        }
        public FinancialRatiosList(Game game, bool currentRatiosForAllPlayers)
        {
            // gets ratios for all players for current turn

        }
        public FinancialRatiosList(Player player)
        {
            // gets ratios for player for current turn

        }
    }
}
