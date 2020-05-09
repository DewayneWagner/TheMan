using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.ViewModel.FinancialVM.Financials.DetailedBreakdowns;
using static TheManXS.ViewModel.FinancialVM.Financials.FinancialsVM;

namespace TheManXS.Model.Financial
{
    public class FinancialsLineItemsArray
    {
        Game _game;
        DataPanelType _dataPanelType;
        FinancialsLineItems[] _lineItemsArray;
        public FinancialsLineItemsArray(Game game, DataPanelType dataPanelType)
        {
            _game = game;
            _dataPanelType = dataPanelType;
            _lineItemsArray = new FinancialsLineItems[(int)LineItemType.Total];
            InitListWithHeadings();
            _game.FinancialValuesList.AssignValuesToFinancialLineItemsArrays(_lineItemsArray);
        }
        public FinancialsLineItems[] GetArrayOfFinancialsLineItems() => _lineItemsArray;
        void InitListWithHeadings()
        {
            _lineItemsArray[(int)LineItemType.CompanyNamesOrTurnNumber] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CompanyNamesOrTurnNumber,
                    FinalText = GetFinalTextForCompanyOrTurn(),
                    DataRowType = DataRowType.MainHeading,
                });

            _lineItemsArray[(int)LineItemType.BalanceSheets] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.BalanceSheets,
                    FinalText = "Balance Sheet",
                    DataRowType = DataRowType.SubHeading,
                });

            _lineItemsArray[(int)LineItemType.Cash] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Cash,
                    FinalText = "Cash",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.PPE] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.PPE,
                    FinalText = "PPE",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.TotalAssets] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.TotalAssets,
                    FinalText = "Total Assets",
                    DataRowType = DataRowType.SubHeading,
                });

            _lineItemsArray[(int)LineItemType.LongTermDebt] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.LongTermDebt,
                    FinalText = "Long Term Debt",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.TotalCapital] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.TotalCapital,
                    FinalText = "Total Capital",
                    DataRowType = DataRowType.SubHeading,
                });

            _lineItemsArray[(int)LineItemType.CashFlowStateMent] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CashFlowStateMent,
                    FinalText = "Cash Flow Statement",
                    DataRowType = DataRowType.MainHeading,
                });

            _lineItemsArray[(int)LineItemType.Revenue] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Revenue,
                    FinalText = "Revenue",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.OPEX] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.OPEX,
                    FinalText = "OPEX Cost",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.TheManCut] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.TheManCut,
                    FinalText = "The Man Cost",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.DebtPayment] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.DebtPayment,
                    FinalText = "Debt Payment",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.GrossProfitD] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.GrossProfitD,
                    FinalText = "Gross Profit ($)",
                    DataRowType = DataRowType.Subtotal,
                });

            _lineItemsArray[(int)LineItemType.GrossProfitP] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.GrossProfitP,
                    FinalText = "Gross Profit (%)",
                    DataRowType = DataRowType.Subtotal,
                });

            _lineItemsArray[(int)LineItemType.CAPEXCosts] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CAPEXCosts,
                    FinalText = "CAPEX Costs",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.NetProfitD] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.NetProfitD,
                    FinalText = "Net Profit ($)",
                    DataRowType = DataRowType.Subtotal,
                });

            _lineItemsArray[(int)LineItemType.NetProfitP] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.NetProfitP,
                    FinalText = "Net Profit (%)",
                    DataRowType = DataRowType.Subtotal,
                });

            _lineItemsArray[(int)LineItemType.InterestExpense] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.InterestExpense,
                    FinalText = "Interest Expense",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.CreditRating] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CreditRating,
                    FinalText = "Credit Rating",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.InterestRate] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.InterestRate,
                    FinalText = "Interest Rate",
                    DataRowType = DataRowType.Data,
                });

            _lineItemsArray[(int)LineItemType.StockPrice] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.StockPrice,
                    FinalText = "Stock Price",
                    DataRowType = DataRowType.GrandTotal,
                });
        }
        string GetFinalTextForCompanyOrTurn() => _dataPanelType == DataPanelType.AllPlayers ? "Company Name" : "Quarter";
    }
}
