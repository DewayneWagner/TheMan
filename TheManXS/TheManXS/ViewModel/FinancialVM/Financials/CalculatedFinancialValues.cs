using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    class CalculatedFinancialValues
    {
        Game _game;
        Player _player;
        public CalculatedFinancialValues(Game game, Player player)
        {
            _game = game;
            _player = player;
            InitFinancialValues();
        }

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
        
        private void InitFinancialValues()
        {
            // for testing and set-up
            CAPEXThisTurn = 250;
            DebtPayment = 150;
            InterestExpense = 15;
            PPE = 1523;

            // final
            setRevenueAndOPEX();
            TheManCut = Revenue * QC.TheManCut;
            setGrossProfit();
            TotalAssets = PPE + _player.Cash;
            TotalCapital = TotalAssets - _player.Debt;

            setNetProfit();
            void setRevenueAndOPEX()
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
            void setGrossProfit()
            {
                GrossProfitD = Revenue - TotalOPEX - TheManCut;
                GrossProfitP = GrossProfitD / Revenue;
            }
            void setNetProfit()
            {
                NetProfitD = GrossProfitD - CAPEXThisTurn - DebtPayment - InterestExpense;
                NetProfitP = NetProfitD / Revenue;
            }
        }
    }
}
