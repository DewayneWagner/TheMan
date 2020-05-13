using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using CR = TheManXS.Model.ParametersForGame.CreditRatings;
using QC = TheManXS.Model.Settings.QuickConstants;
using AP = TheManXS.Model.ParametersForGame.AllConstantParameters;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Financial.Debt
{
    public class Loan
    {        
        public enum LoanStatusTypes { Proposed, Approved }
        private Game _game;
        private int _term;
        public Loan() { }

        public Loan(LoanTermLength term, double startingBalance, Game game)
        {
            _game = game;
            Term = term;
            _term = (int)Term;
            InterestRate = SetInterestRate();
            StartingBalance = startingBalance;
            PrincipalPaymentPerTurn = StartingBalance / (int)Term; ;
            PlayerNumber = _game.ActivePlayer.Number;
            SavedGameSlot = QC.CurrentSavedGameSlot;
            LoanStatus = LoanStatusTypes.Proposed;
        }

        // set in constructor
        public LoanTermLength Term { get; set; }        
        public int TurnIssued { get; set; }
        public double InterestRate { get; set; }
        public double StartingBalance { get; set; }
        public int PlayerNumber { get; set; }
        public int SavedGameSlot { get; set; }
        public double PrincipalPaymentPerTurn { get; set; }

        // Calculated, and not in DB
        public double RemainingBalance => StartingBalance / _term * (_game.TurnNumber - TurnIssued);
        public double InterestPaymentPerTurn => RemainingBalance / TurnsRemaining * InterestRate;
        public int TurnsRemaining => TurnIssued + (int)Term - _game.TurnNumber;
        
        private LoanStatusTypes _loanStatus;
        public LoanStatusTypes LoanStatus
        {
            get => _loanStatus;
            set
            {
                _loanStatus = value;
                if(_loanStatus == LoanStatusTypes.Approved) { AddLoanToDB(); }
            }
        }
        private double SetInterestRate()
        {
            return _game.PrimeInterestRate + _game.ParameterConstantList.GetConstant(AP.PrimeRateAdderBasedOnCreditRating, (int)_game.ActivePlayer.CreditRating);
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
        }
    }
}
