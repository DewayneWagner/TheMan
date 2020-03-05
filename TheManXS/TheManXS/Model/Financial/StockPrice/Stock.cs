﻿using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using Xamarin.Forms;

namespace TheManXS.Model.Financial.StockPrice
{
    public class Stock
    {
        private const int numberOfOutstandingShares = 10000;
        private const double bookValueValuationRatio = 1.5;
        private double _lastStockPrice;
        private const double peRatio = 15;
        FinancialValues _financialValues;

        public Stock(FinancialValues financialValues, double lastStockPrice)
        {
            // Benjamin Graham Model - assumes companies shouldn't be valued more then 1.5x book
            // Book value = total capital
            // P/E ratio - assumes a PE of 15
            //Intrinsic Value = Square root of (15 X 1.5 (Earnings per share) X (Book Value per share))

            _lastStockPrice = lastStockPrice;
            _financialValues = financialValues;
        }

        public double Price => Math.Sqrt((peRatio * bookValueValuationRatio) * _financialValues.TotalCapital / numberOfOutstandingShares);
        public double Delta => _lastStockPrice == 0 ? 0 : (Price - _lastStockPrice) / _lastStockPrice;
    }
}
