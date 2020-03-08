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
        enum RandomVariablesForTerrainConstruction { GrasslandRatio, ForestRatio, Offset, Axis }
        public Terrain() { }
        public Terrain(bool isNewGame, SQMapConstructArray map, Game game)
        {
            _game = game;
            _map = map;
            _startRowRatio = _game.ParameterConstantList.GetConstant(AllConstantParameters.MapConstants, (int)MapConstantsSecondary.StartRowRatioFromEdgeOfMap);

            if (isNewGame)
            {
                InitNewMap();
            }
        }

        private void InitNewMap()
        {
            int axisShift, offset, R, stSqR;
            int northGL, southGL, northFNorth, northFSouth, southFNorth, southFSouth;
            stSqR = (int)(_startRowRatio * QC.RowQ - (int)(double)QC.RowQ * _forestWidthRatio / 2);
            //stSqR = GetRowNumber(_startRowRatio) - (int)((double)QC.RowQ * _forestWidthRatio.LBOrConstant / 2);
            R = stSqR;

            for (int c = 0; c < QC.ColQ; c++)
            {
                //northGL = R;
                //southGL = northGL + GetWidth(_grasslandWidthRatio);
                //northFSouth = northGL - 1;
                //northFNorth = northFSouth - GetWidth(_forestWidthRatio);
                //southFNorth = southGL + 1;
                //southFSouth = southFNorth + GetWidth(_forestWidthRatio);

                northGL = R;
                southGL = northGL + GetRandom(RandomVariablesForTerrainConstruction.GrasslandRatio);
                northFSouth = northGL - 1;
                northFNorth = northFSouth - GetRandom(RandomVariablesForTerrainConstruction.ForestRatio);
                southFNorth = southGL + 1;
                southFSouth = southFNorth + GetRandom(RandomVariablesForTerrainConstruction.ForestRatio);

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
                offset = GetRandom(RandomVariablesForTerrainConstruction.Offset);
                axisShift = GetRandom(RandomVariablesForTerrainConstruction.Axis);
                //offset = rnd.Next((int)_offsetBounds.LBOrConstant, (int)_offsetBounds.UB);
                //axisShift = rnd.Next((int)_axisShiftBounds.LBOrConstant, (int)_axisShiftBounds.UB);

                stSqR += axisShift;
                R = stSqR + offset;
            }
        }
        
        int GetRandom(RandomVariablesForTerrainConstruction rv)
        {
            switch (rv)
            {
                case RandomVariablesForTerrainConstruction.GrasslandRatio:
                    return (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.GrasslandWidthRatio);
                case RandomVariablesForTerrainConstruction.ForestRatio:
                    return (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.ForestWidthRatio);
                case RandomVariablesForTerrainConstruction.Offset:
                    return (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.TerrainOffset);
                case RandomVariablesForTerrainConstruction.Axis:
                    return (int)_game.ParameterBoundedList.GetRandomValue(AllBoundedParameters.TerrainConstruct, (int)TerrainBoundedConstructSecondary.AxisShift);
                default:
                    return 0;
            }
        }
        private int GetRowNumber() => (int)_startRowRatio * QC.RowQ;
        
    }
}
