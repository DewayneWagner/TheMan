using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheManXS.ViewModel.Services;
using TheManXS.ViewModel.Style;
using Xamarin.Forms;

namespace TheManXS.ViewModel.Style
{
    public class ColorPaletteVM : BaseViewModel
    {
        

        public ColorPaletteVM()
        {
            PaletteColorList = new PaletteColorList();
            CompressedLayout.SetIsHeadless(this, true);
        }
        public PaletteColorList PaletteColorList { get; set; }
    }
}
