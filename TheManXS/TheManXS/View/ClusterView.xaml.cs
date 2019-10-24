using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TheManXS.ViewModel.DetailPages;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClusterView : ContentPage
    {
        private ClusterVM CVM;
        public ClusterView()
        {
            InitializeComponent();
            CVM = new ClusterVM();
            Content.BindingContext = CVM;
            LV_Cluster.ItemsSource = CVM.ListOfAllClusters;
        }

        private void LV_Cluster_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void OnEdit_Clicked(object sender, EventArgs e)
        {
            var c = (ClusterVM)sender;
            

        }
        private void OnDelete_Clicked(object sender, EventArgs e)
        {

        }
    }
}