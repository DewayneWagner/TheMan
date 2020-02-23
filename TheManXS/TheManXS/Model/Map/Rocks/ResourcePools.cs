using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using TB = TheManXS.Model.Settings.SettingsMaster.TerrainConstructBounded;
using TC = TheManXS.Model.Settings.SettingsMaster.TerrainConstructConstants;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using PP = TheManXS.Model.Settings.SettingsMaster.PoolParams;
using TheManXS.Model.Main;
using TheManXS.Model.Map.Surface;

namespace TheManXS.Model.Map.Rocks
{
    public class ResourcePools
    {
        System.Random rnd = new System.Random();
        private SQMapConstructArray _map;
        public ResourcePools(bool placeholder, SQMapConstructArray map)
        {
            _map = map;
            FormationCounter = 1;
            TotalResSq = 0;

            do
            {
                Pool p = new Pool(this, _map);
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
                sq.Status = SettingsMaster.StatusTypeE.Producing;
                sq.Production = QC.StartSQProduction;
                sq.OPEXPerUnit = QC.StartSQOpex;
                playerNumber++;
            }            
        }
    }
}
