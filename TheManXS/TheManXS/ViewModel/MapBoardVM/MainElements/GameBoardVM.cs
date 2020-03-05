using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial.CommodityStuff;
using TheManXS.Model.Main;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;
using static TheManXS.Model.Settings.SettingsMaster;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardVM : BaseViewModel
    {
        Game _game;
        public GameBoardVM() 
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM = this;
            CompressedLayout.SetIsHeadless(this, true);
            TouchEffectsEnabled = true;
            SidePanelManager = new SidePanelManager(_game);
            UpdateTickerText();
        }

        public GameBoardVM(bool isForAppDictionary) { }

        private string _tickerText;
        public string TickerText
        {
            get => _tickerText;
            set
            {
                _tickerText = value;
                SetValue(ref _tickerText, value);
            }
        }

        private TitleBarVM _titleBar;
        public TitleBarVM TitleBar
        {
            get => _titleBar;
            set
            {
                _titleBar = value;
                SetValue(ref _titleBar, value);
            }
        }

        private Grid _splitScreenGrid;
        public Grid SplitScreenGrid
        {
            get => _splitScreenGrid;
            set
            {
                _splitScreenGrid = value;
                SetValue(ref _splitScreenGrid, value);
            }
        }

        private MapVM _mapVM;
        public MapVM MapVM
        {
            get => _mapVM;
            set
            {
                _mapVM = value;
                SetValue(ref _mapVM, value);
            }
        }

        private ActionPanelGrid _actionPanelGrid;
        public ActionPanelGrid ActionPanelGrid
        {
            get => _actionPanelGrid;
            set
            {
                _actionPanelGrid = value;
                SetValue(ref _actionPanelGrid, value);
            }
        }

        public bool SideSQActionPanelExists { get; set; }
        public bool UnitActionPanelExists { get; set; }
        public bool IsThereActiveUnit { get; set; }
        public bool TouchEffectsEnabled { get; set; }
        public SidePanelManager SidePanelManager { get; set; }

        public void UpdateTickerText()
        {
            TickerText = null;
            updateStockPrices();
            updateCommodityPrices();

            void updateStockPrices()
            {
                foreach (Player player in _game.PlayerList)
                {
                    TickerText += player.Ticker + "   "
                        + player.StockPrice.ToString("c2") + "   "
                        + player.Delta.ToString("c2") + "  |  ";

                }
            }
            void updateCommodityPrices()
            {
                foreach (Commodity c in _game.CommodityList)
                {
                    TickerText += Convert.ToString((ResourceTypeE)c.ResourceTypeNumber) + "   "
                        + c.Price.ToString("c2") + "   "
                        + c.Delta.ToString("c2") + "  |  ";
                }
            }
        }
    }
}
