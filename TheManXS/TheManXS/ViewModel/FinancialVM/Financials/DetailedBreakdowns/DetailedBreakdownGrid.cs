using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.FinancialVM.Financials.Charts;
using Windows.Gaming.UI;
using Windows.UI;
using Xamarin.Forms;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    class DetailedBreakdownGrid : Grid
    {
        private int _rowQ;
        private int _colQ;
        private List<DataRowList> _listOfDataRowLists;

        public DetailedBreakdownGrid(bool filterRowNeeded, List<DataRowList> listOfDataRowLists) 
        {
            _listOfDataRowLists = listOfDataRowLists;
            _rowQ = listOfDataRowLists.Count;
            _colQ = listOfDataRowLists[1].Count;
            InitGrid();
            AddLabelsToGrid();
        }

        public Grid DetailedBreakdownGrid { get; }
        protected bool FilterRowNeeded { get; set; }
        private void InitGrid()
        {
            for (int col = 0; col < _colQ; col++) { ColumnDefinitions.Add(new ColumnDefinition()); }
            for (int row = 0; row < _rowQ; row++) { RowDefinitions.Add(new RowDefinition()); }
        }
        private void AddLabelsToGrid()
        {
            for (int row = 0; row < _rowQ; row++)
            {
                for (int col = 0; col < _colQ; col++)
                {
                    this.Children.Add(_listOfDataRowLists[row][col]);
                }
            }
        }
    }
}
