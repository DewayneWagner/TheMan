using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Financial.Debt;
using TheManXS.Model.Main;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    class DetailedBreakdown
    {
        bool _filterRowNeeded;
        bool _actionRowNeeded;
        private Game _game;

        public DetailedBreakdown(Game game, FinancialsLineItems[] financialsLineItemsArray, 
            bool filterRowNeeded, bool actionButtonsNeeded)
        {
            _game = game;
            _filterRowNeeded = filterRowNeeded;
            _actionRowNeeded = actionButtonsNeeded;
            ListOfDataRowList = GetDataRowList(financialsLineItemsArray);
        }

        public DetailedBreakdown(Game game, DataPanelType dataPanelType)
        {
            _game = game;
            if(dataPanelType == DataPanelType.Loans) { GetDataRowListForLoansList(); }
        }

        public List<DataRowList> ListOfDataRowList { get; set; }
        public DetailedBreakdownGrid DetailedBreakdownGrid => new DetailedBreakdownGrid(_filterRowNeeded, ListOfDataRowList);
        private List<DataRowList> GetDataRowList(FinancialsLineItems[] financialsLineItemsArray)
        {
            List<DataRowList> dataRowListOfLists = new List<DataRowList>();

            foreach (FinancialsLineItems financials in financialsLineItemsArray)
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
        private List<DataRowList> GetDataRowListForLoansList()
        {
            List<DataRowList> dataRowListOfLists = new List<DataRowList>();
            List<Loan> loansForActivePlayer = getListOfLoansForActivePlayer();
            addHeadings();

            foreach (Loan loan in loansForActivePlayer)
            {
                List<string> dataValues = getListOfDataValues(loan);
                dataRowListOfLists.Add(new DataRowList(_game, DataRowType.Data, dataValues));
            }

            return dataRowListOfLists;

            List<Loan> getListOfLoansForActivePlayer()
            {
                return _game.LoanList.Where(l => l.PlayerNumber == _game.ActivePlayer.Number).ToList();
            }
            void addHeadings()
            {
                dataRowListOfLists.Add(new DataRowList(_game, DataRowType.MainHeading, _game.LoanList.DisplayHeadingsList));
            }
            List<string> getListOfDataValues(Loan loan)
            {
                List<string> dataRow = new List<string>();

                dataRow.Add(Convert.ToString(loan.ID));
                dataRow.Add(Convert.ToString(loan.Term));
                dataRow.Add(loan.InterestRate.ToString("p0"));
                dataRow.Add(loan.StartingBalance.ToString("c0"));
                dataRow.Add(loan.PrincipalPaymentPerTurn.ToString("c0"));
                dataRow.Add(loan.InterestPaymentPerTurn.ToString("c0"));
                dataRow.Add(Convert.ToString(loan.TurnsRemaining));

                return dataRow;
            }
        }
    }
}
