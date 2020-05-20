using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown.PropertyBreakdownGrid;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    class HeaderLabel : Label
    {
        public HeaderLabel(PropertyBreakdownColumns propertyBreakdownColumns)
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            FontAttributes = FontAttributes.Bold;
            TextColor = Color.White;
            Text = getText(propertyBreakdownColumns);
        }

        private string getText(PropertyBreakdownColumns propertyBreakdownColumns)
        {
            switch (propertyBreakdownColumns)
            {
                case PropertyBreakdownColumns.Company:
                    return "Company Name";
                case PropertyBreakdownColumns.ResourceType:
                    return "Resource Type";
                case PropertyBreakdownColumns.Status:
                    return "Status";
                case PropertyBreakdownColumns.Production:
                    return "Production";
                case PropertyBreakdownColumns.PPE:
                    return "Asset Value";
                case PropertyBreakdownColumns.Revenue:
                    return "Revenue";
                case PropertyBreakdownColumns.OPEX:
                    return "OPEX";
                case PropertyBreakdownColumns.GrossProfitD:
                    return "Gross Profit";

                case PropertyBreakdownColumns.GrossProfitP:
                case PropertyBreakdownColumns.ActionButton:
                case PropertyBreakdownColumns.Total:
                default:
                    return "";
            }
        }
    }
}
