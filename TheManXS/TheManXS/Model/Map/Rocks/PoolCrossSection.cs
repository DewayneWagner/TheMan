using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.ParametersForGame;
using PP = TheManXS.Model.ParametersForGame.PoolConstructParametersSecondary;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;

namespace TheManXS.Model.Map.Rocks
{
    public class PoolCrossSection
    {
        private SQMapConstructArray _map;
        System.Random rnd = new System.Random();
        Game _game;
        public PoolCrossSection(Pool p, SQMapConstructArray map, Game game)
        {
            _map = map;
            _game = game;
            PoolWidth = (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.PoolConstructParameters, (int)PP.PoolWidth);
            SQ sq;

            int offset = rnd.Next(-1, 2);
            int row = p.StartCoordinate.Row + (offset * p.YY);
            int col = p.StartCoordinate.Col + (offset * p.XX);

            for (int i = 0; i < PoolWidth; i++)
            {
                if (Coordinate.DoesSquareExist(row, col))
                {
                    sq = _map[row, col];
                    if (sq.ResourceType == RT.Nada)
                    {
                        sq.ResourceType = p.RT;
                        sq.FormationID = p.Formation.ID;
                        sq.Production = (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.ProductionUnitsPerTerrainType, (int)p.RT);
                        sq.OPEXPerUnit = (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.ActionCosts, (int)ActionCostsSecondary.OpexCostPerUnit);

                        p.TotalResSq++;

                        row += (p.X * p.YY);
                        col += (p.Y * p.XX);

                        p.PoolResSqCounter++;
                    }
                }
                else
                    break;
            }
        }
        private int PoolWidth { get; set; }
    }
}
