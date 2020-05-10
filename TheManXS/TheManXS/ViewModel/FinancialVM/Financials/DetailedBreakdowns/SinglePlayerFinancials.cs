using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    class SinglePlayerFinancials : DetailedBreakdown
    {
        Game _game;
        private bool _filterRowNeeded;
        private FinancialsLineItems[] _financialsLineItemsArray;
        
        public SinglePlayerFinancials(Game game, FinancialsLineItems[] financialsLineItemsArray)
        {
            _game = game;
            _financialsLineItemsArray = financialsLineItemsArray;
            _filterRowNeeded = false;
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

                    for (int i = 0; i < f.ValuesArray.GetLength(0); i++)
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
