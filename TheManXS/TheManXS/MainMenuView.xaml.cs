using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.View.DetailView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuView : ContentPage
    {
        public MainMenuView()
        {
            InitializeComponent();
            MainMenuVM mmvm = new MainMenuVM();
            Content.BindingContext = mmvm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //initialize Quick Constants here, so that screen size is available
            new QC();
            QC.ScreenHeight = (int)this.Height;
            QC.ScreenWidth = (int)this.Width;
            QC.Rotation = this.Rotation;
        }
    }
}