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
        private List<FinancialsLineItems> _financialsLineItemsList;
        public FinancialsGrid(Game game, List<FinancialsLineItems> financialsLineItemsList)
        {
            _game = game;
            _financialsLineItemsList = financialsLineItemsList;
            new CalculatedFinancialValuesList(_game,_financialsLineItemsList);
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
            for (int i = 0; i < _financialsLineItemsList.Count; i++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }
        }

        void InitColumns()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            int qValuesToDisplay = 5;
            int valuesColumnsWidth = QC.ScreenWidth / (qValuesToDisplay + 1);
            for (int i = 0; i < (_financialsLineItemsList[0].ValuesArray.Length); i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(valuesColumnsWidth, GridUnitType.Absolute) });
            }
        }

        void AddCompanyNames()
        {
            for (int column = 1; column <= QC.PlayerQ; column++)
            {
                Children.Add(new RowTypeLabel(_game.PlayerList[column].Name));
            }
        }

        void AddData()
        {
            foreach (FinancialsLineItems f in _financialsLineItemsList)
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
