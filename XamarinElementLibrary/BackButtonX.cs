using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinElementLibrary
{
    class BackButtonX : Button
    {
        public BackButtonX()
        {
            Text = "X";
            HorizontalOptions = LayoutOptions.EndAndExpand;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            FontAttributes = FontAttributes.Bold;
            BackgroundColor = Color.Transparent;
            TextColor = Color.Black;
        }
    }
}
