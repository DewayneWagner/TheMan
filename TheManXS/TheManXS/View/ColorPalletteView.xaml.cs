using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheManXS.ViewModel.Style;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static TheManXS.ViewModel.Style.PaletteColorList;

namespace TheManXS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPalletteView : ContentPage
    {
        ColorPaletteVM _colorPaletteVM;

        private Color _selected = Color.CornflowerBlue;
        private Color _notSelected = Color.LightGreen;

        Button[] _activeButtonArray = new Button[(int)ColorTypes.Total];

        public ColorPalletteView()
        {
            InitializeComponent();
            //BindingContext = _colorPaletteVM = new ColorPaletteVM();
            //UpdateActiveButtonArray();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _colorPaletteVM.PaletteColorList.WritePColorsToBinaryFile();
        }

        private void C0Button_Clicked(object sender, EventArgs e) { }
        private void C1Button_Clicked(object sender, EventArgs e) { }
        private void C2Button_Clicked(object sender, EventArgs e) { }
        private void C3Button_Clicked(object sender, EventArgs e) { }
        private void C4Button_Clicked(object sender, EventArgs e) { }
        
        
        void UpdateActiveButtonArray()
        {
            var items = LV_ColorPalette.ItemsSource;
            
        }
    }
}