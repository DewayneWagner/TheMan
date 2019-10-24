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
    public class PoolCrossSection
    {
        private SQMapConstructArray _map;
        System.Random rnd = new System.Random();
        public PoolCrossSection(Pool p, SQMapConstructArray map)
        {
            _map = map;
            PoolWidth = (int)Setting.GetRand(AS.PoolParams, (int)PP.PoolWidth);
            SQ sq;

            int offset = rnd.Next(-1, 2);
            int row = p.StartCoordinate.Row + (offset * p.YY);
            int col = p.StartCoordinate.Col + (offset * p.XX);

            for (int i = 0; i < PoolWidth; i++)
            {
                if (Coordinate.DoesSquareExist(row,col))
                {
                    sq = _map[row, col];
                    if (sq.ResourceType == RT.Nada)
                    {
                        sq.ResourceType = p.RT;
                        sq.FormationID = p.Formation.ID;
                        sq.Production = (int)Setting.GetRand(AS.ProductionTT, (int)sq.TerrainType);
                        sq.OPEXPerUnit = (int)Setting.GetRand(AS.OPEXTT, (int)sq.TerrainType);
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
