using System;
using System.Collections.Generic;
using System.Text;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Model.Financial.CommodityStuff
{
    public class Commodity
    {
        public Commodity() { SavedGameSlot = QC.CurrentSavedGameSlot; }
        public int ID { get; set; }
        public double Price { get; set; }
        public double Delta { get; set; }
        public int ResourceTypeNumber { get; set; }
        public int Turn { get; set; }
        public int SavedGameSlot { get; set; }
        public double FourTurnMovingAvgPricing { get; set; }
    }
}
