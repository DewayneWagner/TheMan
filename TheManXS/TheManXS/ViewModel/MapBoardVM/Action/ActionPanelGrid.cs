using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    public class ActionPanelGrid : Grid
    {
        private SQ sq;
        private GameBoardVM _gameBoardVM;
        private ActionPanel _actionPanel;

        enum Rows { BackButton, Logo, Title, Owner, Status, Resource, Production, Revenue, OPEX, TransportCost, 
            GrossProfitD, GrossProfitP, ActionCost, Button, Total }

        private double _column1Width;
        private double _column2Width;
        private const double _widthRatioColumn1 = 0.4;

        private double _standardRowHeight;
        private double _buttonRowHeight;
        private const int _numberOfButtons = 2;

        public ActionPanelGrid(ActionPanel a)
        {
            _actionPanel = a;
            sq = (SQ)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActiveSQ)];
            _gameBoardVM = (GameBoardVM)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.GameBoardVM)];
            
            _column1Width = QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize * _widthRatioColumn1;
            _column2Width = (QC.ScreenWidth * QC.WidthOfActionPaneRatioOfScreenSize) - _column1Width;

            RowSpacing = 0;
            Padding = 0;
            VerticalOptions = LayoutOptions.Center;

            SetRowHeightFields();

            InitGrid();
            AddLogoToTopOfGrid();
            InitDataRows();
            InitBackButton();
            InitTitle();    
            InitActionButton();
        }
        private void SetRowHeightFields()
        {
            _standardRowHeight = (QC.ScreenHeight * 0.75) / ((int)Rows.Total + _numberOfButtons);
            _buttonRowHeight = _standardRowHeight * 1.25;
        }
        private void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column1Width });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = _column2Width });

            for (int i = 0; i < (int)Rows.Total; i++)
            {
                if (i == (int)Rows.BackButton || i == (int)Rows.Button) { RowDefinitions.Add(new RowDefinition() { Height = _buttonRowHeight }); }
                else if(i == (int)Rows.Logo) { RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star }); }
                else { RowDefinitions.Add(new RowDefinition() { Height = _standardRowHeight }); }             
            }
        }
        private void AddLogoToTopOfGrid()
        {
            Image logo = AllImages.GetImage(AllImages.ImagesAvailable.Logo);
            logo.Aspect = Aspect.AspectFill;
            Children.Add(logo, 0, (int)Rows.Logo);
            Grid.SetColumnSpan(logo, 2);
            logo.VerticalOptions = LayoutOptions.StartAndExpand;
        }
        private void InitDataRows()
        {
            SqAttributes sqAttributes;
            for (int i = (int)Rows.Owner; i <= (int)Rows.ActionCost; i++)
            {
                Label rowHeading = new Label() { Text = Convert.ToString((Rows)i) };
                this.Children.Add(rowHeading, 0, i);

                sqAttributes = new SqAttributes(sq, (SqAttributes.AllSQAttributes)(i-(int)Rows.Owner));
                Label rowValue = new Label()
                {
                    Text = sqAttributes.Value,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                this.Children.Add(rowValue, 1, i);

                if(i == (int)Rows.ActionCost)
                {
                    rowHeading.FontAttributes = FontAttributes.Bold;
                    rowValue.FontAttributes = FontAttributes.Bold;
                }
            }
        }
        private void InitBackButton()
        {
            Button backButton = new Button()
            {
                Text = "Close",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
            };
            Children.Add(backButton, 1, (int)Rows.BackButton);
            backButton.Clicked += OnBackButton;
        }
        private void OnBackButton(object sender, EventArgs e)
        {
            _gameBoardVM.ActualGameBoardVM.GameBoardSplitScreenGrid.ActionPanel.CloseActionPanel();
            Tile tile = _gameBoardVM.ActualGameBoardVM.GameBoardSplitScreenGrid.MapScrollView.PinchToZoomContainer.
                GameBoard.FocusedGameBoard.GetTile(sq.Row, sq.Col);
            tile.OverlayGrid.RemoveOutsideBorders();
        }
        private void InitTitle()
        {
            Label titleLabel = new Label()
            {
                Text = "SQ Action",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Crimson,
                TextColor = Color.White,
                Margin = (_standardRowHeight * 0.05),
                FontSize = (_standardRowHeight * 0.8),
            };
            Children.Add(titleLabel, 0, (int)Rows.Title);
            Grid.SetColumnSpan(titleLabel, 2);
        }        
        private void InitActionButton()
        {
            ActionButton actionButton = new ActionButton(sq,_actionPanel);
            Children.Add(actionButton, 0, (int)Rows.Button);

            Grid.SetColumnSpan(actionButton, 2);            
        }
    }
}
