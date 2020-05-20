using System;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.MainElements;
using TheManXS.ViewModel.MapBoardVM.TouchExecution;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanelGrid : Grid
    {
        public enum PanelType { SQ, Unit, LoanOptions }
        MapVM _mapVM;
        Game _game;

        public enum ActionRows
        {
            TitleAndBackButton, Owner, Status, Resource, Production, Revenue, OPEX, TransportCost,
            GrossProfitD, GrossProfitP, ActionCost, Button, UnitOPEXDiscount, UnitActionCostDiscount
        }

        private int _quantityOfRowsInSQ = (int)ActionRows.Button + 1;
        private int _quantityOfRowsInUnit = (int)ActionRows.UnitActionCostDiscount + 1;

        private double _column1Width;
        private double _column2Width;
        private const double _widthRatioColumn1 = 0.4;

        private double _buttonAndTitleHeight = 35;

        private const int SIDEPANELWIDTH = 300;
        private PanelType _panelType;

        public ActionPanelGrid(PanelType pt, Game game)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);
            _panelType = pt;
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
                for (int i = 0; i < _quantityOfRowsInUnit; i++)
                {
                    if (i == (int)ActionRows.TitleAndBackButton || i == (int)ActionRows.Button)
                    { RowDefinitions.Add(new RowDefinition()); }
                    else { RowDefinitions.Add(new RowDefinition()); }
                }
            }
        }

        public void InitDataRows()
        {
            if (_panelType == PanelType.SQ)
            {
                SqAttributes sqAttributes = new SqAttributes(_game);

                for (int i = (int)ActionRows.Owner; i <= (int)ActionRows.ActionCost; i++)
                {
                    Label rowHeading = new Label()
                    {
                        Text = SplitCamelCase(Convert.ToString(((ActionRows)i))),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                    };
                    this.Children.Add(rowHeading, 0, i);

                    Label rowValue = new Label()
                    {
                        Text = sqAttributes.GetValue((SqAttributes.AllSQAttributes)(i - (int)ActionRows.Owner)),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                    };
                    this.Children.Add(rowValue, 1, i);

                    if (i == (int)ActionRows.ActionCost)
                    {
                        rowHeading.FontAttributes = FontAttributes.Bold;
                        rowValue.FontAttributes = FontAttributes.Bold;
                    }
                }
            }
            else if (_panelType == PanelType.Unit)
            {
                UnitAttributes unitAttributes = new UnitAttributes(_game.ActiveUnit);

                for (int i = (int)ActionRows.Owner; i <= (int)ActionRows.ActionCost; i++)
                {
                    Label rowHeading = new Label() { Text = SplitCamelCase(Convert.ToString((ActionRows)i)) };
                    this.Children.Add(rowHeading, 0, i);

                    Label rowValue = new Label()
                    {
                        Text = unitAttributes.GetValue((UnitAttributes.AllUnitAttributes)(i - (int)ActionRows.Owner)),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                    };
                    this.Children.Add(rowValue, 1, i);

                    if (i == (int)ActionRows.ActionCost)
                    {
                        rowHeading.FontAttributes = FontAttributes.Bold;
                        rowValue.FontAttributes = FontAttributes.Bold;
                    }
                }
            }
        }

        public void InitBackButton()
        {
            Button backButton = new Button()
            {
                Text = "X",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Transparent,
                TextColor = Color.Black,
            };

            Children.Add(backButton, 1, (int)ActionRows.TitleAndBackButton);
            backButton.Clicked += OnBackButton;
        }

        public void OnBackButton(object sender, EventArgs e) => _game.GameBoardVM.SidePanelManager.RemoveSidePanel(_panelType);

        public void InitTitle()
        {
            string text = _panelType == PanelType.SQ ? "SQ Action" : "Unit Action";
            Label titleLabel = new Label()
            {
                Text = text,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = _game.PaletteColors.Where(c => c.Description == "Banff 2").Select(c => c.Color).FirstOrDefault(),
                TextColor = Color.White,
                HeightRequest = _buttonAndTitleHeight,
                Margin = (_buttonAndTitleHeight * 0.1),
                FontSize = (_buttonAndTitleHeight * 0.5),
            };

            if (_panelType == PanelType.SQ) { Children.Add(titleLabel, 0, (int)ActionRows.TitleAndBackButton); }
            else if (_panelType == PanelType.Unit) { Children.Add(titleLabel, 0, (int)ActionRows.TitleAndBackButton); }
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
