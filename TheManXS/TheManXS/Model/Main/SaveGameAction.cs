using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.Services.IO;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main
{
    class SaveGameAction
    {
        private Game _game;

        public SaveGameAction(Game game)
        {
            _game = game;
            SaveAllData();
        }

        private void SaveAllData()
        {
            using (DBContext db = new DBContext())
            {
                updateGameSpecificParameters();
                updateSQDictionaryInDB();
                new SavedMap(_game).SaveMap();

                void updateGameSpecificParameters()
                {
                    var gsp = db.GameSpecificParameters.Where(g => g.Slot == QC.CurrentSavedGameSlot).FirstOrDefault();
                    gsp.Quarter = _game.Quarter;
                    gsp.TurnNumber = _game.TurnNumber;
                    gsp.ActivePlayerNumber = _game.ActivePlayer.Number;
                    gsp.LastPlayed = DateTime.Now;
                    db.GameSpecificParameters.Update(gsp);
                }
                void updateSQDictionaryInDB()
                {
                    List<SQ> sqList = new List<SQ>();
                    foreach(KeyValuePair<int,SQ> item in _game.SquareDictionary) { sqList.Add(item.Value); }
                    db.SQ.UpdateRange(sqList);
                }
            }
        }
    }
}
