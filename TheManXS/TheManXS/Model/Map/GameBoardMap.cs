using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.CityStuff;
using TheManXS.Model.Main;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Map.Rocks;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;
using TheManXS.Services.EntityFrameWork;
using ST = TheManXS.Model.ParametersForGame.StatusTypeE;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using TheManXS.Model.ParametersForGame;
using Windows.UI.Xaml.Controls;

namespace TheManXS.Model.Map
{
    public class GameBoardMap
    {
        private System.Random rnd = new System.Random();
        public SQMapConstructArray SQMap { get; set; }
        Game _game;

        public GameBoardMap() { }
        public GameBoardMap(Game game, bool isNewGame)
        {
            _game = game;

            if (isNewGame)
            {
                InitNewMap();
            }
            else if (!isNewGame)
            {

            }
        }
        
        private void InitNewMap()
        {
            SQMap = new SQMapConstructArray(_game);
            new Terrain(SQMap,_game);

            // this can get hungup
            new ResourcePools(true, SQMap,_game);

            new City(SQMap);
            
            _game.SQList = SQMap.GetListOfSQs();
            AddNewListOfSQToDB();

#if DEBUG
            new DebugTesting(_game);
#endif
            new Infrastructure(true,SQMap,_game);
        }
        private void AddNewListOfSQToDB()
        {
            using (DBContext db = new DBContext())
            {
                var oldList = db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                db.SQ.RemoveRange(oldList);
                db.SaveChanges();

                db.SQ.AddRange(_game.SQList);
                db.SaveChanges();
            }
        }
    }
}
