using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;

namespace TheManXS.Model.Financial.Debt
{
    class PlayerDebt
    {
        private List<Loan> _loansList = new List<Loan>();
        private Game _game;
        private Player _player;
        public PlayerDebt(Game game, Player player)
        {
            _game = game;
            _player = player;
            CalculateProperties();
        }
        public double LongTermDebt { get; set; }
        public double DebtPayment { get; set; }
        public double InterestExpense { get; set; }
        public double WeightedAverageInterestRate { get; set; }
        public void CalculateProperties()
        {
            double debt = 0, debtPayment = 0, interestExpense = 0;
            List<Loan> loanList = _game.LoanList.Where(l => l.PlayerNumber == _player.Number).ToList();

            foreach (Loan loan in loanList)
            {
                debt += (loan.TotalInterestRemaining + loan.RemainingBalance);
                debtPayment += (loan.PrincipalPaymentPerTurn + loan.InterestPaymentPerTurn);
                interestExpense += (loan.InterestPaymentPerTurn);
            }

            InterestExpense = Double.IsNaN(interestExpense) ? 0 : interestExpense;
            double weightedAverageInterestRate = InterestExpense / debt;
            WeightedAverageInterestRate = Double.IsNaN(weightedAverageInterestRate) ? 0 : weightedAverageInterestRate;
            LongTermDebt = debt;
            DebtPayment = debtPayment;
        }
    }
}
