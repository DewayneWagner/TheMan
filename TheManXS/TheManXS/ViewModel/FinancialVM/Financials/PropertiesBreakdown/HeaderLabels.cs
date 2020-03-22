using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown.PropertyBreakdownGrid;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class HeaderLabels : Label
    {
        Game _game;
        public HeaderLabels(Game game, PropertyBreakdownColumns type)
        {
            _game = game;
            InitPropertiesOfLabel();
            Text = Convert.ToString(type);
        }
        void InitPropertiesOfLabel()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            BackgroundColor = _game.PaletteColors.GetColorFromColorScheme(ViewModel.Style.PaletteColorList.ColorTypes.C0);
            FontAttributes = FontAttributes.Bold;
        }
    }
}
