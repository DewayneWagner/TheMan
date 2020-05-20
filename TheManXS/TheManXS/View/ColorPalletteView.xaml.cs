using System;
using TheManXS.ViewModel;
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
        ApplicationVM _applicationVM;

        public ColorPalletteView()
        {
            _applicationVM = (ApplicationVM)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ApplicationVM)];
            InitializeComponent();
            BindingContext = _colorPaletteVM = new ColorPaletteVM();
            //UpdateActiveButtonArray();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _colorPaletteVM.UpdateSelectedColors();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _colorPaletteVM.PaletteColorList.WritePColorsToBinaryFile();
            _applicationVM.UpdateColors(_colorPaletteVM.PaletteColorList);
        }
    }
}