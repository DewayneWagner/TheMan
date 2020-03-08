using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static TheManXS.Model.Settings.SettingsMaster;
using TheManXS.Model.Financial.StockPrice;

namespace TheManXS.Model.Financial
{
    public class FinancialValues
    {
        Game _game;
        Player _player;
        private double _lastStockPrice;

        public FinancialValues() { }
        public FinancialValues(Game game, Player player)
        {
            _game = game;
            _player = player;
            _lastStockPrice = _player.StockPrice;
            PlayerNumber = _player.Number;
            TurnNumber = _game.TurnNumber;
            SavedGameSlot = QC.CurrentSavedGameSlot;

            CalculateFinancialValuesForLastQuarter();
        }

        public int ID { get; set; }
        public int SavedGameSlot { get; set; }
        public int PlayerNumber { get; set; }
        public int TurnNumber { get; set; }
        public double Cash { get; set; }
        public double Debt { get; set; }
        public double PPE { get; set; } 
        public double TotalAssets { get; set; } 
        public double TotalCapital { get; set; } 
        public double Revenue { get; set; } 
        public double TotalOPEX { get; set; } 
        public double TheManCut { get; set; } 
        public double GrossProfitD { get; set; } 
        public double GrossProfitP { get; set; } 
        public double CAPEXThisTurn { get; set; } 
        public double DebtPayment { get; set; }
        public double InterestExpense { get; set; } 
        public double NetProfitD { get; set; } 
        public double NetProfitP { get; set; }
        public double StockPrice { get; set; }
        public double StockPriceDelta { get; set; }
        public double InterestRate { get; set; }
        public string CreditRating { get; set; }

        private void CalculateFinancialValuesForLastQuarter()
        {
            // calculate cashflow
            SetRevenueAndOPEX();
            TheManCut = Revenue * QC.TheManCut;
            SetGrossProfit();
            SetNetProfit();

            // calculate balance sheet
            SetPPE();
            if (NetProfitD > 0)
            {
                /* if capex isn't added back in - it would be double-dipped due to action buttons subtracting 
                capex costs for actions live */

                Cash += (NetProfitD + CAPEXThisTurn); 
                Debt -= DebtPayment;
            }
            else
            {
                Cash = 0;
                Debt += NetProfitD;
                Debt -= DebtPayment;
            }
            _game.PlayerList[_game.ActivePlayer.Number].Cash = Cash;
            TotalAssets = (PPE + Cash);
            TotalCapital = (TotalAssets - Debt);

            SetCreditRatingAndInterestRate();
            SetInterestExpense();
            SetStockPrice();
        }
        void SetRevenueAndOPEX()
        {
            double rev = 0;
            double opex = 0;

            foreach (KeyValuePair<int, SQ> sq in _game.SquareDictionary)
            {
                if (sq.Value.OwnerNumber == _player.Number && sq.Value.Status == ST.Producing)
                {
                    rev += (sq.Value.Production * _game.CommodityList[(int)sq.Value.ResourceType].Price);
                    opex += sq.Value.OPEXPerUnit * sq.Value.Production;
                }
            }
            Revenue = rev;
            TotalOPEX = opex;
        }
        void SetGrossProfit()
        {
            GrossProfitD = Revenue - TotalOPEX - TheManCut;
            GrossProfitP = GrossProfitD / Revenue;
        }
        void SetNetProfit()
        {
            NetProfitD = GrossProfitD - CAPEXThisTurn - DebtPayment - InterestExpense;
            NetProfitP = NetProfitD / Revenue;
        }
        void SetStockPrice()
        {
            Stock s = new Stock(this,_lastStockPrice);
            StockPrice = s.Price;
            StockPriceDelta = s.Delta;
            _player.StockPrice = s.Price;
            _player.Delta = s.Delta;
        }
        void SetCreditRatingAndInterestRate()
        {
            CreditRating cr = new CreditRating(this, _player);
            CreditRating = Convert.ToString(cr.Rating);
            _player.CreditRating = cr.Rating;
            InterestRate = cr.InterestRate;
            _player.InterestRate = cr.InterestRate;
        }
        void SetInterestExpense()
        {
            // need to build a class that sets debt payment amount and interest when debt is incurred, and 
            // increased / decreased when debt is added / paid-down.
            // temporary calculation below - that will change every turn.

            int amortizationPeriod = 25;
            DebtPayment = Debt / amortizationPeriod;
            InterestExpense = DebtPayment * InterestRate;
        }
        void SetPPE()
        {
            PPE p = new PPE(_game, _player);
            PPE = p.Valuation;
        }
    }
    
    public class FinancialValuesDBConfig : IEntityTypeConfiguration<FinancialValues>
    {
        public void Configure(EntityTypeBuilder<FinancialValues> builder)
        {
            builder.Property(p => p.ID).ValueGeneratedOnAdd();
        }
    }

}
