using System.Collections.Generic;
using System.Linq;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main
{
    public class SQList : List<SQ>
    {
        public SQList(Game game) {; }
        public SQList()
        {
            InitDictionary();
        }
        void InitDictionary()
        {
            using (DBContext db = new DBContext())
            {
                var sqList = db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                foreach (SQ sq in sqList) { this.Add(sq); }
            }
        }
        public SQ this[int row, int col]
        {
            get => this.Where(s => s.Row == row)
                        .Where(s => s.Col == col)
                        .FirstOrDefault();
            set => this[row, col] = value;
        }
    }
}
