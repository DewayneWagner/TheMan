using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    class FinancialRatiosGrid : Grid
    {
        Game _game;
        public FinancialRatiosGrid(Game game)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);
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
                Text = "FINANCIAL RATIOS" + "\n" + "COMING SOON!",
                FontSize = 50,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            Children.Add(label, 0, 0);
        }
    }
}
