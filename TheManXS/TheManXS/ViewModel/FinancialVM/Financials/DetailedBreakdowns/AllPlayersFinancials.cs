using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    class AllPlayersFinancials : DetailedBreakdown
    {
        Game _game;
        private bool _filterRowNeeded;
        private FinancialsLineItems[] _financialsLineItemsArray;
        private int _numberOfColumns;

        public AllPlayersFinancials(Game game, FinancialsLineItems[] financialsLineItemsArray)
        {
            _game = game;
            _financialsLineItemsArray = financialsLineItemsArray;
            _filterRowNeeded = false;
            _numberOfColumns = QC.PlayerQ;
        }
        public override List<DataRowList> ListOfDataRowList
        {
            get
            {
                List<DataRowList> dataRowListOfLists = new List<DataRowList>();

                foreach (FinancialsLineItems financials in _financialsLineItemsArray)
                {

                    dataRowListOfLists.Add(new DataRowList(_game, financials.DataRowType,
                        getDataRowList(financials)));
                }

                return dataRowListOfLists;

                List<string> getDataRowList(FinancialsLineItems f)
                {
                    List<string> dataRowList = new List<string>();
                    dataRowList.Add(f.FinalText);
                    int q = f.ValuesArray.GetLength(0);
                    
                    for (int i = 0; i < q; i++)
                    {
                        dataRowList.Add(f.ValuesArray[i]);
                    }

                    return dataRowList;
                }
            }
        }

        public override DetailedBreakdownGrid DetailedBreakdownGrid => new DetailedBreakdownGrid(_filterRowNeeded, ListOfDataRowList);        
    }
}
