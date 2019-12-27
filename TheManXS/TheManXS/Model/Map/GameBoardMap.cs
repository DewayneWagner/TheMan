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
using EFCore.BulkExtensions;

namespace TheManXS.Model.Map
{
    public class GameBoardMap
    {
        public SQMapConstructArray SQMap { get; set; }

        public GameBoardMap() { }
        public GameBoardMap(bool isNewGame)
        {
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
            SQMap = new SQMapConstructArray();// to create the squares themselves as per dimensions
            new Terrain(true, SQMap); // build new terrain
            new ResourcePools(true, SQMap); // build resource pools
            new City(SQMap); // build new city
            new Infrastructure(true,SQMap);
            //new StartSQ(true); // set starting SQ's for each player

            //new WriteMapToDB(SQMap.GetListOfSQs());

            AddNewListOfSQToDB();
        }

        private void AddNewListOfSQToDB()
        {
            using (DBContext db = new DBContext())
            {
                db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).BatchDelete();
                db.SaveChanges();

                var sqList = SQMap.GetListOfSQs();

                db.BulkInsert<SQ>(sqList);
                db.SaveChanges();
            }

            //using (DBContext db = new DBContext())
            //{                
            //    var _existingSQs = db.SQ.ToList();
            //    if(_existingSQs != null) { db.SQ.RemoveRange(_existingSQs); }
                                
            //    List<SQ> sList = SQMap.GetListOfSQs();
            //    db.SQ.AddRange(sList);
            //    db.SaveChanges();
            //}
        }
    }
}
