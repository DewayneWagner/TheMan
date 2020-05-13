using System;
using System.Collections.Generic;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using System.Linq;
using TheManXS.Model.Map;
using IT = TheManXS.Model.ParametersForGame.InfrastructureType;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.InfrastructureStuff
{
    public enum InfrastructurePhase { IsProposed, IsUnderConstruction, IsActive, IsOutOfCommision }
    public class Infrastructure
    {
        System.Random rnd = new System.Random();
        Game _game;
        public Infrastructure() { }

        private SQMapConstructArray _map;
        public Infrastructure(bool isNewGame, SQMapConstructArray map, Game game)
        {
            _game = game;
            _map = map;
            if (isNewGame) { InitNewInfrastructure(); }
        }

        private void InitNewInfrastructure()
        {
            infrastructureMapArray = InitArray();
            new MainRoad(infrastructureMapArray,_map.CityStartSQ);
            new MainRiver(infrastructureMapArray, _map, _game);
            UpdateInfrastructureInSQArray();
        }

        private SQ[,] infrastructureMapArray;

        private SQ[,] InitArray()
        {
            using (DBContext db = new DBContext())
            {
                SQ[,] a = new SQ[QC.RowQ, QC.ColQ];
                for (int row = 0; row < QC.RowQ; row++)
                {
                    for (int col = 0; col < QC.ColQ; col++)
                    {                        
                        a[row, col] = _game.SQList[row, col];
                    }
                }
                return a;
            }            
        }

        private void UpdateInfrastructureInSQArray()
        {
            List<SQ> sList = ConvertArrayToList();

            using (DBContext db = new DBContext())
            {
                db.SQ.UpdateRange(sList);
                db.SaveChanges();
            }
            
            List < SQ > ConvertArrayToList()
            {
                List<SQ> sqList = new List<SQ>();

                for (int row = 0; row < infrastructureMapArray.GetLength(0); row++)
                {
                    for (int col = 0; col < infrastructureMapArray.GetLength(1); col++)
                    {
                        sqList.Add(infrastructureMapArray[row, col]);
                    }
                }

                return sqList;
            }
        }
        private void InitInfrastructureForStartSQsToHubs()
        {
            using (DBContext db = new DBContext())
            {
                var startSQList = db.SQ.Where(s => s.IsStartSquare == true).ToList();
                var sqList = db.SQ.ToList();

                for (int i = 0; i < startSQList.Count; i++)
                {
                    
                }

                void initInfrastructure(SQ sq)
                {

                }
            }
        }

        
        public void ExecuteConstructionOfRoute(IT it,List<SQ> sqList)
        {
            using (DBContext db = new DBContext())
            {
                foreach (SQ sq in sqList)
                {
                    switch (it)
                    {
                        //case IT.Road:
                        //    sq.IsSecondaryRoad = true;
                        //    break;
                        //case IT.RailRoad:
                        //    sq.IsTrainConnected = true;
                        //    break;
                        //case IT.Pipeline:
                        //    sq.IsPipelineConnected = true;
                        //    break;
                        //default:
                        //    break;
                    }
                    db.SQ.Update(sq);
                }
                db.SaveChanges();
            }
        }
    }
}
