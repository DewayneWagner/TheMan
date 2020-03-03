using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;
using Xamarin.Forms;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class FinancialsGrid : Grid
    {
        Game _game;
        DataPanelType _dataPanelType;
        private FinancialsLineItems[] _financialsLineItemsArray;
        int _numberOfColumns;
        private List<string> _quarterColumnHeadings;
        public FinancialsGrid(Game game, DataPanelType dataPanelType,FinancialsLineItems[] financialsLineItemsArray)
        {
            _game = game;
            _dataPanelType = dataPanelType;
            _financialsLineItemsArray = financialsLineItemsArray;
            CompressedLayout.SetIsHeadless(this, true);

            _game.FinancialValuesList.AssignValuesToFinancialLineItemsArrays(_financialsLineItemsArray);
                        
            SetNumberOfColummns();
            if (_dataPanelType == DataPanelType.SinglePlayer) { LoadListOfQuarterColumnHeadings(); }
            InitPropertiesOfGrid();
            InitRows();
            InitColumns();
            AddData();
        }

        void LoadListOfQuarterColumnHeadings() =>_quarterColumnHeadings = new QuarterCalc(_game).GetLastXQuarters(_numberOfColumns);

        void SetNumberOfColummns()
        {
            if (_dataPanelType == DataPanelType.AllPlayers) { _numberOfColumns = FinancialsVM.QDATACOLUMNS; }
            else if(_game.TurnNumber > FinancialsVM.QDATACOLUMNS) { _numberOfColumns = FinancialsVM.QDATACOLUMNS; }
            else { _numberOfColumns = _game.TurnNumber; }
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
            for (int i = 0; i < (_numberOfColumns + 1); i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        void AddData()
        {
            foreach (FinancialsLineItems f in _financialsLineItemsArray)
            {
                if(f.FormatType == FormatTypes.CompanyNameColHeading)
                {
                    if(_dataPanelType == DataPanelType.AllPlayers) { addCompanyNames(); }
                    else if(_dataPanelType == DataPanelType.SinglePlayer) { addQuarterNames(); }
                }
                
                else if (f.FormatType == FormatTypes.MainHeading) { addMainHeadings(); }                
                else { addAllOtherLines(); }

                void addCompanyNames()
                {
                    for (int c = 1; c <= _numberOfColumns; c++)
                    {
                        RowTypeLabel companyHeadingLabel = new RowTypeLabel(f);
                        companyHeadingLabel.Text = _game.PlayerList[(c - 1)].Name;
                        Children.Add(companyHeadingLabel, c, (int)f.LineItemType);
                    }
                }
                void addQuarterNames()
                {
                    for (int c = 1; c <= _numberOfColumns; c++)
                    {
                        RowTypeLabel quarterHeadingLabel = new RowTypeLabel(f);
                        quarterHeadingLabel.Text = _quarterColumnHeadings[(c - 1)];
                        Children.Add(quarterHeadingLabel, c, (int)f.LineItemType);
                    }
                }
                void addMainHeadings()
                {
                    RowTypeLabel mainHeadingLabel = new RowTypeLabel(f);
                    mainHeadingLabel.Text = f.FinalText;
                    Children.Add(mainHeadingLabel, 0, (int)f.LineItemType);
                    Grid.SetColumnSpan(mainHeadingLabel, _numberOfColumns + 1);
                    mainHeadingLabel.VerticalOptions = LayoutOptions.Center;
                }
                void addAllOtherLines()
                {
                    RowTypeLabel rowHeadingLabel = new RowTypeLabel(f);
                    rowHeadingLabel.HorizontalTextAlignment = TextAlignment.Start;
                    rowHeadingLabel.Text = f.FinalText;
                    Children.Add(rowHeadingLabel, 0, (int)f.LineItemType);

                    for (int i = 0; i < _numberOfColumns; i++)
                    {
                        RowTypeLabel l = new RowTypeLabel(f);
                        l.Text = f.ValuesArray[i];
                        Children.Add(l, (i + 1), (int)f.LineItemType);
                    }
                }
            }
        }
    }
}
