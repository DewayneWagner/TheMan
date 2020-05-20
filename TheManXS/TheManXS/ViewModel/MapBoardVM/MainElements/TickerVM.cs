using System;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Main;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static TheManXS.ViewModel.MapBoardVM.Tiles.AllImages;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class TickerVM : BaseViewModel
    {
        Game _game;
        private const double HEIGHTRATIO = 0.04;
        public TickerVM(bool isForAddingToGameBoardVM) { }
        public TickerVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM.TickerVM = this;
            Ticker = GetTicker();
            Content = Ticker;
            CompressedLayout.SetIsHeadless(this, true);
        }

        public void UpdateTicker()
        {
            Ticker = GetTicker();
            Content = Ticker;
        }

        private StackLayout _ticker;
        public StackLayout Ticker
        {
            get => _ticker;
            set
            {
                _ticker = value;
                SetValue(ref _ticker, value);
            }
        }

        private StackLayout GetTicker()
        {
            StackLayout ticker = new StackLayout();
            initPropertiesOfTicker();
            initStockPrices();
            initCommPrices();

            void initPropertiesOfTicker()
            {
                //ticker.HeightRequest = (QC.ScreenHeight * HEIGHTRATIO);
                ticker.VerticalOptions = LayoutOptions.FillAndExpand;
                ticker.Orientation = StackOrientation.Horizontal;
                ticker.HorizontalOptions = LayoutOptions.Start;
            }

            void initStockPrices()
            {
                foreach (Player player in _game.PlayerList)
                {
                    ticker.Children.Add(new PricingDisplayGrid(player.Ticker, player.StockPrice, player.Delta));
                }
            }

            void initCommPrices()
            {
                foreach (Commodity commodity in _game.CommodityList)
                {
                    string commodityName = Convert.ToString((RT)commodity.ResourceTypeNumber);
                    ticker.Children.Add(new PricingDisplayGrid(commodityName, commodity.Price, commodity.Delta));
                }
            }
            return ticker;
        }
    }
    public class PricingDisplayGrid : Grid
    {
        string _name, _price, _delta;
        ImagesAvailable _imageType;

        int _qColumns = 4;

        public PricingDisplayGrid(string companyName, double price, double delta)
        {
            _name = companyName;
            _price = price.ToString("c2");
            _imageType = GetArrow(delta);
            _delta = delta.ToString("p1");
            InitGrid();
        }

        ImagesAvailable GetArrow(double delta)
        {
            if (delta < 0) { return ImagesAvailable.DownArrow; }
            else if (delta == 0) { return ImagesAvailable.NoChangeArrow; }
            else { return ImagesAvailable.UpArrow; }
        }

        void InitGrid()
        {
            initElements();
            initPropertiesOfGrid();
            addLabels();
            addArrowImage();

            void initElements()
            {
                RowDefinitions.Add(new RowDefinition());
                for (int i = 0; i < _qColumns; i++)
                {
                    ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                }
            }
            void initPropertiesOfGrid()
            {
                RowSpacing = 0;
                ColumnSpacing = 5;
                Margin = 5;
            }
            void addLabels()
            {
                this.Children.Add(new TickerLabel(_name), 0, 0);
                this.Children.Add(new TickerLabel(_price), 1, 0);
                this.Children.Add(new TickerLabel(_delta), 2, 0);
            }
            void addArrowImage()
            {
                Image arrow = AllImages.GetImage(_imageType);
                arrow.Aspect = Aspect.AspectFit;
                arrow.HorizontalOptions = LayoutOptions.StartAndExpand;
                arrow.VerticalOptions = LayoutOptions.StartAndExpand;
                arrow.WidthRequest = 25;
                this.Children.Add(arrow, 3, 0);
            }
        }
        class TickerLabel : Label
        {
            public TickerLabel(string text)
            {
                BackgroundColor = Color.Transparent;
                TextColor = Color.White;
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.Center;
                LineBreakMode = LineBreakMode.NoWrap;
                Text = text;
            }
        }
    }
}
