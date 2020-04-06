using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;

namespace TheManXS.Model.Main
{
    public class DeleteGameAction
    {
        private int _savedGameSlot;
        public DeleteGameAction(int savedGameSlot)
        {
            _savedGameSlot = savedGameSlot;
            DeleteGame();
        }
        private void DeleteGame()
        {
            using (DBContext db = new DBContext())
            {
                var gList = db.GameSpecificParameters.Where(g => g.Slot == _savedGameSlot).FirstOrDefault();
                db.GameSpecificParameters.RemoveRange(gList);

                var sList = db.SQ.Where(s => s.SavedGameSlot == _savedGameSlot).ToList();
                db.SQ.RemoveRange(sList);

                var cList = db.Commodity.Where(c => c.SavedGameSlot == _savedGameSlot).ToList();
                db.Commodity.RemoveRange(cList);

                var fList = db.FinancialValues.Where(f => f.SavedGameSlot == _savedGameSlot).ToList();
                db.FinancialValues.RemoveRange(fList);

                var formList = db.Formation.Where(f => f.SavedGameSlot == _savedGameSlot).ToList();
                db.Formation.RemoveRange(formList);

                var loansList = db.Loans.Where(l => l.SavedGameSlot == _savedGameSlot).ToList();
                db.Loans.RemoveRange(loansList);

                var playerList = db.Player.Where(p => p.SavedGameSlot == _savedGameSlot).ToList();
                db.Player.RemoveRange(playerList);

                var resourceProductionList = db.ResourceProduction.Where(r => r.SavedGameSlot == _savedGameSlot).ToList();
                db.ResourceProduction.RemoveRange(resourceProductionList);

                var sqInfrastructureList = db.SQInfrastructure.Where(s => s.SavedGameSlot == _savedGameSlot).ToList();
                db.SQInfrastructure.RemoveRange(sqInfrastructureList);
            }
        }
    }
}
