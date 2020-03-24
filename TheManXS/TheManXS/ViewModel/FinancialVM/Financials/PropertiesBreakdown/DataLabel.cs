using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    class DataLabel : Label
    {
        private static Color _dataGridBackGroundColor = Color.WhiteSmoke;
        private static Color _dataGridBackGroundColorForActivePlayerProperties = Color.LightGreen;
        private bool _isOwnedByActivePlayer;

        public DataLabel(string value, bool isOwnedByActivePlayer)
        {
            _isOwnedByActivePlayer = isOwnedByActivePlayer;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            Text = value;
            BackgroundColor = getBackGroundColor();
            AutomationId = PropertyBreakdownGrid.DataTableAutomationID;
        }
        Color getBackGroundColor()
        {
            return _isOwnedByActivePlayer ? _dataGridBackGroundColorForActivePlayerProperties : _dataGridBackGroundColor;
        }
    }
}
