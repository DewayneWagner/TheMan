using System;
using System.Collections.Generic;
using System.Text;

namespace TheManXS.Model.Financial
{
    public class Cash
    {
        public Cash() { }
        public Cash(double revenue, double opex, double transport)
        {
            Revenue = revenue;
            OPEX = opex;
            Transport = transport;

            ProfitDollar = Revenue - OPEX - Transport;
            ProfitPercent = (Revenue != 0) ? (ProfitDollar / Revenue) : 0;
        }
        public double Revenue { get; set; }
        public double OPEX { get; set; }
        public double Transport { get; set; }
        public double ProfitDollar { get; set; }
        public double ProfitPercent { get; set; }
        public int UnitProduction { get; set; }
        public double UnitNexActionCost { get; set; }
    }
}
