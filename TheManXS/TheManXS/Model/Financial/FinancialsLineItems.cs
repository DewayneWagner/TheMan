using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.FinancialVM.Financials;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;

namespace TheManXS.Model.Financial
{
    public class FinancialsLineItems
    {
        public LineItemType LineItemType { get; set; }
        public string FinalText { get; set; }
        public FormatTypes FormatType { get; set; }
        public string[] ValuesArray { get; set; } = new string[FinancialsVM.QDATACOLUMNS];
    }
}
