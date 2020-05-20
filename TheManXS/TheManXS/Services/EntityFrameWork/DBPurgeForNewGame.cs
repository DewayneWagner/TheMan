using System.Linq;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Services.EntityFrameWork
{
    class DBPurgeForNewGame
    {
        public DBPurgeForNewGame() { RemoveDataFromCurrentSavedGameSlot(); }

        void RemoveDataFromCurrentSavedGameSlot()
        {
            using (DBContext db = new DBContext())
            {
                // commodity pricing
                var cList = db.Commodity.Where(c => c.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if (cList.Count > 0) { db.RemoveRange(cList); }

                // financial values list
                var fList = db.Commodity.Where(f => f.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if (fList.Count > 0) { db.RemoveRange(fList); }

                // sq data - (done somewhere else?)
                var sqList = db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if (sqList.Count > 0) { db.RemoveRange(sqList); }

                // formation
                var formationList = db.Formation.Where(f => f.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                if (formationList.Count > 0) { db.RemoveRange(formationList); }

                db.SaveChanges();
            }
        }
    }
}
