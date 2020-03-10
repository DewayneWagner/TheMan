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
        Game _game;
        System.Random rnd = new System.Random();

        public Terrain(SQMapConstructArray map, Game game)
        {
            _game = game;
            _map = map;
            InitNewMap();
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
                setNextNorthGrasslandStartSQ();
            }
            void setNextNorthGrasslandStartSQ()
            {
                int offset = (int)(_game.ParameterBoundedList.GetRandomValue(ABP.TerrainConstruct,
                    (int)TerrainBoundedConstructSecondary.TerrainOffset));
                northGrassLandStart += offset;
            }
        }

        private int GetStartSQ()
        {
            double startSqRatioFromEdgeOfMap = _game.ParameterConstantList.GetConstant(ACP.MapConstants, (int)MapConstantsSecondary.StartRowRatioFromEdgeOfMap);
            int min = (int)(QC.RowQ * startSqRatioFromEdgeOfMap);
            int max = (int)(QC.RowQ - min);
            return rnd.Next(min, max);
        }
    }
}
