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
using ABP = TheManXS.Model.ParametersForGame.AllBoundedParameters;
using ACP = TheManXS.Model.ParametersForGame.AllConstantParameters;

namespace TheManXS.Model.Map.Surface
{
    public class Terrain
    {
        private SQMapConstructArray _map;
        private double _startRowRatio;
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
            int northGrassLandStart = GetStartSQ();
            
            for (int Col = 0; Col < QC.ColQ; Col++)
            {
                TerrainColumnList tc = new TerrainColumnList(new TerrainColumn(northGrassLandStart, _game));

                for (int Row = 0; Row < QC.RowQ; Row++)
                {
                    _map[Row, Col].TerrainType = tc[Row];
                }
            }
        }

        private void InitNewMap(bool isOldVersion)
        {
            var pb = _game.ParameterBoundedList;
            var pc = _game.ParameterConstantList;
            int startSq = GetStartSQ();
            int northGLBoundary = startSq;
            int GLWidth = (int)(QC.RowQ * pb.GetRandomValue(ABP.TerrainConstruct, (int)TerrainBoundedConstructSecondary.GrasslandWidthRatio));
            int southGLBoundary = startSq + GLWidth;





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
                southGL = northGL + (GetRandom(RandomVariablesForTerrainConstruction.GrasslandRatio)*QC.RowQ);
                northFSouth = northGL - 1;
                northFNorth = northFSouth - (GetRandom(RandomVariablesForTerrainConstruction.ForestRatio)*QC.RowQ);
                southFNorth = southGL + 1;
                southFSouth = southFNorth + (GetRandom(RandomVariablesForTerrainConstruction.ForestRatio)*QC.RowQ);

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
        private int GetStartSQ()
        {
            double startSqRatioFromEdgeOfMap = _game.ParameterConstantList.GetConstant(ACP.MapConstants, (int)MapConstantsSecondary.StartRowRatioFromEdgeOfMap);
            int min = (int)(QC.RowQ * startSqRatioFromEdgeOfMap);
            int max = (int)(QC.RowQ - min);
            return rnd.Next(min, max);
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
