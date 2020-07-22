using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TheManXS.ViewModel.MapBoardVM.Action
{
    class BackButtonX : Button
    {
        public BackButtonX()
        {
            HorizontalOptions = LayoutOptions.EndAndExpand;
            VerticalOptions = LayoutOptions.Center;
            Text = "X";
            FontAttributes = FontAttributes.Bold;
        }
    }
}
