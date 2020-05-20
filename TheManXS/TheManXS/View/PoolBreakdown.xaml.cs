using TheManXS.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoolBreakdown : ContentPage
    {
        public PoolBreakdown()
        {
            InitializeComponent();
            Content = new ZoomedOutMapVM();
            //AbsoluteLayout.BindingContext = new ZoomedOutMapVM();
        }
    }
}