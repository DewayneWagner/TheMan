using System;
using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Financial.Debt;
using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchExecution;
using Xamarin.Forms;
using static TheManXS.Model.Financial.Debt.Loan;
using static TheManXS.ViewModel.MapBoardVM.Action.SidePanelLabel;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanelGrid : Grid
    {
        public enum PanelType { SQ, Unit, LoanOptions }
        private enum LoanOptionsHeadings { Heading, CashRequired, ProposedTerm, TotalPaymentPerTurn, 
            InterestPaymentPerTurn, CurrentCreditRating, LoanInterestRate, ActionButton, Total }

        MapVM _mapVM;
        Game _game;
        double _loanAmountRequired;
        LoanTermLength _loanTermLength;

        public enum ActionRows
        {
            TitleAndBackButton, Owner, Status, Resource, Production, Revenue, OPEX, TransportCost,
                 GrossProfitD, GrossProfitP, ActionCost, Button, UnitOPEXDiscount, UnitActionCostDiscount
        }

        private int _quantityOfRowsInSQ = (int)ActionRows.Button + 1;
        private int _quantityOfRowsInUnit = (int)ActionRows.UnitActionCostDiscount + 1;
        private double _buttonAndTitleHeight = 35;

        private double _column1Width;
        private double _column2Width;
        private const double _widthRatioColumn1 = 0.4;

        private const int SIDEPANELWIDTH = 300;
        private PanelType _panelType;

        public ActionPanelGrid(PanelType pt, Game game, double loanAmountRequired = 0, LoanTermLength loanTermLength = LoanTermLength.TwentyFive)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);
            _panelType = pt;
            _loanAmountRequired = loanAmountRequired;
            _loanTermLength = loanTermLength;
            _mapVM = _game.GameBoardVM.MapVM;

            WidthRequest = SIDEPANELWIDTH;
            SetPropertiesOfGrid();
            InitFields();
            InitAllElements();

            _game.GameBoardVM.SidePanelManager.SelectedSQHighlight = new SelectedSQHighlight(_game, pt);
        }

        private void SetPropertiesOfGrid()
        {
            RowSpacing = 0;
            Padding = 0;
            VerticalOptions = LayoutOptions.Center;
            BackgroundColor = Color.White;
        }

        private void InitFields()
        {
            WidthRequest = QC.WidthOfActionPanel;
            _column1Width = QC.WidthOfActionPanel * _widthRatioColumn1;
            _column2Width = QC.WidthOfActionPanel - _column1Width;
        }

        private void InitAllElements()
        {
            InitGrid();
            InitDataRows();
            InitBackButton();
            InitTitle();
            InitActionButton();
        }

        public void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column1Width });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column2Width });

            if (_panelType == PanelType.SQ) { for (int i = 0; i < _quantityOfRowsInSQ; i++) { RowDefinitions.Add(new RowDefinition()); } }

            else if (_panelType == PanelType.Unit)
            {
                for (int i = 0; i < _quantityOfRowsInUnit; i++) { RowDefinitions.Add(new RowDefinition()); }
            }

            else if (_panelType == PanelType.LoanOptions)
            {
                for (int i = 0; i < (int)LoanOptionsHeadings.Total; i++) { RowDefinitions.Add(new RowDefinition()); }
            }
        }

        public void InitDataRows()
        {
            if (_panelType == PanelType.SQ)
            {
                SqAttributes sqAttributes = new SqAttributes(_game);

                for (int i = (int)ActionRows.Owner; i <= (int)ActionRows.ActionCost; i++)
                {
                    this.Children.Add(new SidePanelLabel(_game,
                        LabelType.RowHeading,
                        SplitCamelCase(Convert.ToString(((ActionRows)i)))), 
                        0, i);

                    this.Children.Add(new SidePanelLabel(_game,
                        LabelType.Data,
                        sqAttributes.GetValue((SqAttributes.AllSQAttributes)(i - (int)ActionRows.Owner))),
                        1, i);
                }
            }
            else if (_panelType == PanelType.Unit)
            {
                UnitAttributes unitAttributes = new UnitAttributes(_game.ActiveUnit);

                for (int i = (int)ActionRows.Owner; i <= (int)ActionRows.ActionCost; i++)
                {
                    this.Children.Add(new SidePanelLabel(_game,
                        LabelType.RowHeading,
                        SplitCamelCase(Convert.ToString((ActionRows)i))), 0, i);

                    this.Children.Add(new SidePanelLabel(_game,
                        LabelType.Data,
                        unitAttributes.GetValue((UnitAttributes.AllUnitAttributes)(i - (int)ActionRows.Owner))),
                        1, i);
                }
            }
            else if(_panelType == PanelType.LoanOptions)
            {
                Loan loan = new Loan(_loanTermLength, _loanAmountRequired, _game);

                for (int i = 0; i < (int)LoanProperties.Total; i++)
                {
                    this.Children.Add(new SidePanelLabel(_game,
                        LabelType.RowHeading,
                        SplitCamelCase(Convert.ToString((LoanProperties)i))), 
                        0, i);

                    this.Children.Add(new SidePanelLabel(_game,
                        LabelType.Data,
                        loan.GetValue((LoanProperties)i)),
                        1, i);
                }
            }
        }

        public void InitBackButton()
        {
            BackButtonX backButton = new BackButtonX();

            Children.Add(backButton, 1, (int)ActionRows.TitleAndBackButton);
            backButton.Clicked += OnBackButton;
        }

        public void OnBackButton(object sender, EventArgs e) => _game.GameBoardVM.SidePanelManager.RemoveSidePanel(_panelType);

        public void InitTitle()
        {            
            Children.Add(new SidePanelLabel(_game, 
                SidePanelLabel.LabelType.TopHeading, 
                SplitCamelCase(Convert.ToString(_panelType))));
        }

        public void InitActionButton()
        {
            ActionButton actionButton = new ActionButton(_game, _panelType)
            {
                HeightRequest = _buttonAndTitleHeight,
                Margin = (_buttonAndTitleHeight * 0.1),
                FontSize = (_buttonAndTitleHeight * 0.5),
            };
            Children.Add(actionButton, 0, (int)ActionRows.Button);
            Grid.SetColumnSpan(actionButton, 2);
        }
        public string SplitCamelCase(string s)
        {
            return System.Text.RegularExpressions.Regex.Replace(s, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}
