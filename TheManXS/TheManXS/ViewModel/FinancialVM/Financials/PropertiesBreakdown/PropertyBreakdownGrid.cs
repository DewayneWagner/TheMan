using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Units;
using System.Linq;
using TheManXS.Model.Services.EntityFrameWork;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class PropertyBreakdownGrid : Grid
    {
        public enum PropertyBreakdownColumns { Company, ResourceType, Status, Production, PPE, Revenue, OPEX, GrossProfitD, 
            GrossProfitP, ActionButton, Total }

        public enum HeaderFilterBoxType { Company, Resource, Status, Total }

        private enum HeaderRows { TitleLabel, FilterOrSort, Total }

        public const string HeaderAutomationID = "Header";
        public const string DataTableAutomationID = "DataTable";
        private static Color _headerBackgroundColor = Color.FromHex("#3f4343");
        Game _game;

        private HeaderFilterBox _companyFilterBox;
        private HeaderFilterBox _resourceFilterBox;
        private HeaderFilterBox _statusFilterBox;

        private int _dataRowHeight;
        private int _headerRowHeight;

        public PropertyBreakdownGrid(Game game)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);

            PropertyBreakdownListOfAllProducingProperties = new AllPropertyBreakdownList(_game);
            _dataRowHeight = (int)(QC.ScreenHeight * 0.05);
            _headerRowHeight = (int)(_dataRowHeight * 0.6);
            SortList();
            InitGrid();
            SetPropertiesOfGrid();

            AddHeaderLabels();
            AddHeaderFilterBoxes();
            AddHeaderButtons();
            
            AddDataValuesToGrid();
        }
        public AllPropertyBreakdownList PropertyBreakdownListOfAllProducingProperties { get; set; }
        

        void InitGrid()
        {
            initColumns();
            initRows();
            
            void initColumns()
            {
                for (int i = 0; i < (int)PropertyBreakdownColumns.Total; i++)
                { ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star }); }
            }
        }

        void initRows()
        {
            initHeaderRows();
            initDataRows();

            void initHeaderRows()
            {
                for (int i = 0; i < (int)HeaderRows.Total; i++)
                {
                    RowDefinitions.Add(new RowDefinition()
                        { Height = new GridLength(_headerRowHeight, GridUnitType.Absolute)});
                }
            }
            void initDataRows()
            {
                for (int i = 0; i < PropertyBreakdownListOfAllProducingProperties.PropertyBreakdownDisplayList.Count; i++)
                {
                    RowDefinitions.Add(new RowDefinition()
                    { Height = new GridLength(_dataRowHeight, GridUnitType.Absolute) });
                }
            }            
        }
        public void UpdateGrid()
        {
            deleteAllDataItems();

            while (RowDefinitions.Count > (int)HeaderRows.Total) { RowDefinitions.RemoveAt(RowDefinitions.Count - 1); }

            int rowsRequired = PropertyBreakdownListOfAllProducingProperties.PropertyBreakdownDisplayList.Count + 
                (int)HeaderRows.Total;
                
            for (int i = 0; i < rowsRequired; i++) { RowDefinitions.Add(new RowDefinition()
                { Height = new GridLength(_dataRowHeight, GridUnitType.Absolute) }); }

            AddDataValuesToGrid();

            void deleteAllDataItems()
            {
                var dataItemList = Children.Where(d => d.AutomationId == DataTableAutomationID).ToList();
                foreach(var item in dataItemList) { Children.Remove(item); }
            }
        }
        
        void SetPropertiesOfGrid()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            RowSpacing = 1;
            ColumnSpacing = 1;
            BackgroundColor = Color.DarkGray;
        }

        void AddHeaderLabels()
        {
            for (int c = 0; c < (int)PropertyBreakdownColumns.Total; c++)
            {
                HeaderLabel h = new HeaderLabel((PropertyBreakdownColumns)c);
                h.BackgroundColor = _headerBackgroundColor;
                if (c == (int)PropertyBreakdownColumns.GrossProfitD)
                {
                    this.Children.Add(h, c, (int)HeaderRows.TitleLabel);
                    Grid.SetColumnSpan(h, 2);
                }
                else if (c == (int)PropertyBreakdownColumns.GrossProfitP) {; } // do nothing
                else { this.Children.Add(h, c, (int)HeaderRows.TitleLabel); }                
            }
        }

        void AddHeaderFilterBoxes()
        {
            _companyFilterBox = new HeaderFilterBox(_game, HeaderFilterBoxType.Company);
            _companyFilterBox.BackgroundColor = _headerBackgroundColor;
            _companyFilterBox.SelectedIndexChanged += _companyFilterBox_SelectedIndexChanged;
            Children.Add(_companyFilterBox, (int)HeaderFilterBoxType.Company, (int)HeaderRows.FilterOrSort);

            _resourceFilterBox = new HeaderFilterBox(_game, HeaderFilterBoxType.Resource);
            _resourceFilterBox.BackgroundColor = _headerBackgroundColor;
            _resourceFilterBox.SelectedIndexChanged += _resourceFilterBox_SelectedIndexChanged;
            Children.Add(_resourceFilterBox, (int)HeaderFilterBoxType.Resource, (int)HeaderRows.FilterOrSort);

            _statusFilterBox = new HeaderFilterBox(_game, HeaderFilterBoxType.Status);
            _statusFilterBox.BackgroundColor = _headerBackgroundColor;
            _statusFilterBox.SelectedIndexChanged += _statusFilterBox_SelectedIndexChanged;
            Children.Add(_statusFilterBox, (int)HeaderFilterBoxType.Status, (int)HeaderRows.FilterOrSort);
        }

        private void _companyFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string companyName = (string)_companyFilterBox.ItemsSource[picker.SelectedIndex];

            PropertyBreakdownListOfAllProducingProperties.AddFilter(AllPropertyBreakdownList.FilterType.Company, companyName);
            UpdateGrid();
        }

        private void _resourceFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string resourceType = (string)_resourceFilterBox.ItemsSource[picker.SelectedIndex];

            PropertyBreakdownListOfAllProducingProperties.AddFilter(AllPropertyBreakdownList.FilterType.Resource, resourceType);
            UpdateGrid();
        }

        private void _statusFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string statusType = (string)_statusFilterBox.ItemsSource[picker.SelectedIndex];

            PropertyBreakdownListOfAllProducingProperties.AddFilter(AllPropertyBreakdownList.FilterType.Status, statusType);
            UpdateGrid();
        }

        void AddHeaderButtons()
        {
            for (int column = (int)PropertyBreakdownColumns.Production; column < (int)PropertyBreakdownColumns.Total; column++)
            {
                SortButton sb = new SortButton((PropertyBreakdownColumns)column, this);
                sb.BackgroundColor = _headerBackgroundColor;
                Children.Add(new ColorBackgroundBoxView(), column, (int)HeaderRows.FilterOrSort);
                if (column != (int)PropertyBreakdownColumns.ActionButton) { Children.Add(sb, column, (int)HeaderRows.FilterOrSort); }
            }
        }
        void SortList()
        {
            PropertyBreakdownListOfAllProducingProperties.OrderBy(p => p.Status)
                .ThenBy(p => p.Resource)
                .ThenBy(p => p.CompanyName);
        }
        void AddDataValuesToGrid()
        {
            int row = (int)HeaderRows.Total;
            foreach (PropertyBreakdown pb in PropertyBreakdownListOfAllProducingProperties.PropertyBreakdownDisplayList)
            {
                AddRowToGrid(pb, row);
                row++;
            }
        }
        void AddRowToGrid(PropertyBreakdown bd, int row)
        {
            bool isOwnedByActivePlayer = bd.CompanyName == _game.ActivePlayer.Name ? true : false;
            Children.Add(new DataLabel(bd.CompanyName, isOwnedByActivePlayer), (int)PropertyBreakdownColumns.Company, row);
            Children.Add(new DataLabel(bd.GrossProfitD.ToString("c0"), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.GrossProfitD, row);
            Children.Add(new DataLabel(bd.GrossProfitP.ToString("p1"), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.GrossProfitP, row);
            Children.Add(new DataLabel(bd.OPEX.ToString("c0"), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.OPEX, row);
            Children.Add(new DataLabel(bd.PPE.ToString("c0"), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.PPE, row);
            Children.Add(new DataLabel(bd.Production.ToString(), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.Production, row);
            Children.Add(new DataLabel(bd.Resource.ToString(), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.ResourceType, row);
            Children.Add(new DataLabel(bd.Revenue.ToString("c0"), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.Revenue, row);
            Children.Add(new DataLabel(bd.Status.ToString(), isOwnedByActivePlayer), (int)PropertyBreakdownColumns.Status, row);
            Children.Add(new ActionButton(_game, bd), (int)PropertyBreakdownColumns.ActionButton, row);
        }

        class ColorBackgroundBoxView : BoxView
        {
            public ColorBackgroundBoxView()
            {
                BackgroundColor = _headerBackgroundColor;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                VerticalOptions = LayoutOptions.FillAndExpand;
            }
        }
    }
}
