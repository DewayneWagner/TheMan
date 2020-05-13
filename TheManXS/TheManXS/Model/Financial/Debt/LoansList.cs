using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Financial.Debt
{
    public class LoansList : List<Loan>
    {
        public LoansList() { InitLoansList(); }
        private void InitLoansList()
        {
            using (DBContext db = new DBContext())
            {
                var loansList = db.Loans.Where(l => l.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                foreach (Loan loan in loansList)
                {
                    this.Add(loan);
                }
            }
        }
    }
}
