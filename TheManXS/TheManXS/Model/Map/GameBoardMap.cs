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

namespace TheManXS.Model.Map
{
    public class GameBoardMap
    {
        private System.Random rnd = new System.Random();
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

            InitSQsForTesting();

            AddNewListOfSQToDB();

            new Infrastructure(true,SQMap);
        }

        private void AddNewListOfSQToDB()
        {
            using (DBContext db = new DBContext())
            {
                var oldList = db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                db.SQ.RemoveRange(oldList);
                db.SaveChanges();

                var sqList = SQMap.GetListOfSQs();
                db.SQ.AddRange(sqList);
                db.SaveChanges();
            }
        }
        private void InitSQsForTesting()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    SQMap[row, col].ResourceType = Settings.SettingsMaster.ResourceTypeE.Oil;
                    SQMap[row, col].Production = rnd.Next(5, 20);
                    SQMap[row, col].OPEXPerUnit = rnd.Next(15, 35);
                    SQMap[row, col].FormationID = 50;
                    SQMap[row, col].Transport = rnd.Next(5, 20);
                }
            }
        }
    }
}
