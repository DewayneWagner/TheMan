using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class TickerVM : BaseViewModel
    {
        Game _game;
        private const double HEIGHTRATIO = 0.08;
        public TickerVM(bool isForAddingToGameBoardVM) { }
        public TickerVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM.TickerVM = this;

            UpdateTicker();
            InitPropertiesOfTicker();
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
        void InitPropertiesOfTicker()
        {
            Ticker.HeightRequest = (QC.ScreenHeight * HEIGHTRATIO);
            Ticker.BackgroundColor = Color.Black;
            Ticker.VerticalOptions = LayoutOptions.FillAndExpand;
            Ticker.Orientation = StackOrientation.Horizontal;
            Ticker.HorizontalOptions = LayoutOptions.Start;
        }
        public void UpdateTicker()
        {
            initStockPrices();
            initCommPrices();

            void initStockPrices()
            {
                foreach (Player player in _game.PlayerList)
                {
                    Ticker.Children.Add(new PricingDisplaySL(player.Ticker, player.StockPrice, player.Delta));
                }
            }
            void initCommPrices()
            {
                foreach (Commodity commodity in _game.CommodityList)
                {
                    Ticker.Children.Add(new PricingDisplaySL(commodity.ResourceTypeNumber, commodity.Price, commodity.Delta);
                }
            }
        }
    }
    public class PricingDisplaySL : StackLayout
    {
        string _name;
        string _price;
        string _delta;
        Image _arrow;

        public PricingDisplaySL(int resourceType, double price, double delta)
        {
            _name = Convert.ToString((RT)resourceType);
            _price = price.ToString("c2");
            _delta = delta.ToString("p1");
            AddLabels();
        }

        public PricingDisplaySL(string name, double price, double delta)
        {
            _name = name;
            _price = price.ToString("c2");
            _delta = delta.ToString("p1");
            AddLabels();
        }

        void AddLabels()
        {
            this.Children.Add(new TickerLabel(_name));
            this.Children.Add(new TickerLabel(_price));
            this.Children.Add(new TickerLabel(_delta));
        }

        class TickerLabel : Label
        {
            public TickerLabel(string text)
            {
                BackgroundColor = Color.Transparent;
                TextColor = Color.White;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.Center;
                Text = text;
            }
        }
    }
}
