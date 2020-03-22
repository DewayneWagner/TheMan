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
            GrossProfitP, Total }

        public enum HeaderFilterBoxType { Company, Resource, Status, Total }
        private AllPropertyBreakdownList _propertyBreakdownListOfAllProducingProperties;
        private bool _allProducingPropertiesBeingDisplayed = true;
        private List<PropertyBreakdown> _activeListOnDisplay;
        private int _qRows;
        private static Color _dataGridBackGroundColor = Color.WhiteSmoke;
        Game _game;
        private HeaderFilterBox _companyFilterBox;
        private HeaderFilterBox _resourceFilterBox;
        private HeaderFilterBox _statusFilterBox;
        private int _rowHeight;

        public PropertyBreakdownGrid(Game game)
        {
            _game = game;
            CompressedLayout.SetIsHeadless(this, true);

            _propertyBreakdownListOfAllProducingProperties = new AllPropertyBreakdownList(_game);
            _qRows = getNumberOfRows();
            _rowHeight = (int)(QC.ScreenHeight * 0.05);
            SortList();
            InitGrid();
            SetPropertiesOfGrid();
            AddHeaderFilterBoxes();
            AddHeaderButtons();
            //AddHeaderLabels();
            AddDataValuesToGrid();
        }
        int getNumberOfRows() => (_propertyBreakdownListOfAllProducingProperties.Count + 1);
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
            for (int i = 0; i < _qRows; i++) { RowDefinitions.Add(new RowDefinition() 
                { Height = new GridLength(_rowHeight,GridUnitType.Absolute)}); }
        }
        void UpdateGrid()
        {
            deleteAllLabels();
            while (RowDefinitions.Count > 1) { RowDefinitions.RemoveAt(RowDefinitions.Count - 1); }
            
            int rowsRequired = _allProducingPropertiesBeingDisplayed? 
                _propertyBreakdownListOfAllProducingProperties.Count + 1 : 
                _activeListOnDisplay.Count + 1;

            for (int i = 0; i < rowsRequired; i++) { RowDefinitions.Add(new RowDefinition()
                { Height = new GridLength(_rowHeight, GridUnitType.Absolute) }); }

            AddDataValuesToGrid();

            void deleteAllLabels()
            {
                var labelList = Children.Where(l => l.BackgroundColor == _dataGridBackGroundColor).ToList();

                foreach (var item in labelList)
                {
                    Children.Remove(item);
                }
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

            if(companyName == "All") { UpdateGrid(); }
            else
            {
                _activeListOnDisplay = _propertyBreakdownListOfAllProducingProperties.Where(p => p.CompanyName == companyName).ToList();
                _allProducingPropertiesBeingDisplayed = false;
               
                UpdateGrid();
                AddDataValuesToGrid();
            }  
        }

        private void _resourceFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string resourceType = (string)_resourceFilterBox.ItemsSource[picker.SelectedIndex];

            if (resourceType == "All") { UpdateGrid(); }
            else
            {
                RT rt = getResourceType();
                _activeListOnDisplay = _propertyBreakdownListOfAllProducingProperties.Where(s => s.Resource == rt).ToList();
                _allProducingPropertiesBeingDisplayed = false;

                UpdateGrid();
            }
            RT getResourceType()
            {
                for (int i = 0; i < (int)RT.Total; i++)
                {
                    if (Convert.ToString(resourceType) == Convert.ToString((RT)i)) { return (RT)i; }
                }
                return RT.Total;
            }
        }

        private void _statusFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            string statusType = (string)_statusFilterBox.ItemsSource[picker.SelectedIndex];

            if (statusType == "All") { UpdateGrid(); }
            else
            {
                ST st = getStatusType();
                _activeListOnDisplay = _propertyBreakdownListOfAllProducingProperties.Where(s => s.Status == st).ToList();
                _allProducingPropertiesBeingDisplayed = false;

                UpdateGrid();
            }
            ST getStatusType()
            {
                for (int i = 0; i < (int)ST.Total; i++)
                {
                    if (Convert.ToString(statusType) == Convert.ToString((ST)i)) { return (ST)i; }
                }
                return ST.Total;
            }
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
            switch (pbc)
            {
                case PropertyBreakdownColumns.Production:
                    if (_allProducingPropertiesBeingDisplayed) 
                    {
                        _propertyBreakdownListOfAllProducingProperties =
                            _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.Production).ToList();
                    }
                    else { _activeListOnDisplay.OrderBy(p => p.Production); }
                    break;

                case PropertyBreakdownColumns.PPE:
                    if (_allProducingPropertiesBeingDisplayed)
                        { _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.PPE); }
                    else { _activeListOnDisplay.OrderBy(p => p.PPE); }
                    break;

                case PropertyBreakdownColumns.Revenue:
                    if (_allProducingPropertiesBeingDisplayed)
                        { _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.Revenue); }
                    else { _activeListOnDisplay.OrderBy(p => p.Revenue); }
                    break;

                case PropertyBreakdownColumns.OPEX:
                    if (_allProducingPropertiesBeingDisplayed)
                        { _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.OPEX); }
                    else { _activeListOnDisplay.OrderBy(p => p.OPEX); }
                    break;

                case PropertyBreakdownColumns.GrossProfitD:
                    if (_allProducingPropertiesBeingDisplayed)
                        { _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.GrossProfitD); }
                    else { _activeListOnDisplay.OrderBy(p => p.GrossProfitD); }
                    break;
                case PropertyBreakdownColumns.GrossProfitP:
                    if (_allProducingPropertiesBeingDisplayed)
                        { _propertyBreakdownListOfAllProducingProperties.OrderBy(p => p.GrossProfitP); }
                    else { _activeListOnDisplay.OrderBy(p => p.GrossProfitP); }
                    break;

                default:
                    break;
            }
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
            if (_allProducingPropertiesBeingDisplayed)
            {
                for (int row = 1; row < _qRows; row++)
                {
                    AddRowToGrid(_propertyBreakdownListOfAllProducingProperties[(row - 1)], row);
                }
            }
            else
            {
                int row = 1;
                foreach (PropertyBreakdown pb in _activeListOnDisplay)
                {
                    AddRowToGrid(pb, row);
                    row++;
                }
            }
        }
        void AddRowToGrid(PropertyBreakdown bd, int row)
        {
            Children.Add(new DataLabel(bd.CompanyName), (int)PropertyBreakdownColumns.Company, row);
            Children.Add(new DataLabel(bd.GrossProfitD.ToString("c0")), (int)PropertyBreakdownColumns.GrossProfitD, row);
            Children.Add(new DataLabel(bd.GrossProfitP.ToString("p1")), (int)PropertyBreakdownColumns.GrossProfitP, row);
            Children.Add(new DataLabel(bd.OPEX.ToString("c0")), (int)PropertyBreakdownColumns.OPEX, row);
            Children.Add(new DataLabel(bd.PPE.ToString("c0")), (int)PropertyBreakdownColumns.PPE, row);
            Children.Add(new DataLabel(bd.Production.ToString()), (int)PropertyBreakdownColumns.Production, row);
            Children.Add(new DataLabel(bd.Resource.ToString()), (int)PropertyBreakdownColumns.ResourceType, row);
            Children.Add(new DataLabel(bd.Revenue.ToString("c0")), (int)PropertyBreakdownColumns.Revenue, row);
            Children.Add(new DataLabel(bd.Status.ToString()), (int)PropertyBreakdownColumns.Status, row);
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
                BackgroundColor = _dataGridBackGroundColor;
            }
        }
    }
}
