using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;
using TheManXS.Model.Services.EntityFrameWork;
using AP = TheManXS.Model.ParametersForGame.AllConstantParameters;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Financial.Debt
{
    public class Loan
    {
        public enum LoanStatusTypes { Proposed, Approved }
        private Game _game;
        public Loan() { }

        public Loan(LoanTermLength term, double startingBalance, Game game)
        {
            _game = game;
            Term = (int)(term + 1) * 5;
            InterestRate = SetInterestRate();
            StartingBalance = startingBalance;
            PrincipalPaymentPerTurn = StartingBalance / Term; ;
            PlayerNumber = _game.ActivePlayer.Number;
            SavedGameSlot = QC.CurrentSavedGameSlot;
            LoanStatus = LoanStatusTypes.Proposed;
        }

        // set in constructor
        public int Term { get; set; }
        public int TurnIssued { get; set; }
        public double InterestRate { get; set; }
        public double StartingBalance { get; set; }
        public int PlayerNumber { get; set; }
        public int SavedGameSlot { get; set; }
        public double PrincipalPaymentPerTurn { get; set; }

        // Calculated, and not in DB        
        public int TurnsRemaining => TurnIssued + Term - _game.TurnNumber;
        public double RemainingBalance
        {
            get
            {
                int turnNumber = _game.TurnNumber == 0 ? 1 : _game.TurnNumber;
                return TurnsRemaining * PrincipalPaymentPerTurn;
            }
        }
        public double TotalInterestRemaining => RemainingBalance * InterestRate * TurnsRemaining;
        public double InterestPaymentPerTurn => TotalInterestRemaining / TurnsRemaining;

        private LoanStatusTypes _loanStatus;
        public LoanStatusTypes LoanStatus
        {
            get => _loanStatus;
            set
            {
                _loanStatus = value;
                if (_loanStatus == LoanStatusTypes.Approved) { AddLoanToDB(); }
            }
        }
        private double SetInterestRate()
        {
            return _game.PrimeInterestRate + (_game.ParameterConstantList.GetConstant(AP.PrimeRateAdderBasedOnCreditRating, (int)_game.ActivePlayer.CreditRating) / 100);
        }
        private void AddLoanToDB()
        {
            using (DBContext db = new DBContext())
            {
                db.Loans.Add(this);
                db.SaveChanges();
            }
        }
    }
    public class LoanDBConfig : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasNoKey();
            builder.Ignore(l => l.TurnsRemaining);
            builder.Ignore(l => l.RemainingBalance);
            builder.Ignore(l => l.InterestPaymentPerTurn);
            builder.Ignore(l => l.TurnsRemaining);
            builder.Ignore(l => l.TotalInterestRemaining);
        }
    }
}
