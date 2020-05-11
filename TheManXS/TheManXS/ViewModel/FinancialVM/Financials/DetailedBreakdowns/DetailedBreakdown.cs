using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    class DetailedBreakdown
    {
        bool _filterRowNeeded;
        bool _actionRowNeeded;
        private Game _game;
        private FinancialsLineItems[] _financialsLineItemsArray;

        public DetailedBreakdown(Game game, FinancialsLineItems[] financialsLineItemsArray, 
            bool filterRowNeeded, bool actionButtonsNeeded)
        {
            _game = game;
            _financialsLineItemsArray = financialsLineItemsArray;
            _filterRowNeeded = filterRowNeeded;
            _actionRowNeeded = actionButtonsNeeded;
        }

        public List<DataRowList> ListOfDataRowList 
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

                    for (int i = 0; i < f.ValuesArray.GetLength(0); i++)
                    {
                        dataRowList.Add(f.ValuesArray[i]);
                    }

                    return dataRowList;
                }
            } 
        }
        public DetailedBreakdownGrid DetailedBreakdownGrid => new DetailedBreakdownGrid(_filterRowNeeded, ListOfDataRowList);

    }
}
