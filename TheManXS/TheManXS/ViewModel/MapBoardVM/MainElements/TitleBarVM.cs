﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Main;
using TheManXS.View;
using TheManXS.ViewModel.MapBoardVM.MapConstruct;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.MainElements
{
    public enum ViewType { Terrain, Resources, Roads, Pipeline, TrainTracks, Total }
    public class TitleBarVM : BaseViewModel
    {
        private string _quarter;
        private string _companyName;
        private PageService _pageService;
        private Game _game;
        
        public TitleBarVM(bool isForInitGameBoardVM) { }
        public TitleBarVM()
        {
            _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
            _game.GameBoardVM.TitleBar = this;

            _pageService = new PageService();
            CompressedLayout.SetIsHeadless(this, true);

            ShowFinancials = new Command(ShowFinancialsPage);
            EndTurn = new Command(EndTurnAction);

            ShowTerrain = new Command(ShowTerrainMap);
            ShowResources = new Command(ShowResourceMap);

#if DEBUG
            Quarter = "1900-Q1";
            CompanyName = "Rockyspring Ltd.";
#endif
        }

        public string Quarter
        {
            get => _quarter;
            set
            {
                _quarter = value;
                SetValue(ref _quarter, value);
            }
        }

        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                SetValue(ref _companyName, value);
            }
        }
        
        public ICommand ShowFinancials { get; set; }
        public ICommand EndTurn { get; set; }
        public ICommand ShowTerrain { get; set; }
        public ICommand ShowResources { get; set; }

        private async void ShowResourceMap() => await _pageService.PushAsync(new ZoomedOutMapView(ZoomedOutMapVM.ViewType.Resources));

        //private async void ShowTerrainMap() => await _pageService.PushAsync(new ZoomedOutMapView(ZoomedOutMapVM.ViewType.Terrain));

        private async void ShowTerrainMap() => await _pageService.PushAsync(new MapBoard());
        private async void EndTurnAction(object obj)
        {
            await _pageService.DisplayAlert("Turn has been ended.");
        }

        private async void ShowFinancialsPage(object obj)
        {
            //await _pageService.DisplayAlert("Financials Page");
            await _pageService.PushAsync(new FinancialsView());
        }
    }
}
