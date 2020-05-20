using System;
using System.Collections.Generic;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.Charts.FinancialChartsVM;

namespace TheManXS.ViewModel.FinancialVM.Financials.Charts
{
    class FilterBox : Picker
    {
        private static Color _headerBackgroundColor = Color.FromHex("#3f4343");
        private FinancialChartsVM _financialChartsVM;
        public FilterBox(FinancialChartsVM financialChartsVM, FilterBoxes filterBoxes)
        {
            _financialChartsVM = financialChartsVM;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = _headerBackgroundColor;
            ItemsSource = getItemSourceList(filterBoxes);
            SelectedIndexChanged += FilterBox_SelectedIndexChanged;
        }

        private void FilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker p = sender as Picker;
            int selectedIndex = p.SelectedIndex;

        }
        List<string> getItemSourceList(FilterBoxes fb)
        {
            switch (fb)
            {
                case FilterBoxes.FilterBox1:
                    return _financialChartsVM.FilterBox1ItemsList;
                case FilterBoxes.FilterBox2:
                    return _financialChartsVM.FilterBox2ItemsList;
                case FilterBoxes.FilterBox3:
                    return _financialChartsVM.FilterBox3ItemsList;

                case FilterBoxes.Total:
                default:
                    return _financialChartsVM.FilterBox1ItemsList;
            }
        }
    }
}
