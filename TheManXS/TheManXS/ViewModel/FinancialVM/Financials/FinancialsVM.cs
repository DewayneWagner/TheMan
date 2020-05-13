using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;
using TheManXS.ViewModel.FinancialVM.Financials.Charts;
using TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns;
using TheManXS.ViewModel.FinancialVM.Financials.Loans;
using TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class FinancialsVM : BaseViewModel
    {
        public enum LineItemType { CompanyNamesOrTurnNumber, BalanceSheets, Cash, PPE, TotalAssets, LongTermDebt, 
            TotalCapital, CashFlowStateMent, Revenue, OPEX, TheManCut, GrossProfitD, GrossProfitP, CAPEXCosts,
            DebtPayment, InterestExpense, NetProfitD, NetProfitP, CreditRating, InterestRate, StockPrice, Total }

        public enum DataPanelType { AllPlayers, Quarter, Ratios, PropertyBreakdown, Graphs, Loans }

        Game _game;
        public const int QDATACOLUMNS = 5;
        private PageService _pageServices;
        public FinancialsVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            DataPresentationArea = new ScrollView();
            CompressedLayout.SetIsHeadless(this, true);
            InitCommands();
            ButtonSize = QC.ScreenHeight / 6;
            DataPanel = DataPanelType.AllPlayers;
            _pageServices = new PageService();
        }

        public static double ColumnWidth = QC.ScreenWidth / 6;
        public double ButtonSize { get; set; }

        private ScrollView _dataPresentationArea;
        public ScrollView DataPresentationArea
        {
            get => _dataPresentationArea;
            set
            {
                _dataPresentationArea = value;
                SetValue(ref _dataPresentationArea, value);
            }
        }

        private DataPanelType _dataPanelType;
        public DataPanelType DataPanel
        {
            get => _dataPanelType;
            set
            {
                _dataPanelType = value;
                CreateNewDataPanel();
            }
        }

        public ICommand SinglePlayer { get; set; }
        public ICommand AllPlayers { get; set; }
        public ICommand Ratios { get; set; }
        public ICommand PropertyBreakdown { get; set; }
        public ICommand Graphs { get; set; }
        public ICommand Loans { get; set; }
        
        void CreateNewDataPanel()
        {
            switch (DataPanel)
            {
                case DataPanelType.AllPlayers:
                case DataPanelType.Quarter:
                    FinancialsLineItemsArray flia = new FinancialsLineItemsArray(_game, DataPanel);
                    DetailedBreakdown db = new DetailedBreakdown(_game, flia.GetArrayOfFinancialsLineItems(), false, false);
                    DataPresentationArea.Content = db.DetailedBreakdownGrid;
                    break;

                case DataPanelType.Ratios:
                    DataPresentationArea.Content = new FinancialRatiosGrid(_game);
                    break;
                case DataPanelType.PropertyBreakdown:
                    DataPresentationArea.Content = new PropertyBreakdownGrid(_game);
                    break;
                case DataPanelType.Graphs:
                    DataPresentationArea.Content = new FinancialChartsVM(_game);
                    break;
                case DataPanelType.Loans:
                    DetailedBreakdown dbl = new DetailedBreakdown(_game, DataPanelType.Loans);
                    DataPresentationArea.Content = dbl.DetailedBreakdownGrid;
                    break;
                default:
                    break;
            }
        }

        void InitCommands()
        {
            SinglePlayer = new Command(OnSinglePlayer);
            AllPlayers = new Command(OnAllPlayers);
            Ratios = new Command(OnRatios);
            PropertyBreakdown = new Command(OnPropertyBreakdown);
            Graphs = new Command(OnGraphs);
            Loans = new Command(OnLoans);
        }
        void OnSinglePlayer() => DataPanel = DataPanelType.Quarter;
        void OnAllPlayers() => DataPanel = DataPanelType.AllPlayers;
        void OnRatios() => DataPanel = DataPanelType.Ratios;
        void OnPropertyBreakdown() => DataPanel = DataPanelType.PropertyBreakdown;
        void OnGraphs() => DataPanel = DataPanelType.Graphs;
        void OnLoans() => DataPanel = DataPanelType.Loans;
    }
}
