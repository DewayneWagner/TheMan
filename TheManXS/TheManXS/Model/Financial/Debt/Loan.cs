using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using CR = TheManXS.Model.ParametersForGame.CreditRatings;

namespace TheManXS.Model.Financial.Debt
{
    public class Loan
    {
        
        private Game _game;
        public Loan() { }

        // this would be for calculating a loan for a player evaluating loan options
        public Loan(double cashRequired, CR cr, int term)
        {

        }
        public Loan(int term, double interestRate, Game game)
        {
            _game = game;
            Term = term;
            InterestRate = interestRate;
            PlayerNumber = _game.ActivePlayer.Number;
            TurnIssued = _game.TurnNumber;
            CalculatePrincipalAndInterestPayments();
        }

        public int PlayerNumber { get; set; }
        public double StartingBalance { get; set; }
        public double RemainingBalance { get; set; }
        public int Term { get; set; }
        public int TurnIssued { get; set; }
        public double InterestRate { get; set; }
        public double PrincipalPaymentPerTurn { get; set; }
        public double  InterestPaymentPerTurn { get; set; }

        public int TurnsRemaining => TurnIssued + Term - _game.TurnNumber;
        private void CalculatePrincipalAndInterestPayments()
        {

        }
        private void UpdateRemainingBalance()
        {

        }
        
    }
    public class LoanDBConfig : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasNoKey();
            builder.Ignore(l => l.TurnsRemaining);
        }
    }
}
