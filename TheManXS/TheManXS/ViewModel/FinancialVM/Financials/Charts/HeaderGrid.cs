using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using static TheManXS.ViewModel.FinancialVM.Financials.Charts.FinancialChartsVM;

namespace TheManXS.ViewModel.FinancialVM.Financials.Charts
{
    class HeaderGrid : Grid
    {
        enum Columns { FilterBox1, FilterBox2, FilterBox3, Total }
        enum Rows { HeaderRow, Total }
        private int _headerRowHeight;
        private double _headerHeightRatio = 0.05;
        FinancialChartsVM _financialChartsVM;

        public HeaderGrid(FinancialChartsVM financialChartsVM)
        {
            _financialChartsVM = financialChartsVM;
            _headerRowHeight = (int)(QC.ScreenHeight * _headerHeightRatio);
            InitFilterBoxes();
            InitGrid();
        }
        private void InitGrid()
        {
            for (int row = 0; row < (int)Rows.Total; row++)
            { RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_headerRowHeight, GridUnitType.Absolute) }); }
            for (int i = 0; i < (int)Columns.Total; i++) { ColumnDefinitions.Add(new ColumnDefinition()); }
        }
        private void InitFilterBoxes()
        {
            for (int columns = 0; columns < (int)Columns.Total; columns++)
            {
                Children.Add(new FilterBox(_financialChartsVM, (FilterBoxes)columns),columns, (int)Rows.HeaderRow);
            }
        }
    }
}
