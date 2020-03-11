using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class ResourceBreakdownGrid : Grid
    {
        enum PrimarySelector { ResourceType, AllResources, StockPrice, Revenue, CommodityPrice, Ratios, Total }
        enum SecondarySelector { AllCompanies, ActualPlayer, Player2, Player3, Player4, Player5, Total }
        enum Terms { AllTurns, AllCompanies, }
        Game _game;
        int _heightOfSectorRow = 50;
        public ResourceBreakdownGrid(Game game)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);
            InitGrid();
            SetPropertiesOfGrid();
            AddHeaderSelectionGrid();
        }
        void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
        }
        void SetPropertiesOfGrid()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            RowSpacing = 0;
            ColumnSpacing = 0;
        }
        void AddHeaderSelectionGrid()
        {
            this.Children.Add(new HeaderSelectionGrid());
        }
    }
    public class HeaderSelectionGrid : Grid
    {
        int _titleRowHeight = 50;
        int _selectorRowHeight = 50;
        double _columnWidth;
        public HeaderSelectionGrid()
        {
            
            InitGrid();
            AddSelectionLabels();
        }
        private void InitGrid()
        {
            RowDefinitions.Add(new RowDefinition() { Height = _titleRowHeight });
            RowDefinitions.Add(new RowDefinition() { Height = _selectorRowHeight });

            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
        }
        private void AddSelectionLabels()
        {
            FinancialsDisplayColors f = new FinancialsDisplayColors();
            Children.Add(new HeadingLabel("Color1",f.Color1, _titleRowHeight), 0, 0);
            Children.Add(new HeadingLabel("Color2",f.Color2, _titleRowHeight), 1, 0);
            Children.Add(new HeadingLabel("Color3",f.Color3, _titleRowHeight), 2, 0);
        }
        class HeadingLabel : Label
        {
            public HeadingLabel(string text, Color c, double rowHeight)
            {
                HorizontalOptions = LayoutOptions.FillAndExpand;
                VerticalOptions = LayoutOptions.FillAndExpand;
                Text = text;
                BackgroundColor = c;
                HorizontalTextAlignment = TextAlignment.Center;
                VerticalTextAlignment = TextAlignment.Center;
                TextColor = Color.White;
                Margin = 3;
                FontSize = rowHeight * 0.8;
            }
        }
    }
}
