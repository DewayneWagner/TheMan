using System;
using System.Collections.Generic;
using System.Text;
using static TheManXS.Model.Settings.SettingsMaster;

namespace TheManXS.Model.Financial
{
    class CreditRating
    {
        FinancialValues _finacialValues;
        public CreditRating(FinancialValues financialValues)
        {
            _finacialValues = financialValues;
            SetRating();
        }
        public CreditRatingsE Rating { get; set; }
        private void SetRating()
        {

        }
    }
}
