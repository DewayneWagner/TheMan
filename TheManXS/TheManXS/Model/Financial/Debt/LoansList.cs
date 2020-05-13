using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using LT = TheManXS.Model.ParametersForGame.LoanTermLength;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Financial.Debt
{
    public class LoansList : List<Loan>
    {
        private enum LoansListDisplayHeadings { Term, InterestRate, StartingBalance, PrincipalPayment, InterestPayment, TurnsRemaining, RemainingBalance, Total }
        private Game _game;
        public LoansList(Game game, bool isNewGame) 
        {
            _game = game;


            if (isNewGame) { InitLoansWithAtStartOfGame(); }
            else { InitLoansList(); }
        }
        private void InitLoansList()
        {
            using (DBContext db = new DBContext())
            {
                var loansList = db.Loans.Where(l => l.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                foreach (Loan loan in loansList)
                {
                    this.Add(loan);
                }
            }
        }
        public void InitLoansWithAtStartOfGame()
        {
            LT lt = LT.TwentyFive;
            double startingBalance = _game.ParameterConstantList.GetConstant(AllConstantParameters.CashConstant, (int)CashConstantSecondary.StartDebt);

            foreach (Player player in _game.PlayerList)
            {
                Loan startingLoan = new Loan(lt, startingBalance, _game);
                this.Add(startingLoan);
            }
        }
        public List<string> DisplayHeadingsList
        {
            get
            {
                List<string> displayHeadingsList = new List<string>();
                for (int i = 0; i < (int)LoansListDisplayHeadings.Total; i++)
                {
                    displayHeadingsList.Add(Convert.ToString((LoansListDisplayHeadings)i));
                }
                return displayHeadingsList;
            }
        }
    }
}
