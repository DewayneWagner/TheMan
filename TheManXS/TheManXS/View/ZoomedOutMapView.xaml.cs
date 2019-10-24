using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZoomedOutMapView : ContentPage
    {
        public ZoomedOutMapView(TheManXS.ViewModel.ZoomedOutMapVM.ViewType vt)
        {
            ZoomedOutMapVM z = new ZoomedOutMapVM(vt);
            InitializeComponent();
            Content = z;
        }
    }
}