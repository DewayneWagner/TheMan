using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns
{
    abstract class DetailedBreakdown
    {
        public abstract DetailedBreakdownGrid DetailedBreakdownGrid { get; }
        public abstract List<DataRowList> ListOfDataRowList { get; }
    }
}
