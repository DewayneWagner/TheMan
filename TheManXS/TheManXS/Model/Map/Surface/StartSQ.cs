using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.ParametersForGame;

namespace TheManXS.Model.Map.Surface
{
    public class StartSQ
    {
        System.Random rnd = new System.Random();
        SQMapConstructArray _sqMap;
        SQInfrastructure[,] _map;

        public StartSQ() { }
        public StartSQ(SQMapConstructArray map)
        {
            //_map = map;
        }
        public StartSQ(SQInfrastructure[,] map, SQMapConstructArray sqMap)
        {
            _map = map;
            _sqMap = sqMap;

            InitNewStartSQ();
        }
        private void InitNewStartSQ()
        {
            int countOfStSQs = 0;
            int row = 0;
            int col = 0;

            do
            {
                row = rnd.Next(QC.RowQ);
                col = rnd.Next(QC.ColQ);

                if(_sqMap[row,col].TerrainType == TerrainTypeE.Grassland && 
                    _sqMap[row,col].ResourceType != ResourceTypeE.Nada &&
                    _sqMap[row,col].OwnerNumber == 0)
                {
                    _sqMap[row, col].OwnerNumber = countOfStSQs;
                    _sqMap[row, col].IsStartSquare = true;
                    countOfStSQs++;
                }
            } while (countOfStSQs < QC.PlayerQ);
        }
    }
}
