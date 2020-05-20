using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

namespace TheManXS.Model.Map.Rocks
{
    public class ResourcePools
    {
        System.Random rnd = new System.Random();
        private SQMapConstructArray _map;
        Game _game;
        public ResourcePools(bool placeholder, SQMapConstructArray map, Game game)
        {
            _game = game;
            _map = map;
            FormationCounter = 1;
            TotalResSq = 0;

            do
            {
                Pool p = new Pool(this, _map, _game);
            } while (TotalResSq < QC.MaxResourceSQsOnMap);

            AssignStartSQs();
        }
        public int TotalResSq { get; set; }
        public int FormationCounter { get; set; }
        private void AssignStartSQs()
        {
            int playerNumber = QC.PlayerIndexActual;
            int row, col;
            SQ sq;
            int loopCounter = 0;
            int maxLoops = 100;

            while (playerNumber <= QC.PlayerQ)
            {
                row = rnd.Next(0, QC.RowQ);
                col = rnd.Next(0, QC.ColQ);
                loopCounter++;

                if (Coordinate.DoesSquareExist(row, col))
                {
                    sq = _map[row, col];
                    if (SQMeetsStartSqConditions()) { assignStartSQ(); }
                }
            };
            bool SQMeetsStartSqConditions()
            {
                if (sq.OwnerNumber == QC.PlayerIndexTheMan && sq.ResourceType != RT.Nada)
                {
                    if (loopCounter < maxLoops && sq.TerrainType == TT.Grassland) { return true; }
                    else if (loopCounter > maxLoops && sq.TerrainType == TT.Forest) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            void assignStartSQ()
            {
                sq.OwnerNumber = playerNumber;
                sq.Status = StatusTypeE.Producing;
                sq.Production = QC.StartSQProduction;
                sq.OPEXPerUnit = QC.StartSQOpex;
                playerNumber++;
            }
        }
    }
}
