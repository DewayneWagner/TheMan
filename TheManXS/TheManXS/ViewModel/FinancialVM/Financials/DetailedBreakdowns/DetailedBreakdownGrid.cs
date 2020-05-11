using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.FinancialVM.Financials.Charts;
using Windows.Gaming.UI;
using Windows.UI;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    class DetailedBreakdownGrid : Grid
    {
        private int _rowQ;
        private int _colQ;
        private double _rowHeight;
        private List<DataRowList> _listOfDataRowLists;

        public DetailedBreakdownGrid(bool filterRowNeeded, List<DataRowList> listOfDataRowLists)
        {
            InitPropertiesOfGrid();
            _listOfDataRowLists = listOfDataRowLists;
            _rowQ = listOfDataRowLists.Count;
            _colQ = listOfDataRowLists[5].Count;
            _rowHeight = QC.ScreenHeight / (_rowQ * 2.25);
            InitGrid();
            AddLabelsToGrid();
        }

        void InitPropertiesOfGrid()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            CompressedLayout.SetIsHeadless(this, true);
            ColumnSpacing = 0;
            RowSpacing = 0;
            Margin = 5;
        }

        protected bool FilterRowNeeded { get; set; }
        private void InitGrid()
        {
            for (int col = 0; col < _colQ; col++) { ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star }); }
            for (int row = 0; row < _rowQ; row++) { RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_rowHeight, GridUnitType.Absolute) }); }
        }
        private void AddLabelsToGrid()
        {
            for (int row = 0; row < _rowQ; row++)
            {
                for (int col = 0; col < _colQ; col++)
                {
                    this.Children.Add(_listOfDataRowLists[row][col],col,row);
                }
            }
        }
    }
}
