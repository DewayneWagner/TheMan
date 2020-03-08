using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;

namespace TheManXS.Model.Financial.Debt
{
    public class LongTermDebt
    {
        Game _game;
        Player _player;
        public LongTermDebt(Game game, Player player)
        {
            if(_game.TurnNumber == 1) { SetStartingDebt(); }
            else { CalculateLongTermDebtBalance(); }
        }
        private void SetStartingDebt()
        {

        }
        private void CalculateLongTermDebtBalance ()
        {

        }

    }
}
