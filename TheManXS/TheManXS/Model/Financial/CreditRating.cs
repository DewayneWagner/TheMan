using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using static TheManXS.Model.Settings.SettingsMaster;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;

namespace TheManXS.Model.Financial
{
    class CreditRating
    {
        FinancialValues _finacialValues;
        Player _player;
        public CreditRating(FinancialValues financialValues, Player player)
        {
            _finacialValues = financialValues;
            _player = player;
            SetRating();
        }

        public CreditRatingsE Rating { get; set; }
        public double InterestRate { get; set; }

        private void SetRating()
        {
            double debtToCashFlowRatio = _finacialValues.Debt / _finacialValues.GrossProfitD;
            List<double> interestRates = new List<double>();

            setInterestRateListFromDB();

            CreditRatingsE calculatedRating;
            setCreditRatingByCalculation();            
            setFinalCreditRating();

            InterestRate = interestRates[(int)Rating];

            void setFinalCreditRating() // limit change to 1 place / turn
            {
                int currentRatingIndex = (int)_player.CreditRating;
                if ((int)_player.CreditRating < (int)calculatedRating) { Rating = ((CreditRatingsE)(currentRatingIndex + 1)); }
                else if ((int)_player.CreditRating > (int)calculatedRating) { Rating = ((CreditRatingsE)(currentRatingIndex - 1)); }
                else Rating = calculatedRating;
            }

            void setCreditRatingByCalculation() // calculate what Credit Rating should be
            {
                if(debtToCashFlowRatio < 0.1) { calculatedRating = CreditRatingsE.AAA; }
                else if(debtToCashFlowRatio < 0.15) { calculatedRating = CreditRatingsE.AA; }
                else if(debtToCashFlowRatio < 0.2) { calculatedRating = CreditRatingsE.A; }
                else if(debtToCashFlowRatio < 0.25) { calculatedRating = CreditRatingsE.B; }
                else if(debtToCashFlowRatio < 0.3) { calculatedRating = CreditRatingsE.C; }
                else { calculatedRating = CreditRatingsE.Junk; }
            }
            
            void setInterestRateListFromDB()
            {
                List<double> interestRatesFullValue;
                using (DBContext db = new DBContext())
                {
                    interestRatesFullValue = db.Settings.Where(s => s.PrimaryIndex == AS.PrimeRateAdderBasedOnCreditRating).Select(s => s.LBOrConstant).ToList();
                }
                foreach (double i in interestRatesFullValue)
                {
                    interestRates.Add(i / 100);
                }
            }
        }

    }
}
