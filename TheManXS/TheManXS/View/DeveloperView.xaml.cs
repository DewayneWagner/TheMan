using TheManXS.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeveloperView : ContentPage
    {
        public DeveloperView()
        {
            DeveloperVM dvm = new DeveloperVM();
            InitializeComponent();
            BindingContext = dvm;

        }
    }
}