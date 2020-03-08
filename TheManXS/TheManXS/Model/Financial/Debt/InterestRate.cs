using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;
using CR = TheManXS.Model.Settings.SettingsMaster.CreditRatingsE;

namespace TheManXS.Model.Financial.Debt
{
    
    public class InterestRate
    {
        private bool _listsHaveBeenLoaded;
        private static List<double> _primeAdderForTerm;
        private static List<double> _primeAdderForCreditRating;
        public InterestRate()
        {
            InitListOfPrimeAdderBasedOnCreditRating();
            InitListOfPrimAddersBasedOnTerm();
        }
        private void InitListOfPrimeAdderBasedOnCreditRating()
        {
            using (DBContext db = new DBContext())
            {
                _primeAdderForCreditRating = db.Settings
                    .Where(s => s.PrimaryIndex == Settings.SettingsMaster.AS.PrimeRateAdderBasedOnCreditRating)
                    .Select(s => s.LBOrConstant)
                    .ToList();
            }
        }
        private void InitListOfPrimAddersBasedOnTerm()
        {
            using (DBContext db = new DBContext())
            {
                _primeAdderForTerm = db.Settings
                    .Where(s => s.PrimaryIndex == Settings.SettingsMaster.AS.PrimeRateAdderBasedOnTermLength)
                    .Select(s => s.LBOrConstant)
                    .ToList();
            }
        }
    }
}
