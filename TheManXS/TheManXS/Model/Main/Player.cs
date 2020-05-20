using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkiaSharp;
using System.Linq;
using TheManXS.Model.Financial.Debt;
using TheManXS.Model.ParametersForGame;
using TheManXS.Model.Services.EntityFrameWork;

namespace TheManXS.Model.Main
{
    public class Player
    {
        public Player() { }

        public int Key { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public double Cash { get; set; }
        public double Debt { get; set; }
        public double StockPrice { get; set; }
        public double Delta { get; set; }
        public CreditRatings CreditRating { get; set; }
        public double InterestRate { get; set; }
        public bool IsComputer { get; set; }

        private SKColor _skColor;
        public SKColor SKColor
        {
            get => _skColor;
            set
            {
                _skColor = value;
                ColorString = _skColor.ToString();
            }
        }

        public string ColorString { get; set; }
        public int SavedGameSlot { get; set; }

        public LoansList ListOfLoans
        {
            get
            {
                LoansList loansList = new LoansList();
                using (DBContext db = new DBContext())
                {
                    var list = db.Loans.Where(l => l.PlayerNumber == Number).ToList();
                    foreach (Loan loan in list)
                    {
                        loansList.Add(loan);
                    }
                }
                return loansList;
            }
        }
    }
    public class PlayerDBConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(p => p.Key);

            builder.Property(p => p.Ticker).HasMaxLength(3);

            builder.Ignore(p => p.SKColor);

            builder.Ignore(p => p.ListOfLoans);

            builder.Property(p => p.CreditRating)
                .HasConversion(new EnumToStringConverter<CreditRatings>());
        }
    }
}
