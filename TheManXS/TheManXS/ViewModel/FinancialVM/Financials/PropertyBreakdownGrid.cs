using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    class PropertyBreakdownGrid : Grid
    {
        Game _game;
        public PropertyBreakdownGrid(Game game)
        {
            _game = game;
            InitGrid();
            SetPropertiesOfGrid();
            AddTestLabel();
        }
        void InitGrid()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinitions.Add(new RowDefinition());
        }
        void SetPropertiesOfGrid()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            RowSpacing = 0;
            ColumnSpacing = 0;
        }
        void AddTestLabel()
        {
            Label label = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.LightGreen,
                Text = "COMING SOON!",
                FontSize = 50,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            Children.Add(label, 0, 0);
        }
    }
}
