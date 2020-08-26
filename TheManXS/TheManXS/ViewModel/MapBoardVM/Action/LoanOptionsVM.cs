using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    
    class LoanOptionsVM : BaseViewModel
    {
        private enum LoanDisplayOptions
        {
            TermHeading, Term, InterestRateHeading, InterestRate, StartingBalanceHeading, StartingBalance,
            PrincipalPaymentPerTurnHeading, PrincipalPaymentPerTurn, InterestPaymentPerTurnHeading, InterestPaymentPerTurn
        }

        private const double _rowHeighRatio = 0.075;
        private static double _rowHeight;
        public LoanOptionsVM()
        {
            if(_rowHeight == 0) { _rowHeight = QC.ScreenHeight* _rowHeighRatio; }
        }

        private string _term;
        public string Term
        {
            get => _term;
            set
            {
                _term = value;
                SetValue(ref _term, value);
            }
        }

        private string _interestRate;
        public string InterestRate
        {
            get => _interestRate;
            set
            {
                _interestRate = value;
                SetValue(ref _interestRate, value);
            }
        }

        private string _startingBalance;
        public string StartingBalance
        {
            get => _startingBalance;
            set
            {
                _startingBalance = value;
                SetValue(ref _startingBalance, value);
            }
        }

        private string _principalPaymentPerTurn;
        public string PrincipalPaymentPerTurn
        {
            get => _principalPaymentPerTurn;
            set
            {
                _principalPaymentPerTurn = value;
                SetValue(ref _principalPaymentPerTurn, value);
            }
        }

        private string _interestPaymentPerTurn;
        public string InterestPaymentPerTurn
        {
            get => _interestPaymentPerTurn;
            set
            {
                _interestPaymentPerTurn = value;
                SetValue(ref _interestPaymentPerTurn, value);
            }
        }

        public Grid LoanOptionsGrid
        {
            get
            {
                Grid grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition() { Height = _rowHeight });
                grid.RowDefinitions.Add(new RowDefinition() { Height = _rowHeight });

                

                return grid;
            }
        }
    }
    class PlusMinuSelector : Grid
    {
        private const int _qCol = 4;

        private double[] _colWidthRatios = new double[_qCol] { 0.5, 0.1, 0.2, 0.1 };
        private double[] _colWidth = new double[_qCol];
        private List<string> _listOfPossibleValues;

        public PlusMinuSelector(string heading, ref string val, double width, List<string> listOfPossibleValues = null)
        {
            _listOfPossibleValues = listOfPossibleValues;
            InitColWidths(width);
            InitGrid();
            AddLabels(heading, ref val);
            InitButtons();

            NegativeButtonCommand = new Command(onNegativeButtonCommand);
            PositiveButtonCommand = new Command(onPositiveButtonCommand);
        }
        void InitColWidths(double width)
        {
            for (int col = 0; col < _qCol; col++) { _colWidth[col] = _colWidthRatios[col] * width; }
        }
        void InitGrid()
        {
            RowDefinitions.Add(new RowDefinition());
            for (int col = 0; col < _qCol; col++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = _colWidth[col] });
            }
        }
        void AddLabels(string heading, ref string val)
        {
            SidePanelLabel headingLabel = new SidePanelLabel(SidePanelLabel.LabelType.RowHeading, heading);
            SidePanelLabel valueLabel = new SidePanelLabel(SidePanelLabel.LabelType.Data, val);

            this.Children.Add(headingLabel, 0, 0);
            this.Children.Add(valueLabel, 2, 0);
        }
        void InitButtons()
        {
            NegativeButton = new Button()
            {
                Text = "-",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
            };
            this.Children.Add(NegativeButton, 1, 0);

            PositiveButton = new Button()
            {
                Text = "+",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
            };
            this.Children.Add(PositiveButton, 3, 0);
        }
        public Button NegativeButton { get; set; }
        public Button PositiveButton { get; set; }
        public ICommand NegativeButtonCommand { get; set; }
        public ICommand PositiveButtonCommand { get; set; }

        private void onNegativeButtonCommand()
        {
            if(_listOfPossibleValues == null)
            {

            }
        }
        private void onPositiveButtonCommand()
        {

        }
    }
}
