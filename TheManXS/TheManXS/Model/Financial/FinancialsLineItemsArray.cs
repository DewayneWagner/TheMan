using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
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
        }
        public FinancialsLineItems[] GetArrayOfFinancialsLineItems() => _lineItemsArray;
        void InitListWithHeadings()
        {
            _lineItemsArray[(int)LineItemType.CompanyNamesOrTurnNumber] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CompanyNamesOrTurnNumber,
                    FinalText = GetFinalTextForCompanyOrTurn(),
                    FormatType = FormatTypes.CompanyNameColHeading,
                });

            _lineItemsArray[(int)LineItemType.BalanceSheets] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.BalanceSheets,
                    FinalText = "Balance Sheet",
                    FormatType = FormatTypes.MainHeading,
                });

            _lineItemsArray[(int)LineItemType.Assets] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Assets,
                    FinalText = "Assets",
                    FormatType = FormatTypes.SubHeading,
                });

            _lineItemsArray[(int)LineItemType.Cash] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Cash,
                    FinalText = "Cash",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.PPE] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.PPE,
                    FinalText = "PPE",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.TotalAssets] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.TotalAssets,
                    FinalText = "Total Assets",
                    FormatType = FormatTypes.Totals,
                });

            _lineItemsArray[(int)LineItemType.Liabilities] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Liabilities,
                    FinalText = "Total Liabilities",
                    FormatType = FormatTypes.SubHeading,
                });

            _lineItemsArray[(int)LineItemType.LongTermDebt] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.LongTermDebt,
                    FinalText = "Long Term Debt",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.TotalCapital] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.TotalCapital,
                    FinalText = "Total Capital",
                    FormatType = FormatTypes.Totals,
                });

            _lineItemsArray[(int)LineItemType.CashFlowStateMent] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CashFlowStateMent,
                    FinalText = "Cash Flow Statement",
                    FormatType = FormatTypes.MainHeading,
                });

            _lineItemsArray[(int)LineItemType.Revenue] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Revenue,
                    FinalText = "Revenue",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.Expenses] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.Expenses,
                    FinalText = "Expenses",
                    FormatType = FormatTypes.SubHeading
                });

            _lineItemsArray[(int)LineItemType.OPEX] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.OPEX,
                    FinalText = "OPEX Cost",
                    FormatType = FormatTypes.LineItem
                });

            _lineItemsArray[(int)LineItemType.TheManCut] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.TheManCut,
                    FinalText = "The Man Cost",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.DebtPayment] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.DebtPayment,
                    FinalText = "Debt Payment",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.GrossProfitD] = 
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.GrossProfitD,
                    FinalText = "Gross Profit ($)",
                    FormatType = FormatTypes.Totals,
                });

            _lineItemsArray[(int)LineItemType.GrossProfitP] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.GrossProfitP,
                    FinalText = "Gross Profit (%)",
                    FormatType = FormatTypes.Totals
                });

            _lineItemsArray[(int)LineItemType.CAPEXCosts] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.CAPEXCosts,
                    FinalText = "CAPEX Costs",
                    FormatType = FormatTypes.LineItem,
                });

            _lineItemsArray[(int)LineItemType.NetProfitD] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.NetProfitD,
                    FinalText = "Net Profit ($)",
                    FormatType = FormatTypes.Totals,
                });

            _lineItemsArray[(int)LineItemType.NetProfitP] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.NetProfitP,
                    FinalText = "Net Profit (%)",
                    FormatType = FormatTypes.Totals,
                });

            _lineItemsArray[(int)LineItemType.InterestExpense] =
                (new FinancialsLineItems()
                {
                    LineItemType = LineItemType.InterestExpense,
                    FinalText = "Interest Expense",
                    FormatType = FormatTypes.LineItem,
                });
        }
        string GetFinalTextForCompanyOrTurn() => _dataPanelType == DataPanelType.AllPlayers ? "Company Name" : "Quarter";
    }
}
