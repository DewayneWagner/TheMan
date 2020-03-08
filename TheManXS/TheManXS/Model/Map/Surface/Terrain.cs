using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;
using TheManXS.Model.Main;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Map.Surface
{
    public class Terrain
    {
        private SQMapConstructArray _map;
        private double _startRowRatio, _grasslandWidthRatio, _forestWidthRatio, _offsetBounds, _axisShiftBounds;
        System.Random rnd = new System.Random();
        Game _game;
        public Terrain() { }
        public Terrain(bool isNewGame, SQMapConstructArray map, Game game)
        {
            _game = game;
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

            stSqR = GetRowNumber(_startRowRatio) - (int)((double)QC.RowQ * _forestWidthRatio.LBOrConstant / 2);
            R = stSqR;

            for (int c = 0; c < QC.ColQ; c++)
            {
                northGL = R;
                southGL = northGL + GetWidth(_grasslandWidthRatio);
                northFSouth = northGL - 1;
                northFNorth = northFSouth - GetWidth(_forestWidthRatio);
                southFNorth = southGL + 1;
                southFSouth = southFNorth + GetWidth(_forestWidthRatio);

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
                offset = rnd.Next((int)_offsetBounds.LBOrConstant, (int)_offsetBounds.UB);
                axisShift = rnd.Next((int)_axisShiftBounds.LBOrConstant, (int)_axisShiftBounds.UB);

                stSqR += axisShift;
                R = stSqR + offset;
            }
        }
        private void InitFields()
        {
            _startRowRatio = _game.ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.StartRowRatioFromEdgeOfMap);
            _forestWidthRatio = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.ForestWidthRatio);
            _grasslandWidthRatio = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.GrasslandWidthRatio);
            _offsetBounds = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.TerrainOffset);
            _axisShiftBounds = _game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.AxisShift);
        }
        private int GetRowNumber() => (int)_startRowRatio * QC.RowQ;
        private int GetWidth(double ratio) => 
        {
            int lb = (int)(s.LBOrConstant * QC.RowQ);
            int ub = (int)(s.UB * QC.RowQ);
            return rnd.Next(lb, ub);
        }
    }
}
