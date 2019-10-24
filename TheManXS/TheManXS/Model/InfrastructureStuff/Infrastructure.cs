using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.InfrastructureStuff;
using TheManXS.Model.Map;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Main.InfrastructureStuff
{
    public class Infrastructure
    {
        private SQMapConstructArray _map;
        public Infrastructure() { }
        public Infrastructure(bool isNewGame, SQMapConstructArray map)
        {
            _map = map;
            if (isNewGame) { InitNewInfrastructure(); }
        }
        private void InitNewInfrastructure()
        {
            new MainRoad(_map);
            new MainRiver(_map);
            // pipelines
            // train
        }
    }
}
