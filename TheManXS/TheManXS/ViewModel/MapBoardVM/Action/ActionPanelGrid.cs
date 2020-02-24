using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.Model.Units;
using TheManXS.ViewModel.MapBoardVM.MainElements;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanelGrid : Grid
    {
        public enum PanelType { SQ, Unit }
        MapVM _mapVM;
        GameBoardVM _gameBoardVM;

        public enum ActionRows { LogoAndBackButton, Title, Owner, Status, Resource, Production, Revenue, OPEX, TransportCost, 
            GrossProfitD, GrossProfitP, ActionCost, Button, UnitOPEXDiscount, UnitActionCostDiscount }

        private int _quantityOfRowsInSQ = (int)ActionRows.Button + 1;
        private int _quantityOfRowsInUnit = (int)ActionRows.UnitActionCostDiscount + 1;

        private double _column1Width;
        private double _column2Width;
        private const double _widthRatioColumn1 = 0.4;

        private double _buttonAndTitleHeight = 35;
        private const int _numberOfButtons = 2;
        
        private const int SIDEPANELWIDTH = 300;

        public ActionPanelGrid(PanelType pt, GameBoardVM gameBoardVM)
        {
            CompressedLayout.SetIsHeadless(this, true);
            PanelTypeProp = pt;
            _gameBoardVM = gameBoardVM;
            _mapVM = _gameBoardVM.MapVM;
            WidthRequest = SIDEPANELWIDTH;
            SetPropertiesOfGrid();
            InitFields();
            InitAllElements();
        }

        public PanelType PanelTypeProp { get; set; }

        private void SetPropertiesOfGrid()
        {
            RowSpacing = 0;
            Padding = 0;
            VerticalOptions = LayoutOptions.Center;
            BackgroundColor = Color.White;
            VerticalOptions = LayoutOptions.FillAndExpand;
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
            AddLogoToTopOfGrid();
            InitDataRows();
            InitBackButton();
            InitTitle();
            InitActionButton();
        }        
        public void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column1Width });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column2Width });

            if (PanelTypeProp == PanelType.SQ) {for (int i = 0; i < _quantityOfRowsInSQ; i++) { RowDefinitions.Add(new RowDefinition()); }}
            
            else if(PanelTypeProp == PanelType.Unit)
            {
                for (int i = 0; i < _quantityOfRowsInUnit; i++)
                {
                    if (i == (int)ActionRows.LogoAndBackButton || i == (int)ActionRows.Button) 
                        { RowDefinitions.Add(new RowDefinition()); }
                    else { RowDefinitions.Add(new RowDefinition()); }
                }
            }
        }
        public void AddLogoToTopOfGrid()
        {
            Image logo = AllImages.GetImage(AllImages.ImagesAvailable.Logo);
            logo.Aspect = Aspect.AspectFit;
            Children.Add(logo, 0, (int)ActionRows.LogoAndBackButton);
            Grid.SetColumnSpan(logo, 2);
            logo.VerticalOptions = LayoutOptions.StartAndExpand;
            logo.HorizontalOptions = LayoutOptions.StartAndExpand;
        }
        public void InitDataRows()
        {
            if (PanelTypeProp == PanelType.SQ)
            {
                SqAttributes sqAttributes = new SqAttributes(_mapVM);

                for (int i = (int)ActionRows.Owner; i <= (int)ActionRows.ActionCost; i++)
                {
                    Label rowHeading = new Label() 
                    { 
                        Text = Convert.ToString((ActionRows)i),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                    };
                    this.Children.Add(rowHeading, 0, i);

                    Label rowValue = new Label()
                    {
                        Text = sqAttributes.GetValue((SqAttributes.AllSQAttributes)(i - (int)ActionRows.Owner)),
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
            else if(PanelTypeProp == PanelType.Unit)
            {
                UnitAttributes unitAttributes = new UnitAttributes(_gameBoardVM.MapVM.ActiveUnit);

                for (int i = (int)ActionRows.Owner; i <= (int)ActionRows.ActionCost; i++)
                {
                    Label rowHeading = new Label() { Text = Convert.ToString((ActionRows)i) };
                    this.Children.Add(rowHeading, 0, i);

                    Label rowValue = new Label()
                    {
                        Text = unitAttributes.GetValue((UnitAttributes.AllUnitAttributes)(i - (int)ActionRows.Owner)),
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

            Children.Add(backButton, 1, (int)ActionRows.LogoAndBackButton);
            backButton.Clicked += OnBackButton;
        }
        public void OnBackButton(object sender, EventArgs e)
        {
            _gameBoardVM.SplitScreenGrid.Children.RemoveAt(1);
            _gameBoardVM.SplitScreenGrid.ColumnDefinitions.RemoveAt(1);
            _gameBoardVM.MapVM.SideSQActionPanelExists = false;
            _gameBoardVM.MapVM.TouchEffectsEnabled = true;

            //_mapVM.MapBoard.CloseActionPanel();
            //_mapVM.ActionPanel.CloseActionPanel();

            //if(_panelType == PanelType.SQ && _mapVM.ActiveSQ.OwnerNumber == QC.PlayerIndexTheMan) {; }
            //    //{ _activeSQ.Tile.OverlayGrid.RemoveOutsideBorders(_activeSQ.Tile); }

            //else if(_panelType == PanelType.Unit) { _activeUnit.KillUnit(); }
        }
        public void InitTitle()
        {
            string text = PanelTypeProp == PanelType.SQ ? "SQ Action" : "Unit Action";
            Label titleLabel = new Label()
            {
                Text = text,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Crimson,
                TextColor = Color.White,
                HeightRequest = _buttonAndTitleHeight,
                Margin = (_buttonAndTitleHeight * 0.1),
                FontSize = (_buttonAndTitleHeight * 0.75),
            };

            if (PanelTypeProp == PanelType.SQ) { Children.Add(titleLabel, 0, (int)ActionRows.Title); }
            else if(PanelTypeProp == PanelType.Unit) { Children.Add(titleLabel, 0, (int)ActionRows.Title); }
            Grid.SetColumnSpan(titleLabel, 2);
        }        
        public void InitActionButton()
        {
            ActionButton actionButton = new ActionButton(_gameBoardVM, PanelTypeProp)
            {
                HeightRequest = _buttonAndTitleHeight,
                Margin = (_buttonAndTitleHeight * 0.1),
                FontSize = (_buttonAndTitleHeight * 0.5),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Crimson,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                WidthRequest = QC.WidthOfActionPanel * 0.8,
            };
            Children.Add(actionButton, 0, (int)ActionRows.Button); 
            Grid.SetColumnSpan(actionButton, 2);            
        }
    }
}
