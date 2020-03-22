using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown.PropertyBreakdownGrid;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class HeaderButton : Button
    {
        private PropertyBreakdownColumns _col;
        PropertyBreakdownGrid _pbg;
        private bool _isSortedAscending;
        public HeaderButton(PropertyBreakdownGrid pbg, PropertyBreakdownColumns type)
        {
            _pbg = pbg;
            _col = type;
            InitButton();

            Clicked += HeaderButton_Clicked;
        }

        private void HeaderButton_Clicked(object sender, EventArgs e) => _pbg.SortByColumn(_col);

        private void InitButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Red;
            TextColor = Color.White;
            FontAttributes = FontAttributes.Bold;
            Text = Convert.ToString(_col);
        }
    }
}
