using System;
using System.Collections.Generic;
using System.Text;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Financial.Debt;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Main
{
    class DebugTesting
    {
        Game _game;
        static System.Random rnd = new System.Random();
        public DebugTesting(Game game)
        {
            _game = game;
            InitSQsForTesting();
        }
        public static void InitExtraLoansForEachPlayer(Game game)
        {
            int lb = 500;
            int ub = 2500;
            double loanAmount;
            LoanTermLength term;

            foreach(Player p in game.PlayerList)
            {
                for (int i = 0; i < 3; i++)
                {
                    term = (LoanTermLength)(rnd.Next(0, (int)LoanTermLength.Total));
                    loanAmount = rnd.Next(lb, ub);
                    Loan loan = new Loan(term, loanAmount, game);
                    loan.PlayerNumber = p.Number;
                    game.LoanList.Add(loan);
                }
            }
        }
        private void InitSQsForTesting()
        {
            int row = 2;
            int col = 2;

            var s = _game.SQList[row, col];

            s.ResourceType = RT.Oil;
            s.Production = rnd.Next(5, 20);
            s.OPEXPerUnit = rnd.Next(15, 35);
            s.FormationID = 50;
            s.Transport = rnd.Next(5, 20);
            s.Status = ST.Producing;
            s.OwnerNumber = QC.PlayerIndexActual;

            foreach (Player player in _game.PlayerList)
            {
                int productingSQsForPlayer = 0;
                int loopCounter = 0;
                do
                {
                    SQ sq = _game.SQList[rnd.Next(0, QC.RowQ), rnd.Next(0, QC.ColQ)];
                    loopCounter++;

                    if (sq.OwnerNumber == QC.PlayerIndexTheMan
                        && sq.Production > 0
                        && sq.Status == ST.Nada
                        && sq.ResourceType != RT.RealEstate
                        && (int)sq.ResourceType < (int)RT.Nada)
                    {
                        sq.OwnerNumber = player.Number;
                        sq.OwnerName = player.Name;
                        sq.Status = ST.Producing;
                        productingSQsForPlayer++;
                    }

                } while (loopCounter < 50 && productingSQsForPlayer < 3);
            }
        }
    }
}
