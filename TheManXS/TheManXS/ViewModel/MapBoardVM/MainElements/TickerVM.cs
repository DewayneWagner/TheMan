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
        private const double HEIGHTRATIO = 0.06;
        public TickerVM(bool isForAddingToGameBoardVM) { }
        public TickerVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM.TickerVM = this;
            Ticker = GetTicker();
            Content = Ticker;
        }

        public void UpdateTicker() { Content = GetTicker(); }

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

        public StackLayout GetTicker()
        {
            StackLayout ticker = new StackLayout();
            initPropertiesOfTicker();
            initStockPrices();
            initCommPrices();

            void initPropertiesOfTicker()
            {
                ticker.HeightRequest = (QC.ScreenHeight * HEIGHTRATIO);
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
        Image _arrow;
        int _qColumns = 4;

        public PricingDisplayGrid(string companyName, double price, double delta)
        {
            _name = companyName;
            _price = price.ToString("c2");
            _delta = delta.ToString("p1");
            InitGrid();
        }

        void InitGrid()
        {
            initElements();
            initPropertiesOfGrid();
            addLabels();

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
                this.Children.Add(new TickerLabel(_name),0,0);
                this.Children.Add(new TickerLabel(_price), 1, 0);
                this.Children.Add(new TickerLabel(_delta), 2, 0);
            }
            void getArrowImage()
            {
                Image arrow = ImageSource.FromResource()
            }
        }
        class TickerLabel : Label
        {
            public TickerLabel(string text)
            {
                BackgroundColor = Color.Transparent;
                TextColor = Color.White;
                //HorizontalOptions = LayoutOptions.FillAndExpand;
                VerticalTextAlignment = TextAlignment.Center;
                HorizontalTextAlignment = TextAlignment.Center;
                LineBreakMode = LineBreakMode.NoWrap;
                Text = text;
            }
        }
    }


    //public class PricingDisplaySL : StackLayout
    //{
    //    string _name;
    //    string _price;
    //    string _delta;
    //    Image _arrow;

    //    public PricingDisplaySL(int resourceType, double price, double delta)
    //    {
    //        _name = Convert.ToString((RT)resourceType);
    //        _price = price.ToString("c2");
    //        _delta = delta.ToString("p1");
    //        InitPropertiesOfStackLayout();
    //        AddLabels();
    //    }

    //    public PricingDisplaySL(string name, double price, double delta)
    //    {
    //        _name = name;
    //        _price = price.ToString("c2");
    //        _delta = delta.ToString("p1");
    //        InitPropertiesOfStackLayout();
    //        AddLabels();
    //    }

    //    void InitPropertiesOfStackLayout()
    //    {
    //        BackgroundColor = Color.Transparent;
    //        VerticalOptions = LayoutOptions.FillAndExpand;
    //        Orientation = StackOrientation.Horizontal;
    //        HorizontalOptions = LayoutOptions.Start;
    //    }

    //    void AddLabels()
    //    {
    //        this.Children.Add(new TickerLabel(_name));
    //        this.Children.Add(new TickerLabel(_price));
    //        this.Children.Add(new TickerLabel(_delta));
    //    }

    //    class TickerLabel : Label
    //    {
    //        public TickerLabel(string text)
    //        {
    //            BackgroundColor = Color.Transparent;
    //            TextColor = Color.White;
    //            //HorizontalOptions = LayoutOptions.FillAndExpand;
    //            VerticalTextAlignment = TextAlignment.Center;
    //            HorizontalTextAlignment = TextAlignment.Center;
    //            LineBreakMode = LineBreakMode.NoWrap;
    //            Text = text;
    //        }
    //    }
    //}
}
