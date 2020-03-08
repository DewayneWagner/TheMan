using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;
using CR = TheManXS.Model.Settings.SettingsMaster.CreditRatingsE;

namespace TheManXS.Model.Financial.Debt
{
    public enum Terms { T1 = 5, T2 = 10, T3 = 15, T4 = 20, T5 = 25, Total }
    public class InterestRate
    {
        private bool _listsHaveBeenLoaded;
        private static List<double> _primeAdderForTerm;
        private static List<double> _primeAdderForCreditRating;
        public InterestRate()
        {
            InitListOfPrimeAdderBasedOnCreditRating();

        }
        private void InitListOfPrimeAdderBasedOnCreditRating()
        {
            using (DBContext db = new DBContext())
            {
                _primeAdderForCreditRating = db.Settings
                    .Where(s => s.PrimaryIndex == Settings.SettingsMaster.AS.IntRateCR)
                    .Select(s => s.LBOrConstant)
                    .ToList();
            }
        }
        private void InitListOfPrimAddersBasedOnTerm()
        {

        }
    }
}
