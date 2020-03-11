﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Services.EntityFrameWork;
using TheManXS.View;
using TheManXS.View.DetailView;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel.DetailPages
{
    public class MainMenuVM : BaseViewModel
    {
        public MainMenuVM()
        {
            _pageService = new PageService();
            StartNewGame = new Command(StartNewGameMethod);
            LoadGame = new Command(LoadGameMethod);
            DeveloperView = new Command(OnDevelopView);
            CompressedLayout.SetIsHeadless(this, true);
        }
        
        public ICommand StartNewGame { get; set; }
        public ICommand LoadGame { get; set; }
        
        public ICommand DeveloperView { get; set; }
        private PageService _pageService { get; set; }

        private async void LoadGameMethod(object obj)
        {
            await _pageService.PushAsync(new LoadGameView());
        }
        private async void StartNewGameMethod(object obj)
        {
            await _pageService.PushAsync(new StartNewGameView());
        }
        private async void OnDevelopView(object obj)
        {
            await _pageService.PushAsync(new DeveloperView());
        }
    }
}
