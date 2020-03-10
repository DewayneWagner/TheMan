using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using PB = TheManXS.Model.ParametersForGame.AllBoundedParameters;
using PC = TheManXS.Model.ParametersForGame.AllConstantParameters;
using TheManXS.Model.ParametersForGame;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map.Surface
{
    class TerrainColumn
    {
        Game _game;
        public TerrainColumn(int northGrassland, Game game)
        {
            _game = game;
            NorthOfGrassland = northGrassland;
            SetProperties();
        }
        private void SetProperties()
        {
            var pb = _game.ParameterBoundedList;
            var pc = _game.ParameterConstantList;

            setSourthGrassland();
            setNorthForestBoundaries();
            setSouthForestBoundaries();
            
            void setSourthGrassland()
            {
                int grasslandWidth = (int)(pb.GetRandomValue(PB.TerrainConstruct, 
                    (int)TerrainBoundedConstructSecondary.GrasslandWidthRatio) * QC.RowQ);
                SouthOfGrassland = NorthOfGrassland + grasslandWidth;
            }
            void setNorthForestBoundaries()
            {
                int northForestWidth = (int)(pb.GetRandomValue(PB.TerrainConstruct, 
                    (int)TerrainBoundedConstructSecondary.ForestWidthRatio));
                NorthOfNorthForest = NorthOfGrassland - northForestWidth;
                SouthOfNorthForest = NorthOfGrassland - 1;
            }
            void setSouthForestBoundaries()
            {
                int southForestWidth = (int)(pb.GetRandomValue(PB.TerrainConstruct,
                    (int)TerrainBoundedConstructSecondary.ForestWidthRatio));
                NorthOfSouthForest = SouthOfGrassland + 1;
                SouthOfSouthForest = NorthOfSouthForest + southForestWidth;
            }

        }
        public int NorthOfNorthForest { get; set; }
        public int SouthOfNorthForest { get; set; }
        public int NorthOfGrassland { get; set; }
        public int SouthOfGrassland { get; set; }
        public int NorthOfSouthForest { get; set; }
        public int SouthOfSouthForest { get; set; }
    }
}
