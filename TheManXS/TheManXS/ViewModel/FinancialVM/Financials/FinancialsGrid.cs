using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class FinancialsGrid : Grid
    {
        Game _game;
        private FinancialsLineItems[] _financialsLineItemsArray;
        public FinancialsGrid(Game game, FinancialsLineItems[] financialsLineItemsArray)
        {
            _game = game;
            _financialsLineItemsArray = financialsLineItemsArray;
            new CalculatedFinancialValuesList(_game,_financialsLineItemsArray);
            InitPropertiesOfGrid();
            InitRows();
            InitColumns();
            AddCompanyNames();
            AddData();
        }

        void InitPropertiesOfGrid()
        {
            ColumnSpacing = 1;
            RowSpacing = 1;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            Margin = 5;
        }

        void InitRows()
        {
            for (int i = 0; i < _financialsLineItemsArray.Length; i++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }
        }

        void InitColumns()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            int qValuesToDisplay = 5;
            int valuesColumnsWidth = QC.ScreenWidth / (qValuesToDisplay + 1);
            for (int i = 0; i < (_financialsLineItemsArray[0].ValuesArray.Length + 1); i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(valuesColumnsWidth, GridUnitType.Absolute) });
            }
        }

        void AddCompanyNames()
        {
            for (int column = 1; column <= QC.PlayerQ; column++)
            {
                Children.Add(new RowTypeLabel(_game.PlayerList[column-1].Name));
            }
        }

        void AddData()
        {
            foreach (FinancialsLineItems f in _financialsLineItemsArray)
            {
                // add row heading
                RowTypeLabel rtl = new RowTypeLabel(f);
                f.FinalText = f.FinalText;
                Children.Add(rtl, 0, (int)f.LineItemType);

                // add data (if there is data)
                for (int i = 0; i < FinancialsVM.QDATACOLUMNS; i++)
                {
                    RowTypeLabel l = new RowTypeLabel(f);
                    l.Text = f.ValuesArray[i];
                    Children.Add(l, (i + 1), (int)f.LineItemType);
                }
            }
        }
    }
}
