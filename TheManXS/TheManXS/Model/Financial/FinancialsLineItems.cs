using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.ViewModel.FinancialVM.Financials;

namespace TheManXS.Model.Financial
{
    public class FinancialsLineItems
    {
        public FinancialsVM.LineItemType LineItemType { get; set; }
        public string FinalText { get; set; }
        public FinancialsVM.FormatTypes FormatType { get; set; }
        public string[] ValuesArray { get; set; } = new string[FinancialsVM.QDATACOLUMNS];
    }
}
