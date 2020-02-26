﻿using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.Action;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public class GameBoardVM : BaseViewModel
    {
        Game _game;
        public GameBoardVM() 
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM = this;
            TouchEffectsEnabled = true;
        }
        public GameBoardVM(bool isForAppDictionary) { }

        private StockTickerBarVM _stockTickerBarVM;
        public StockTickerBarVM StockTicker
        {
            get => _stockTickerBarVM;
            set
            {
                _stockTickerBarVM = value;
                SetValue(ref _stockTickerBarVM, value);
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


    }
}
