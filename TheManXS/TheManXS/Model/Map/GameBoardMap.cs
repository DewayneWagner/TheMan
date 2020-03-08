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

namespace TheManXS.Model.Map
{
    public class GameBoardMap
    {
        private System.Random rnd = new System.Random();
        public SQMapConstructArray SQMap { get; set; }
        Game _game;
        List<SQ> _listOfAllSQs;

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
            SQMap = new SQMapConstructArray();// to create the squares themselves as per dimensions
            new Terrain(true, SQMap); // build new terrain
            new ResourcePools(true, SQMap); // build resource pools
            new City(SQMap); // build new city

            InitSQsForTesting();
            _listOfAllSQs = SQMap.GetListOfSQs();
            AddNewListOfSQToDB();
            LoadSQDictionaryForGame();

            new Infrastructure(true,SQMap,_game);
        }

        void LoadSQDictionaryForGame()
        {
            _game.SquareDictionary = new Dictionary<int, SQ>();

            foreach (SQ sq in _listOfAllSQs)
            {
                _game.SquareDictionary.Add(sq.Key, sq);
            }
        }

        private void AddNewListOfSQToDB()
        {
            using (DBContext db = new DBContext())
            {
                var oldList = db.SQ.Where(s => s.SavedGameSlot == QC.CurrentSavedGameSlot).ToList();
                db.SQ.RemoveRange(oldList);
                db.SaveChanges();

                db.SQ.AddRange(_listOfAllSQs);
                db.SaveChanges();
            }
        }
        private void InitSQsForTesting()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    SQMap[row, col].ResourceType = ResourceTypeE.Oil;
                    SQMap[row, col].Production = rnd.Next(5, 20);
                    SQMap[row, col].OPEXPerUnit = rnd.Next(15, 35);
                    SQMap[row, col].FormationID = 50;
                    SQMap[row, col].Transport = rnd.Next(5, 20);
                }
            }
            foreach (Player player in _game.PlayerList)
            {
                int productingSQsForPlayer = 0;
                int loopCounter = 0;
                do
                {
                    SQ sq = SQMap[rnd.Next(0, QC.RowQ), rnd.Next(0, QC.ColQ)];
                    loopCounter++;

                    if (sq.OwnerNumber == QC.PlayerIndexTheMan 
                        && sq.Status == ST.Nada 
                        && sq.ResourceType != RT.RealEstate
                        && (int)sq.ResourceType < (int)RT.Nada)
                    {
                        sq.OwnerNumber = player.Number;
                        sq.Status = ST.Producing;
                        productingSQsForPlayer++;
                    }

                } while (loopCounter < 50 && productingSQsForPlayer < 3);
            }
        }
    }
}
