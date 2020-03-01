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
        public FinancialsGrid(Game game, DataPanelType dataPanelType,FinancialsLineItems[] financialsLineItemsArray)
        {
            _game = game;
            _dataPanelType = dataPanelType;

            _financialsLineItemsArray = financialsLineItemsArray;
            new CalculatedFinancialValuesList(_game,_financialsLineItemsArray);
            InitPropertiesOfGrid();
            InitRows();
            InitColumns();
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
            for (int i = 0; i < _financialsLineItemsArray.Length; i++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }
        }

        void InitColumns()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < (FinancialsVM.QDATACOLUMNS + 1); i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        void AddData()
        {
            List<string> _quartersForTesting = new List<string>() { "1901-Q1", "1900-Q4", "1900-Q3", "1900-Q2", "1900-Q1" };

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
                    for (int c = 1; c <= FinancialsVM.QDATACOLUMNS; c++)
                    {
                        RowTypeLabel companyHeadingLabel = new RowTypeLabel(f);
                        companyHeadingLabel.Text = _game.PlayerList[(c - 1)].Name;
                        Children.Add(companyHeadingLabel, c, (int)f.LineItemType);
                    }
                }
                void addQuarterNames()
                {
                    for (int c = 1; c < FinancialsVM.QDATACOLUMNS; c++)
                    {
                        RowTypeLabel quarterHeadingLabel = new RowTypeLabel(f);
                        quarterHeadingLabel.Text = _quartersForTesting[(c - 1)];
                        Children.Add(quarterHeadingLabel, c, (int)f.LineItemType);
                    }
                }
                void addMainHeadings()
                {
                    RowTypeLabel mainHeadingLabel = new RowTypeLabel(f);
                    mainHeadingLabel.Text = f.FinalText;
                    Children.Add(mainHeadingLabel, 0, (int)f.LineItemType);
                    Grid.SetColumnSpan(mainHeadingLabel, FinancialsVM.QDATACOLUMNS + 1);
                    mainHeadingLabel.VerticalOptions = LayoutOptions.Center;
                }
                void addAllOtherLines()
                {
                    RowTypeLabel rowHeadingLabel = new RowTypeLabel(f);
                    rowHeadingLabel.HorizontalTextAlignment = TextAlignment.Start;
                    rowHeadingLabel.Text = f.FinalText;
                    Children.Add(rowHeadingLabel, 0, (int)f.LineItemType);

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
}
