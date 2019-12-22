using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map.Surface
{
    public class StartSQ
    {
        System.Random rnd = new System.Random();
        SQMapConstructArray _map;
        public StartSQ() { }
        public StartSQ(SQMapConstructArray map)
        {
            _map = map;
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

                if(_map[row,col].TerrainType == Settings.SettingsMaster.TerrainTypeE.Grassland && 
                    _map[row,col].ResourceType != Settings.SettingsMaster.ResourceTypeE.Nada &&
                    _map[row,col].OwnerNumber == 0)
                {
                    _map[row, col].OwnerNumber = countOfStSQs;
                    _map[row, col].IsStartSquare = true;
                    countOfStSQs++;
                }
            } while (countOfStSQs < QC.PlayerQ);
        }
    }
}
