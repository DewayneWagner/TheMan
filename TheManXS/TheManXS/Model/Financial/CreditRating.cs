using System.Collections.Generic;
using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Financial
{
    class CreditRating
    {
        FinancialValues _finacialValues;
        Player _player;
        Game _game;
        public CreditRating(FinancialValues financialValues, Player player, Game game)
        {
            _game = game;
            _finacialValues = financialValues;
            _player = player;
            SetRating();
        }

        public CreditRatings Rating { get; set; }
        public double InterestRate { get; set; }

        private void SetRating()
        {
            double debtToCashFlowRatio = _finacialValues.Debt / _finacialValues.GrossProfitD;
            List<double> interestRates = new List<double>();

            setInterestRateListFromDB();

            CreditRatings calculatedRating;
            setCreditRatingByCalculation();
            setFinalCreditRating();

            InterestRate = interestRates[(int)Rating];

            void setFinalCreditRating() // limit change to 1 place / turn
            {
                int currentRatingIndex = (int)_player.CreditRating;
                if ((int)_player.CreditRating < (int)calculatedRating) { Rating = ((CreditRatings)(currentRatingIndex + 1)); }
                else if ((int)_player.CreditRating > (int)calculatedRating) { Rating = ((CreditRatings)(currentRatingIndex - 1)); }
                else Rating = calculatedRating;
            }

            void setCreditRatingByCalculation() // calculate what Credit Rating should be
            {
                if (debtToCashFlowRatio < 0.1) { calculatedRating = CreditRatings.AAA; }
                else if (debtToCashFlowRatio < 0.15) { calculatedRating = CreditRatings.AA; }
                else if (debtToCashFlowRatio < 0.2) { calculatedRating = CreditRatings.A; }
                else if (debtToCashFlowRatio < 0.25) { calculatedRating = CreditRatings.B; }
                else if (debtToCashFlowRatio < 0.3) { calculatedRating = CreditRatings.C; }
                else { calculatedRating = CreditRatings.Junk; }
            }

            void setInterestRateListFromDB()
            {
                for (int i = 0; i < (int)CreditRatings.Total; i++)
                {
                    interestRates.Add(_game.ParameterConstantList.GetConstant(AllConstantParameters.PrimeRateAdderBasedOnCreditRating, i));
                }
            }
        }
    }
}
