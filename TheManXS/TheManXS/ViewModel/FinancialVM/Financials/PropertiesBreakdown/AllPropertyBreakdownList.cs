using System;
using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Main;
using TheManXS.Model.Units;
using static TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown.PropertyBreakdownGrid;
using QC = TheManXS.Model.Settings.QuickConstants;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;

namespace TheManXS.ViewModel.FinancialVM.Financials.PropertiesBreakdown
{
    public class AllPropertyBreakdownList : List<PropertyBreakdown>
    {
        public enum FilterType { Company, Resource, Status }
        Game _game;
        private bool _isFilteredByResource;
        private bool _isFilteredByStatus;
        private bool _isFilteredByCompany;

        public AllPropertyBreakdownList(Game game)
        {
            _game = game;
            LoadListWithAllOwnedSQs();
            LoadListWithAllOwnedUnits();
        }

        private void LoadListWithAllOwnedSQs()
        {
            List<SQ> sqList = _game.SQList
                .Where(p => p.OwnerNumber != QC.PlayerIndexTheMan)
                .Where(p => !p.IsPartOfUnit)
                .ToList();

            PropertyBreakdownDisplayList = new List<PropertyBreakdown>();

            foreach (SQ sq in sqList)
            {
                PropertyBreakdown pb = new PropertyBreakdown(_game, sq);
                Add(pb);
                PropertyBreakdownDisplayList.Add(pb);
            }
        }
        private void LoadListWithAllOwnedUnits()
        {
            foreach (Unit unit in _game.ListOfCreatedProductionUnits)
            {
                Add(new PropertyBreakdown(_game, unit));
            }
        }

