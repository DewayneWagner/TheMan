using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.ParametersForGame;
using TT = TheManXS.Model.ParametersForGame.TerrainTypeE;

namespace TheManXS.Model.Map.Surface
{
    class TerrainColumnList : List<TT>
    {
        TerrainColumn _terrainColumn;
        public TerrainColumnList(TerrainColumn terrainColumn)
        {
            _terrainColumn = terrainColumn;
            InitNorthMountain();
            InitNorthForest();
            InitGrassLand();
            InitSouthForest();
            InitSouthMountains();
        }
        void InitNorthMountain()
        {
            for (int i = 0; i < _terrainColumn.NorthOfNorthForest; i++)
            {
                this.Add(TT.Mountain);
            }
        }
        void InitNorthForest()
        {
            for (int i = _terrainColumn.NorthOfNorthForest; i < _terrainColumn.SouthOfNorthForest; i++)
            {
                this.Add(TT.Forest);
            }
        }
        void InitGrassLand()
        {
            for (int i = _terrainColumn.NorthOfGrassland; i < _terrainColumn.SouthOfGrassland; i++)
            {
                this.Add(TT.Grassland);
            }
        }
        void InitSouthForest()
        {
            for (int i = _terrainColumn.NorthOfSouthForest; i < _terrainColumn.SouthOfSouthForest; i++)
            {
                this.Add(TT.Forest);
            }
        }
        void InitSouthMountains()
        {
            for (int i = (_terrainColumn.SouthOfSouthForest + 1); i < QC.RowQ; i++)
            {
                this.Add(TT.Mountain);
            }
        }
    }
}
