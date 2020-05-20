
using TheManXS.ViewModel.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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