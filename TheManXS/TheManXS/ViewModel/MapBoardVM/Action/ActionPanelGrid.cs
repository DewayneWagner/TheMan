using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.Model.Units;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanelGrid : Grid
    {
        private SQ sq;
        private GameBoardVM _gameBoardVM;
        private ActionPanel _actionPanel;
        private Unit _activeUnit;

        public enum SQActionRows { BackButton, Logo, Title, Owner, Status, Resource, Production, Revenue, OPEX, TransportCost, 
            GrossProfitD, GrossProfitP, ActionCost, Button, Total }

        public enum UnitActionRows { BackButton, Logo, Title, Owner, Status, Resource, Production, Revenue, OPEX, OPEXDisCount, 
            TransportCost, GrossProfitD, GrossProfitP, ActionCost, ActionCostDiscount, Button, Total }

        private double _column1Width;
        private double _column2Width;
        private const double _widthRatioColumn1 = 0.4;

        private double _standardRowHeight;
        private double _buttonRowHeight;
        private const int _numberOfButtons = 2;
        private ActionPanel.PanelType _panelType;

        public ActionPanelGrid(ActionPanel a, ActionPanel.PanelType pt)
        {
            _actionPanel = a;
            _panelType = pt;
            
            _column1Width = QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize * _widthRatioColumn1;
            _column2Width = (QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize) - _column1Width;

            RowSpacing = 0;
            Padding = 0;
            VerticalOptions = LayoutOptions.Center;
            BackgroundColor = Color.White;
            VerticalOptions = LayoutOptions.FillAndExpand;

            InitFields();
            InitAllElements();
        }
        private void InitFields()
        {
            if (_panelType == ActionPanel.PanelType.SQ) { sq = (SQ)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveSQ)]; }
            else if (_panelType == ActionPanel.PanelType.Unit) { _activeUnit = (Unit)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveUnit)]; }
            _gameBoardVM = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
        }
        private void InitAllElements()
        {
            SetRowHeightFields();
            InitGrid();
            AddLogoToTopOfGrid();
            InitDataRows();
            InitBackButton();
            InitTitle();
            InitActionButton();
        }        
        public void SetRowHeightFields()
        {
            _standardRowHeight = _panelType == ActionPanel.PanelType.SQ ?
                (QC.ScreenHeight * 0.75) / ((int)SQActionRows.Total + _numberOfButtons) :
                (QC.ScreenHeight * 0.75) / ((int)UnitActionRows.Total + _numberOfButtons);

            _buttonRowHeight = _standardRowHeight * 1.25;
        }
        public void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column1Width });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column2Width });

            if (_panelType == ActionPanel.PanelType.SQ)
            {
                for (int i = 0; i < (int)SQActionRows.Total; i++)
                {
                    if (i == (int)SQActionRows.BackButton || i == (int)SQActionRows.Button) 
                        { RowDefinitions.Add(new RowDefinition() { Height = _buttonRowHeight }); }
                    else if (i == (int)SQActionRows.Logo) { RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star }); }
                    else { RowDefinitions.Add(new RowDefinition() { Height = _standardRowHeight }); }
                }
            }
            else if(_panelType == ActionPanel.PanelType.Unit)
            {
                for (int i = 0; i < (int)UnitActionRows.Total; i++)
                {
                    if (i == (int)UnitActionRows.BackButton || i == (int)UnitActionRows.Button) 
                        { RowDefinitions.Add(new RowDefinition() { Height = _buttonRowHeight }); }
                    else if (i == (int)UnitActionRows.Logo) { RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star }); }
                    else { RowDefinitions.Add(new RowDefinition() { Height = _standardRowHeight }); }
                }
            }
        }
        public void AddLogoToTopOfGrid()
        {
            Image logo = AllImages.GetImage(AllImages.ImagesAvailable.Logo);
            logo.Aspect = Aspect.AspectFill;
            Children.Add(logo, 0, (int)SQActionRows.Logo);
            Grid.SetColumnSpan(logo, 2);
            logo.VerticalOptions = LayoutOptions.StartAndExpand;
        }
        public void InitDataRows()
        {
            if (_panelType == ActionPanel.PanelType.SQ)
            {
                SqAttributes sqAttributes;

                for (int i = (int)SQActionRows.Owner; i <= (int)SQActionRows.ActionCost; i++)
                {
                    Label rowHeading = new Label() { Text = Convert.ToString((SQActionRows)i) };
                    this.Children.Add(rowHeading, 0, i);

                    sqAttributes = new SqAttributes(sq, (SqAttributes.AllSQAttributes)(i - (int)SQActionRows.Owner));
                    Label rowValue = new Label()
                    {
                        Text = sqAttributes.Value,
                        HorizontalTextAlignment = TextAlignment.Center,
                    };
                    this.Children.Add(rowValue, 1, i);

                    if (i == (int)SQActionRows.ActionCost)
                    {
                        rowHeading.FontAttributes = FontAttributes.Bold;
                        rowValue.FontAttributes = FontAttributes.Bold;
                    }
                }
            }
            else if(_panelType == ActionPanel.PanelType.Unit)
            {
                UnitAttributes unitAttributes = new UnitAttributes(_activeUnit);

                for (int i = (int)UnitActionRows.Owner; i <= (int)UnitActionRows.ActionCost; i++)
                {
                    Label rowHeading = new Label() { Text = Convert.ToString((UnitActionRows)i) };
                    this.Children.Add(rowHeading, 0, i);

                    Label rowValue = new Label()
                    {
                        Text = unitAttributes.GetValue((UnitAttributes.AllUnitAttributes)(i - (int)UnitActionRows.Owner)),
                        HorizontalTextAlignment = TextAlignment.Center,
                    };
                    this.Children.Add(rowValue, 1, i);

                    if (i == (int)UnitActionRows.ActionCost)
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
                Text = "Close",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
            };
            if(_panelType == ActionPanel.PanelType.SQ) { Children.Add(backButton, 1, (int)SQActionRows.BackButton); }
            else if(_panelType == ActionPanel.PanelType.Unit) { Children.Add(backButton, 1, (int)UnitActionRows.BackButton); }
            backButton.Clicked += OnBackButton;
        }
        public void OnBackButton(object sender, EventArgs e)
        {
            _gameBoardVM.ActualGameBoardVM.GameBoardSplitScreenGrid.ActionPanel.CloseActionPanel();

            if(_panelType == ActionPanel.PanelType.SQ) { sq.Tile.OverlayGrid.RemoveOutsideBorders(); }
            else if(_panelType == ActionPanel.PanelType.Unit) { _activeUnit.KillUnit(); }
        }
        public void InitTitle()
        {
            string text = _panelType == ActionPanel.PanelType.SQ ? "SQ Action" : "Unit Action";
            Label titleLabel = new Label()
            {
                Text = text,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Crimson,
                TextColor = Color.White,
                Margin = (_standardRowHeight * 0.05),
                FontSize = (_standardRowHeight * 0.8),
            };

            if (_panelType == ActionPanel.PanelType.SQ) { Children.Add(titleLabel, 0, (int)SQActionRows.Title); }
            else if(_panelType == ActionPanel.PanelType.Unit) { Children.Add(titleLabel, 0, (int)UnitActionRows.Title); }
            Grid.SetColumnSpan(titleLabel, 2);
        }        
        public void InitActionButton()
        {
            ActionButton actionButton = new ActionButton(sq, _actionPanel);
            if(_panelType == ActionPanel.PanelType.SQ) { Children.Add(actionButton, 0, (int)SQActionRows.Button); }
            else if(_panelType == ActionPanel.PanelType.Unit) { Children.Add(actionButton, 0, (int)UnitActionRows.Button); }
            
            Grid.SetColumnSpan(actionButton, 2);            
        }
    }
}
