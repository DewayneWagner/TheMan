using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheManXS.Model.Settings;
using TheManXS.ViewModel.DetailPages;
using SQLite;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParameterView : ContentPage
    {
        public ParameterView()
        {
            InitializeComponent();

            SettingsVM svm = new SettingsVM();
            Content.BindingContext = svm;
            SettingsListView.ItemsSource = svm.SettingsVMOC;
        }        
    }
}