        public List<PropertyBreakdown> PropertyBreakdownDisplayList { get; set; }
        public RT ResourceTypeDisplay { get; set; }
        public ST StatusTypeDisplay { get; set; }
        public string CompanyNameToDisplay { get; set; }
        public void AddFilter(FilterType ft, string filterBy)
        {
            if (filterBy == "All") { resetFilterTypeToAll(); }
            else { setFilterType(); }

            PropertyBreakdownDisplayList = new List<PropertyBreakdown>();
            setFilter();

            void resetFilterTypeToAll()
            {
                switch (ft)
                {
                    case FilterType.Company:
                        _isFilteredByCompany = false;
                        break;

                    case FilterType.Resource:
                        _isFilteredByResource = false;
                        break;

                    case FilterType.Status:
                        _isFilteredByStatus = false;
                        break;

                    default:
                        break;
                }
            }
            void setFilterType()
            {
                switch (ft)
                {
                    case FilterType.Company:
                        _isFilteredByCompany = true;
                        CompanyNameToDisplay = filterBy;
                        break;

                    case FilterType.Resource:
                        _isFilteredByResource = true;
                        setResourceType();
                        break;

                    case FilterType.Status:
                        _isFilteredByStatus = true;
                        setStatusType();
                        break;

                    default:
                        break;
                }
            }
            void setResourceType()
            {
                for (int i = 0; i < (int)RT.Total; i++) { if (Convert.ToString((RT)i) == filterBy) { ResourceTypeDisplay = (RT)i; } }
            }
            void setStatusType()
            {
                for (int i = 0; i < (int)ST.Total; i++) { if (Convert.ToString((ST)i) == filterBy) { StatusTypeDisplay = (ST)i; } }
            }
            void setFilter()
            {
                if (_isFilteredByCompany && _isFilteredByResource && _isFilteredByStatus)
                {
                    PropertyBreakdownDisplayList = this.Where(p => p.CompanyName == CompanyNameToDisplay)
                        .Where(p => p.Resource == ResourceTypeDisplay)
                        .Where(p => p.Status == StatusTypeDisplay)
                        .ToList();
                }
                else if (_isFilteredByCompany && _isFilteredByResource)
                {
                    PropertyBreakdownDisplayList = this.Where(p => p.CompanyName == CompanyNameToDisplay)
                        .Where(p => p.Resource == ResourceTypeDisplay)
                        .ToList();
                }
                else if (_isFilteredByCompany)
                {
                    PropertyBreakdownDisplayList = this.Where(p => p.CompanyName == CompanyNameToDisplay).ToList();
                }
                else if (_isFilteredByResource && _isFilteredByStatus)
                {
                    PropertyBreakdownDisplayList = this.Where(p => p.Resource == ResourceTypeDisplay)
                        .Where(p => p.Status == StatusTypeDisplay)
                        .ToList();
                }
                else if (_isFilteredByResource)
                {
                    PropertyBreakdownDisplayList = this.Where(p => p.Resource == ResourceTypeDisplay).ToList();
                }
                else if (_isFilteredByStatus)
                {
                    PropertyBreakdownDisplayList = this.Where(p => p.Status == StatusTypeDisplay).ToList();
                }
                else if (!_isFilteredByCompany && !_isFilteredByResource && !_isFilteredByStatus)
                {
                    PropertyBreakdownDisplayList = this.ToList();
                }
            }
        }
        public void SortDataByColumnDescending(PropertyBreakdownColumns pbc)
        {
            List<PropertyBreakdown> tempList = new List<PropertyBreakdown>();
            switch (pbc)
            {
                case PropertyBreakdownColumns.Production:
                    tempList = PropertyBreakdownDisplayList.OrderByDescending(p => p.Production).ToList();
                    break;
                case PropertyBreakdownColumns.PPE:
                    tempList = PropertyBreakdownDisplayList.OrderByDescending(p => p.PPE).ToList();
                    break;
                case PropertyBreakdownColumns.Revenue:
                    tempList = PropertyBreakdownDisplayList.OrderByDescending(p => p.Revenue).ToList();
                    break;
                case PropertyBreakdownColumns.OPEX:
                    tempList = PropertyBreakdownDisplayList.OrderByDescending(p => p.OPEX).ToList();
                    break;
                case PropertyBreakdownColumns.GrossProfitD:
                    tempList = PropertyBreakdownDisplayList.OrderByDescending(p => p.GrossProfitD).ToList();
                    break;
                case PropertyBreakdownColumns.GrossProfitP:
                    tempList = PropertyBreakdownDisplayList.OrderByDescending(p => p.GrossProfitP).ToList();
                    break;

                case PropertyBreakdownColumns.Total:
                case PropertyBreakdownColumns.Company:
                case PropertyBreakdownColumns.ResourceType:
                case PropertyBreakdownColumns.Status:
                default:
                    break;
            }
            PropertyBreakdownDisplayList = tempList;
        }
        public void SortDataByColumnAscending(PropertyBreakdownColumns pbc)
        {
            List<PropertyBreakdown> tempList = new List<PropertyBreakdown>();
            switch (pbc)
            {
                case PropertyBreakdownColumns.Production:
                    tempList = PropertyBreakdownDisplayList.OrderBy(p => p.Production).ToList();
                    break;
                case PropertyBreakdownColumns.PPE:
                    tempList = PropertyBreakdownDisplayList.OrderBy(p => p.PPE).ToList();
                    break;
                case PropertyBreakdownColumns.Revenue:
                    tempList = PropertyBreakdownDisplayList.OrderBy(p => p.Revenue).ToList();
                    break;
                case PropertyBreakdownColumns.OPEX:
                    tempList = PropertyBreakdownDisplayList.OrderBy(p => p.OPEX).ToList();
                    break;
                case PropertyBreakdownColumns.GrossProfitD:
                    tempList = PropertyBreakdownDisplayList.OrderBy(p => p.GrossProfitD).ToList();
                    break;
                case PropertyBreakdownColumns.GrossProfitP:
                    tempList = PropertyBreakdownDisplayList.OrderBy(p => p.GrossProfitP).ToList();
                    break;

                case PropertyBreakdownColumns.Total:
                case PropertyBreakdownColumns.Company:
                case PropertyBreakdownColumns.ResourceType:
                case PropertyBreakdownColumns.Status:
                default:
                    break;
            }
            PropertyBreakdownDisplayList = tempList;
        }
    }
}
