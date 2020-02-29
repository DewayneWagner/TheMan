using System;
using System.Collections.Generic;
using System.Text;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class FinancialsLineItems
    {
        public LineItemType LineItemType { get; set; }
        public string FinalText { get; set; }
        public FormatTypes FormatType { get; set; }
        public string[] ValuesArray { get; set; } = new string[FinancialsVM.QDATACOLUMNS];
    }
}
