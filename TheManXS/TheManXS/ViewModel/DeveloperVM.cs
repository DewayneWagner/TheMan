using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.View;
using TheManXS.ViewModel.Services;
using Xamarin.Forms;

namespace TheManXS.ViewModel
{
    public class DeveloperVM : BaseViewModel
    {
        private PageService _pageService;

        public DeveloperVM()
        {
            _pageService = new PageService();
            Cluster = new Command(LoadClusterPage);
            Settings = new Command(LoadSettingsPage);
            DataBaseDelete = new Command(UpdateDataBase);
        }
        public ICommand Settings { get; set; }
        public ICommand Cluster { get; set; }
        public ICommand DataBaseDelete { get; set; }

        private async void LoadClusterPage(object obj)
        {
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
        private async void LoadSettingsPage(object obj)
        {
            await _pageService.PushAsync(new ParameterView());
        }
    }
}
