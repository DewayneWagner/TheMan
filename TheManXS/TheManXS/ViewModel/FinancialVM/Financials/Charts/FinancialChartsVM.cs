using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.Services;
//using Microcharts.Forms;
using Xamarin.Forms;
using QC = TheManXS.Model.Settings.QuickConstants;
using static TheManXS.ViewModel.FinancialVM.Financials.Charts.FinancialChartsVM;

namespace TheManXS.ViewModel.FinancialVM.Financials.Charts
{
    public class FinancialChartsVM : BaseViewModel
    {
        private enum GridRows { Header, Chart, Total }        
        public enum ChartType { ChartType1, ChartType2, ChartType3, Total }
        public enum FilterBoxes { FilterBox1, FilterBox2, FilterBox3, Total }
        Game _game;
        private int _headerRowHeight = 50;
        public FinancialChartsVM(Game game)
        {
            _game = game;
            Content = ChartGrid = GetGrid();
            InitLists();
            InitHeaderGrid();
        }

        private Grid _chartGrid;
        public Grid ChartGrid
        {
            get => _chartGrid;
            set
            {
                _chartGrid = value;
                SetValue(ref _chartGrid, value);
            }
        }

        public List<string> FilterBox1ItemsList { get; set; } = new List<string>();
        public List<string> FilterBox2ItemsList { get; set; } = new List<string>();
        public List<string> FilterBox3ItemsList { get; set; } = new List<string>();

        private Grid GetGrid()
        {
            Grid chartGrid = new Grid();
            chartGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
            chartGrid.VerticalOptions = LayoutOptions.FillAndExpand;

            for(int row = 0; row < (int)GridRows.Total; row++) { chartGrid.RowDefinitions.Add(new RowDefinition()); }
            chartGrid.ColumnDefinitions.Add(new ColumnDefinition());
            chartGrid.BackgroundColor = Color.DarkGray;
            return chartGrid;
        }
        void InitLists()
        {
            FilterBoxStuff fbs = new FilterBoxStuff();
            FilterBox1ItemsList = fbs.GetListOfItems(FilterBoxes.FilterBox1);
            FilterBox2ItemsList = fbs.GetListOfItems(FilterBoxes.FilterBox2);
            FilterBox3ItemsList = fbs.GetListOfItems(FilterBoxes.FilterBox3);
        }
        void InitHeaderGrid()
        {
            ChartGrid.Children.Add(new HeaderGrid(this));
        }
    }
}
