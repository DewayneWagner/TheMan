using System;
using System.Collections.Generic;
using System.Text;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Map.Surface
{
    public class StartSQ
    {
        public StartSQ() { }
        public StartSQ(bool isNewGame)
        {
            if(isNewGame) { InitNewStartSQ(); }
        }
        private void InitNewStartSQ()
        {
            // remember to set status for all SQ's here
        }
    }
}
