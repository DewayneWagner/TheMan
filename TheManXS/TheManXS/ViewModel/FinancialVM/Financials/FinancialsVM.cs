using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

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
            InitCommands();
            ButtonSize = QC.ScreenHeight / 5;

            FinancialsScrollView = new ScrollView();
            FinancialsScrollView.Content = new FinancialsGrid(_game,new FinancialsLineItemsArray(_game).GetArrayOfFinancialsLineItems());
        }

        private ScrollView _financialsScrollView;
        public ScrollView FinancialsScrollView
        {
            get => _financialsScrollView;
            set
            {
                _financialsScrollView = value;
                SetValue(ref _financialsScrollView, value);
            }
        }

        //private FinancialsGrid _financialsGrid;
        //public FinancialsGrid FinancialsGrid
        //{
        //    get => _financialsGrid;
        //    set
        //    {
        //        _financialsGrid = value;
        //        SetValue(ref _financialsGrid, value);
        //    }
        //}

        public ICommand SinglePlayer { get; set; }
        public ICommand AllPlayers { get; set; }
        public ICommand ResourceBreakdown { get; set; }
        public ICommand Ratios { get; set; }
        public double ButtonSize { get; set; }

        void InitCommands()
        {
            SinglePlayer = new Command(OnSinglePlayer);
            AllPlayers = new Command(OnAllPlayers);
            ResourceBreakdown = new Command(OnResourceBreakdown);
            Ratios = new Command(OnRatios);
        }
        void OnSinglePlayer()
        {

        }
        void OnAllPlayers()
        {

        }
        void OnResourceBreakdown()
        {

        }
        void OnRatios()
        {

        }
    }
}
