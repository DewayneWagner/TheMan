using System;
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
            Cluster = new Command(LoadClusterPage);
            Settings = new Command(LoadSettingsPage);
            DataBaseDelete = new Command(UpdateDataBase);
        }

        public ICommand Settings { get; set; }
        public ICommand Cluster { get; set; }
        public ICommand StartNewGame { get; set; }
        public ICommand LoadGame { get; set; }
        public ICommand DataBaseDelete { get; set; }
        private PageService _pageService { get; set; }

        private async void LoadGameMethod(object obj)
        {
            //await _pageService.PushAsync(new NavigationPage(new LoadGameView()));
            await _pageService.PushAsync(new LoadGameView());
        }
        private async void StartNewGameMethod(object obj)
        {
            //await _pageService.PushAsync(new NavigationPage(new StartNewGameView()));
            await _pageService.PushAsync(new StartNewGameView());
        }
        private async void LoadSettingsPage(object obj)
        {
            //await _pageService.PushAsync(new NavigationPage(new ParameterView()));
            await _pageService.PushAsync(new ParameterView());
        }
        private async void LoadClusterPage(object obj)
        {
            //await _pageService.PushAsync(new NavigationPage(new ClusterView()));
            await _pageService.PushAsync(new ClusterView());
        }
        private void UpdateDataBase(object obj)
        {
            using (DBContext db = new DBContext())
            {
                db.DeleteDatabase();
                db.SaveChanges();
            }
        }
    }
}
