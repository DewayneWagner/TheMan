using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using System.Linq;
using TheManXS.Model.Settings;
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
            // need to initialize the infrastructure from startSqs to Hubs

            //SQInfrastructure.WriteArrayToDB(infrastructureMapArray);
            new SQInfrastructureList(infrastructureMapArray);
        }

        private SQInfrastructure[,] infrastructureMapArray;

        private SQInfrastructure[,] InitArray()
        {
            using (DBContext db = new DBContext())
            {
                SQInfrastructure[,] a = new SQInfrastructure[QC.RowQ, QC.ColQ];
                for (int row = 0; row < QC.RowQ; row++)
                {
                    for (int col = 0; col < QC.ColQ; col++)
                    {
                        a[row, col] = new SQInfrastructure(db.SQ.Find(Coordinate.GetSQKey(row, col)));
                    }
                }
                return a;
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

        public SQ ClosestInfrastructureConnectedSQ(IT ClosestITToFind, SQ sq)
        {
            int tRow, tCol;
            using (DBContext db = new DBContext())
            {
                SQ adjacentSQ;
                for (int x = 1; x < QC.ColQ; x++)
                {
                    for (int xx = x; xx >= -x; xx--)
                    {
                        for (int xxx = x; xxx >= -x; xxx--)
                        {
                            tRow = sq.Row + xx;
                            tCol = sq.Col + xxx;                            

                            //if (Coordinate.DoesSquareExist(tRow, tCol))
                            //{
                            //    adjacentSQ = db.SQ.Find(Coordinate.GetSQKey(tRow, tCol));

                            //    if (adjacentSQ.IsHub ||
                            //        adjacentSQ.IsMainTransportationCorridor && ClosestITToFind == IT.Road ||
                            //        adjacentSQ.IsPipelineConnected && ClosestITToFind == IT.Pipeline ||
                            //        adjacentSQ.IsTrainConnected && ClosestITToFind == IT.RailRoad ||
                            //        adjacentSQ.IsSecondaryRoad && ClosestITToFind == IT.Secondary)
                            //    {
                            //        return adjacentSQ;
                            //    }
                            //}
                        }
                    }
                }
            }
            return sq;
        }
        public List<SQ> FindRouteForRoadBetweenTwoPoints(SQ from, SQ to)
        {
            List<SQ> sqRoute = new List<SQ>();
            int rDirection = GetDirection(to.Row - from.Row);
            int cDirection = GetDirection(to.Col - from.Col);

            int tCol = from.Col;
            int tRow = from.Row;
            SQ sq;

            using (DBContext db = new DBContext())
            {
                do
                {
                    if (tRow != to.Row)
                    {
                        if(Math.Abs(tRow - to.Row) >= 3) { tRow += GetAdjustment(); }
                        else { tRow += rDirection; }
                    }

                    if (tCol != to.Col)
                    {
                        if(Math.Abs(tCol - to.Col) >= 3) { tCol += GetAdjustment(); }
                        else { tCol += cDirection; }
                    }

                    if (Coordinate.DoesSquareExist(tRow,tCol))
                    {
                        sq = db.SQ.Find(Coordinate.GetSQKey(tRow, tCol));
                        sqRoute.Add(sq);
                    }

                } while (tRow != to.Row || tCol != to.Col);
            }
            return sqRoute;

            int GetDirection(int diff)
            {
                if (diff < 0)
                    return -1;
                else if (diff == 0)
                    return 0;
                else
                    return 1;
            }
            int GetAdjustment() => rnd.Next(-1, 2);
        }
        public double EstimateRoadConstructionCost(List<SQ> roadRouteList)
        {
            int grassland = 0, forest = 0, mountain = 0;
            
            foreach (SQ sq in roadRouteList)
            {
                switch (sq.TerrainType)
                {
                    case TT.Forest:
                        forest++;
                        break;
                    case TT.Grassland:
                        grassland++;
                        break;
                    case TT.Mountain:
                        mountain++;
                        break;
                }
            }
            double costs = 0;

            var c = _game.ParameterConstantList;
            costs += (grassland * c.GetConstant(AllConstantParameters.InfrastructureConstructionRatiosTT, (int)TT.Grassland));
            costs += (forest * c.GetConstant(AllConstantParameters.InfrastructureConstructionRatiosTT, (int)TT.Forest));
            costs += (mountain * c.GetConstant(AllConstantParameters.InfrastructureConstructionRatiosTT, (int)TT.Mountain));
            return costs;        
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
