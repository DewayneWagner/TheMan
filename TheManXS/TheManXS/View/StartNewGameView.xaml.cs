using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel;
using TheManXS.ViewModel.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartNewGameView : ContentPage
    {
        private StartNewGameVM s;
        public StartNewGameView()
        {
            s = new StartNewGameVM();
            InitializeComponent();
            BindingContext = s;
        }
        private void LV_SavedGameSlot_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int i = e.SelectedItemIndex;
            s.SelectedGameSaveSlot = s.SavedGameSlotsList[i];
            s.IsGameSlotSelected = true;
            
        }
    }
}