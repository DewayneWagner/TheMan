using TheManXS.ViewModel.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadGameView : ContentPage
    {
        public LoadGameView()
        {
            LoadGameVM lgvm = new LoadGameVM();
            InitializeComponent();
            BindingContext = lgvm;
        }
    }
}