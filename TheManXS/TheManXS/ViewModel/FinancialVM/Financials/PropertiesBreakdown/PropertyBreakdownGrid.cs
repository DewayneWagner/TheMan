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

        public const string HeaderAutomationID = "Header";
        public const string DataTableAutomationID = "DataTable";

        private AllPropertyBreakdownList _propertyBreakdownListOfAllProducingProperties;
        private static bool _isOwnedByActivePlayer;
        private static Color _dataGridBackGroundColor = Color.WhiteSmoke;
        private static Color _dataGridBackGroundColorForActivePlayerProperties = Color.LightGreen;
        Game _game;

        private HeaderFilterBox _companyFilterBox;
        private HeaderFilterBox _resourceFilterBox;
        private HeaderFilterBox _statusFilterBox;
        private int _rowHeight;
        private int _numberOfRowsInHeader = 2;

        public PropertyBreakdownGrid(Game game)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);

            _propertyBreakdownListOfAllProducingProperties = new AllPropertyBreakdownList(_game);
            _rowHeight = (int)(QC.ScreenHeight * 0.05);
            SortList();
            InitGrid();
            SetPropertiesOfGrid();
            AddHeaderFilterBoxes();
            AddHeaderButtons();
            
            AddDataValuesToGrid();
        }
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
            for (int i = 0; i < _propertyBreakdownListOfAllProducingProperties.PropertyBreakdownDisplayList.Count; i++) { RowDefinitions.Add(new RowDefinition() 
                { Height = new GridLength(_rowHeight,GridUnitType.Absolute)}); }
        }
        void UpdateGrid()
        {
            deleteAllDataItems();

            while (RowDefinitions.Count > 1) { RowDefinitions.RemoveAt(RowDefinitions.Count - 1); }

            int rowsRequired = _propertyBreakdownListOfAllProducingProperties.PropertyBreakdownDisplayList.Count + 1;
                
            for (int i = 0; i < rowsRequired; i++) { RowDefinitions.Add(new RowDefinition()
                { Height = new GridLength(_rowHeight, GridUnitType.Absolute) }); }

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
        void AddHeaderFilterBoxes()
        {
            _companyFilterBox = new HeaderFilterBox(_game, HeaderFilterBoxType.Company);
            _companyFilterBox.SelectedIndexChanged += _companyFilterBox_SelectedIndexChanged;
            Children.Add(_companyFilterBox, (int)HeaderFilterBoxType.Company, 0);

            _resourceFilterBox = new HeaderFilterBox(_game, HeaderFilterBoxType.Resource);
            _resourceFilterBox.SelectedIndexChanged += _resourceFilterBox_SelectedIndexChanged;
            Children.Add(_resourceFilterBox, (int)HeaderFilterBoxType.Resource, 0);

            _statusFilterBox = new HeaderFilterBox(_game, HeaderFilterBoxType.Status);
            _statusFilterBox.SelectedIndexChanged += _statusFilterBox_SelectedIndexChanged;
            Children.Add(_statusFilterBox, (int)HeaderFilterBoxType.Status, 0);
        }

        private void _companyFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string companyName = (string)_companyFilterBox.ItemsSource[picker.SelectedIndex];

            _propertyBreakdownListOfAllProducingProperties.AddFilter(AllPropertyBreakdownList.FilterType.Company, companyName);
            UpdateGrid();
        }

        private void _resourceFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string resourceType = (string)_resourceFilterBox.ItemsSource[picker.SelectedIndex];

            _propertyBreakdownListOfAllProducingProperties.AddFilter(AllPropertyBreakdownList.FilterType.Resource, resourceType);
            UpdateGrid();
        }

        private void _statusFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string statusType = (string)_statusFilterBox.ItemsSource[picker.SelectedIndex];

            _propertyBreakdownListOfAllProducingProperties.AddFilter(AllPropertyBreakdownList.FilterType.Status, statusType);
            UpdateGrid();
        }

        void AddHeaderButtons()
        {
            for (int column = (int)PropertyBreakdownColumns.Production; column < (int)PropertyBreakdownColumns.Total; column++)
            {
                Children.Add(new HeaderButton(this,(PropertyBreakdownColumns)column), column, 0);
            }
        }
        public void SortByColumn(PropertyBreakdownColumns pbc)
        {
            _propertyBreakdownListOfAllProducingProperties.SortDataByColumn(pbc);
            UpdateGrid();
        }
        void SortList()
        {
            _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.Status)
                .ThenBy(p => p.Resource)
                .ThenBy(p => p.CompanyName);
        }
        void AddDataValuesToGrid()
        {
            int row = 1;
            foreach (PropertyBreakdown pb in _propertyBreakdownListOfAllProducingProperties.PropertyBreakdownDisplayList)
            {
                AddRowToGrid(pb, row);
                row++;
            }
        }
        void AddRowToGrid(PropertyBreakdown bd, int row)
        {
            _isOwnedByActivePlayer = bd.CompanyName == _game.ActivePlayer.Name ? true : false;
            Children.Add(new DataLabel(bd.CompanyName), (int)PropertyBreakdownColumns.Company, row);
            Children.Add(new DataLabel(bd.GrossProfitD.ToString("c0")), (int)PropertyBreakdownColumns.GrossProfitD, row);
            Children.Add(new DataLabel(bd.GrossProfitP.ToString("p1")), (int)PropertyBreakdownColumns.GrossProfitP, row);
            Children.Add(new DataLabel(bd.OPEX.ToString("c0")), (int)PropertyBreakdownColumns.OPEX, row);
            Children.Add(new DataLabel(bd.PPE.ToString("c0")), (int)PropertyBreakdownColumns.PPE, row);
            Children.Add(new DataLabel(bd.Production.ToString()), (int)PropertyBreakdownColumns.Production, row);
            Children.Add(new DataLabel(bd.Resource.ToString()), (int)PropertyBreakdownColumns.ResourceType, row);
            Children.Add(new DataLabel(bd.Revenue.ToString("c0")), (int)PropertyBreakdownColumns.Revenue, row);
            Children.Add(new DataLabel(bd.Status.ToString()), (int)PropertyBreakdownColumns.Status, row);
            Children.Add(new ActionButton(_game, bd), (int)PropertyBreakdownColumns.ActionButton, row);
        }
        
        class DataLabel : Label
        {
            public DataLabel(string value)
            {
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
}
