namespace TheManXS.Model.Financial.Ratios
{
    class FinancialRatios
    {
        FinancialValues f;

        double _totalExpenses;
        public FinancialRatios(FinancialValues financialValues)
        {
            f = financialValues;
            _totalExpenses = f.DebtPayment + f.InterestExpense +
                f.TheManCut + f.TotalOPEX;
        }
        public double CAPEXToCash => f.CAPEXThisTurn / f.Revenue;
        public double CashRatio => f.Cash / f.Debt;
        public double CurrentRatio => f.TotalAssets / f.Debt;
        public double DefensiveIntervalRatio => f.TotalAssets / _totalExpenses;
        public double OperatingCashFlow => f.Revenue / f.Debt;
        public double QuickRatio => f.Cash / f.Debt;
        public double TimeInterestEarned => f.GrossProfitD / f.InterestExpense;
        public double AssetCoverageRatio => f.TotalAssets / (f.DebtPayment + f.InterestExpense);
        public double CashCoverageRatio => f.Cash / (f.DebtPayment + f.InterestExpense);
        public double CashFlowToDebtRatio => f.Revenue / f.Debt;
        public double DebtServiceCoverageRatio => f.GrossProfitD / (f.DebtPayment + f.InterestExpense);
        public double DebtToAssetsRatio => f.Debt / f.TotalAssets;
        public double InterestCoverageRatio => f.GrossProfitD / f.InterestExpense;
        public double GrossMarginRatio => (f.Revenue - f.TotalOPEX) / f.Revenue;
        public double OperatingMargin => f.GrossProfitD / f.Revenue;
        public double ReturnOnAssets => f.NetProfitD / f.TotalAssets;
        public double ReturnOnEquity => f.NetProfitD / f.TotalCapital;
        public double AssetTurnoverRatio => f.Revenue / f.TotalAssets;
    }
}
