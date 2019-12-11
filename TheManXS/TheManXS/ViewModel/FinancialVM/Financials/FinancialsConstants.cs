using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.ViewModel.FinancialVM.Financials
{
    public class FinancialsConstants
    {       
        public enum Detail { BalanceSheet, CashFlow, }
        public enum DetailType { PlayerTeam, AllPlayers, BalanceSheet, CashFlow, ByResource, Total }
        public enum SubHeadings { Assets, TotalAssets, Liabilities, TotalCapital, Revenue, Expenses, Total }
        public enum AssetDetailTypes { PerResource, PerProperty, PerFormation, PerStatus, Total }
        public enum CashFlowDetail { PerResource, PerProperty, PerFormation, Total }
        public enum RevenueDetailSelector { }
        public FinancialsConstants()
        {
            
        }

        private List<string> GetDetailTypeSelectorList()
        {
            List<string> detailTypes = new List<string>();
            for (int i = 0; i < (int)DetailType.Total; i++)
            {
                detailTypes.Add(Convert.ToString((DetailType)i));
            }
            return detailTypes;
        }

    }
}
