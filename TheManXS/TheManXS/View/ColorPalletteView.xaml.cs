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
            BindingContext = _colorPaletteVM = new ColorPaletteVM();
            UpdateActiveButtonArray();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _colorPaletteVM.PaletteColorList.WritePColorsToBinaryFile();
        }

        private void C0Button_Clicked(object sender, EventArgs e) => ProcessButtonClick(ColorTypes.C0, (Button)sender);
        private void C1Button_Clicked(object sender, EventArgs e) => ProcessButtonClick(ColorTypes.C1, (Button)sender);
        private void C2Button_Clicked(object sender, EventArgs e) => ProcessButtonClick(ColorTypes.C2, (Button)sender);
        private void C3Button_Clicked(object sender, EventArgs e) => ProcessButtonClick(ColorTypes.C3, (Button)sender);
        private void C4Button_Clicked(object sender, EventArgs e) => ProcessButtonClick(ColorTypes.C4, (Button)sender);
        
        private void ProcessButtonClick(ColorTypes ct, Button button)
        {
            int id = (int)button.CommandParameter;
            PaletteColor pc = _colorPaletteVM.PaletteColorList[id];
            _activeButtonArray[id].BackgroundColor = _notSelected;
            button.BackgroundColor = _selected;
            _activeButtonArray[id] = button;

            switch (ct)
            {
                case ColorTypes.C0:
                    foreach(PaletteColor p in _colorPaletteVM.PaletteColorList) { p.IsC0 = false; }
                    pc.IsC0 = true;
                    break;
                case ColorTypes.C1:
                    foreach (PaletteColor p in _colorPaletteVM.PaletteColorList) { p.IsC1 = false; }
                    pc.IsC1 = true;
                    break;
                case ColorTypes.C2:
                    foreach (PaletteColor p in _colorPaletteVM.PaletteColorList) { p.IsC2 = false; }
                    pc.IsC2 = true;
                    break;
                case ColorTypes.C3:
                    foreach (PaletteColor p in _colorPaletteVM.PaletteColorList) { p.IsC3 = false; }
                    pc.IsC3 = true;
                    break;
                case ColorTypes.C4:
                    foreach (PaletteColor p in _colorPaletteVM.PaletteColorList) { p.IsC4 = false; }
                    pc.IsC4 = true;
                    break;
                default:
                    break;
            }
        }
        void UpdateActiveButtonArray()
        {
            var items = LV_ColorPalette.ItemsSource;
            
        }
    }
}