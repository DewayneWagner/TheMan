using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    class RowTypeLabel : Label
    {
        FinancialsLineItems _financialsLineItems;
        Color _mainHeadingColor = Color.Crimson;
        public RowTypeLabel(FinancialsLineItems financialLineItem)
        {
            _financialsLineItems = financialLineItem;
            CompressedLayout.SetIsHeadless(this, true);
            InitLabel();
        }
        public RowTypeLabel(string companyName)
        {
            InitColumnHeadingLabel(companyName);
        }
        void InitColumnHeadingLabel(string companyName)
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            FontAttributes = FontAttributes.Bold;
            TextDecorations = TextDecorations.Underline;
            BackgroundColor = _mainHeadingColor;
            TextColor = Color.White;
            Text = companyName;
        }
        void InitLabel()
        {
            initFieldsCommonToAllLabels();

            switch (_financialsLineItems.FormatType)
            {
                case FinancialsVM.FormatTypes.MainHeading:
                    initMainHeading();
                    break;
                case FinancialsVM.FormatTypes.SubHeading:
                    initSubHeading();
                    break;
                case FinancialsVM.FormatTypes.LineItem:
                    initLineItems();
                    break;
                case FinancialsVM.FormatTypes.Totals:
                    initTotals();
                    break;
                case FinancialsVM.FormatTypes.CompanyNameColHeading:
                    initCompanyNameColHeading();
                    break;
                default:
                    break;
            }

            void initFieldsCommonToAllLabels()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand;
                VerticalOptions = LayoutOptions.FillAndExpand;
                HorizontalTextAlignment = TextAlignment.Center;
                WidthRequest = FinancialsVM.ColumnWidth;
            }

            void initMainHeading()
            {
                FontAttributes = FontAttributes.Bold;
                TextDecorations = TextDecorations.Underline;
                BackgroundColor = _mainHeadingColor;
                TextColor = Color.White;
            }

            void initSubHeading()
            {
                FontAttributes = FontAttributes.Bold;
                Text = "";
            }

            void initLineItems() 
            {
                HorizontalTextAlignment = TextAlignment.Center;
                Text = "";
            }

            void initTotals()
            {
                FontAttributes = FontAttributes.Bold;
                BackgroundColor = Color.LightGray;
                Text = "";
            }
            
            void initCompanyNameColHeading()
            {
                FontAttributes = FontAttributes.Bold;
                BackgroundColor = Color.LightSlateGray;
                HorizontalTextAlignment = TextAlignment.Center;
                VerticalTextAlignment = TextAlignment.Center;
                TextDecorations = TextDecorations.Underline;
            }
        }
    }
}
