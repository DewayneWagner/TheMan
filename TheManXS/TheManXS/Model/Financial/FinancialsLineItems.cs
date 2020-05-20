using TheManXS.ViewModel.FinancialVM.Financials;
using TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;

namespace TheManXS.Model.Financial
{
    public class FinancialsLineItems
    {
        public LineItemType LineItemType { get; set; }
        public string FinalText { get; set; }
        public DataRowType DataRowType { get; set; }
        public string[] ValuesArray { get; set; } = new string[FinancialsVM.QDATACOLUMNS];
    }
}
