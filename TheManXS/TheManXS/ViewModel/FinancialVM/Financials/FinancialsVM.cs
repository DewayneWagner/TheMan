using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class FinancialsVM : BaseViewModel
    {
        public enum LineItemType { BalanceSheets, Assets, Cash, PPE, TotalAssets, Liabilities, LongTermDebt, 
            TotalCapital, CashFlowStateMent, Revenue, Expenses, OPEX, TheManCut, GrossProfitD, 
            GrossProfitP, CAPEXCosts, DebtPayment, InterestExpense, NetProfitD, NetProfitP, Total }

        public enum FormatTypes { MainHeading, SubHeading, LineItem, Totals, }
        public enum FinancialsGridType { OnePlayerOneTurn, OnePlayer5Turns, AllPlayers }

        Game _game;
        public const int QDATACOLUMNS = 5;
        public FinancialsVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            FinancialsGrid = new FinancialsGrid(_game,new FinancialsLineItemsList(_game).GetListOfFinancialsLineItems());
        }

        private FinancialsGrid _financialsGrid;
        public FinancialsGrid FinancialsGrid
        {
            get => _financialsGrid;
            set
            {
                _financialsGrid = value;
                SetValue(ref _financialsGrid, value);
            }
        }
    }
}
