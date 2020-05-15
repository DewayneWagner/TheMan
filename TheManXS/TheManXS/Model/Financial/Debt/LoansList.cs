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
        public LoansList() { }

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
                startingLoan.PlayerNumber = player.Number;
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
                SplitWordsInHeading(ref displayHeadingsList);
                return displayHeadingsList;
            }
        }
        void SplitWordsInHeading(ref List<string> headingsList)
        {
            for (int i = 0; i < headingsList.Count; i++)
            {
                string s = SplitCamelCase(headingsList[i]);
                headingsList[i] = s;
            }
            string SplitCamelCase(string s)
            {
                return System.Text.RegularExpressions.Regex.Replace(s, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
        }
    }
}
