using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using TB = TheManXS.Model.Settings.SettingsMaster.TerrainConstructBounded;
using TC = TheManXS.Model.Settings.SettingsMaster.TerrainConstructConstants;
using TT = TheManXS.Model.Settings.SettingsMaster.TerrainTypeE;
using TheManXS.Model.Main;

namespace TheManXS.Model.Map.Surface
{
    public class Terrain
    {
        private SQMapConstructArray _map;
        private Setting startRowRatio, grasslandWidthRatio, forestRatio, offsetBounds, axisShiftBounds;
        System.Random rnd = new System.Random();

        public Terrain() { }
        public Terrain(bool isNewGame, SQMapConstructArray map)
        {
            _map = map;

            if (isNewGame)
            {
                InitFields();
                InitNewMap();
            }
        }

        private void InitNewMap()
        {
            int axisShift, offset, R, stSqR;
            int northGL, southGL, northFNorth, northFSouth, southFNorth, southFSouth;

            stSqR = GetRowNumber(startRowRatio) - (int)((double)QC.RowQ * forestRatio.LBOrConstant / 2);
            R = stSqR;

            for (int c = 0; c < QC.ColQ; c++)
            {
                northGL = R;
                southGL = northGL + GetWidth(grasslandWidthRatio);
                northFSouth = northGL - 1;
                northFNorth = northFSouth - GetWidth(forestRatio);
                southFNorth = southGL + 1;
                southFSouth = southFNorth + GetWidth(forestRatio);

                for (int r = 0; r < QC.RowQ; r++)
                {
                    if (r < northFNorth)
                        _map[r, c].TerrainType = TT.Mountain;
                    else if (r >= northFNorth && r <= northFSouth)
                        _map[r, c].TerrainType = TT.Forest;
                    else if (r >= northGL && r <= southGL)
                        _map[r, c].TerrainType = TT.Grassland;
                    else if (r >= southFNorth && r < southFSouth)
                        _map[r, c].TerrainType = TT.Forest;
                    else
                        _map[r, c].TerrainType = TT.Mountain;
                }
                offset = rnd.Next((int)offsetBounds.LBOrConstant, (int)offsetBounds.UB);
                axisShift = rnd.Next((int)axisShiftBounds.LBOrConstant, (int)axisShiftBounds.UB);

                stSqR += axisShift;
                R = stSqR + offset;
            }
        }
        private void InitFields()
        {
            using (DBContext db = new DBContext())
            {
                var _startRowRatioQuery = db.Settings.Where(s => s.Key == 
                    Setting.GetKey(AS.MapConstants, (int)TC.StartRowRatio));
                    startRowRatio = _startRowRatioQuery.FirstOrDefault();

                var _grassLandWidthRatioQuery = db.Settings.Where(s => s.Key == 
                    Setting.GetKey(AS.TerrainBoundedTCB, (int)TB.GrasslandWidthRatio));
                    grasslandWidthRatio = _grassLandWidthRatioQuery.FirstOrDefault();

                var _forestRatioQuery = db.Settings.Where(s => s.Key ==
                    Setting.GetKey(AS.TerrainBoundedTCB, (int)TB.ForestWidthRatio));
                    forestRatio = _forestRatioQuery.FirstOrDefault();

                var _offsetBoundsQuery = db.Settings.Where(s => s.Key ==
                    Setting.GetKey(AS.TerrainBoundedTCB, (int)TB.TerrainOffset));
                    offsetBounds = _offsetBoundsQuery.FirstOrDefault();

                var _axisShiftBoundsQuery = db.Settings.Where(s => s.Key ==
                    Setting.GetKey(AS.TerrainBoundedTCB, (int)TB.AxisShift));
                    axisShiftBounds = _axisShiftBoundsQuery.FirstOrDefault();
            };
        }
        private int GetRowNumber(Setting s)
        {
            double lb = s.LBOrConstant * QC.RowQ;
            double ub = QC.RowQ - lb;
            double r = rnd.NextDouble();

            return (int)((ub - lb) * r + lb);
        }
        private int GetWidth(Setting s)
        {
            int lb = (int)(s.LBOrConstant * QC.RowQ);
            int ub = (int)(s.UB * QC.RowQ);
            return rnd.Next(lb, ub);
        }
    }
}
