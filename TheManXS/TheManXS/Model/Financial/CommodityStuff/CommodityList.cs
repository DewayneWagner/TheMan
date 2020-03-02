using System;
using System.Collections.Generic;
using System.Text;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;

namespace TheManXS.Model.Financial.CommodityStuff
{
    public class CommodityList
    {
        private static double _startPrice;
        private static double _minPrice;
        private static double _maxPrice;
        private static double _minPriceFluctuation;
        private static double _maxPriceFluctuation;
        private static double _spreadBetweenMinAndMax;

        private System.Random rnd = new System.Random();
        public CommodityList(bool isForNewGame)
        {
            _startPrice = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.CommStartPrice);
            _minPrice = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MinCommPrice);
            _maxPrice = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MaxCommPrice);

            _minPriceFluctuation = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MinCommodityChange);
            _maxPriceFluctuation = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MaxCommodityChange);
            _spreadBetweenMinAndMax = _minPriceFluctuation + _maxPriceFluctuation;
            _minPriceFluctuation *= (-1);

            InitAllCommodityPricing();
        }

        private void InitAllCommodityPricing()
        {
            using (DBContext db = new DBContext())
            {
                for (int i = 0; i < (int)RT.Total; i++)
                {
                    Commodity c = new Commodity()
                    {
                        Delta = 0,
                        Price = _startPrice,
                        ResourceTypeNumber = i,
                        Turn = QC.TurnNumber,
                    };
                    db.Commodity.Add(c);
                }
                db.SaveChanges();
            }            
        }
        public void AdvancePricing()
        {
            using (DBContext db = new DBContext())
            {
                int lastTurnNumber = QC.TurnNumber - 1;
                double oldPrice;
                double delta;
                double newPrice;

                for (int i = 0; i < (int)RT.Total; i++)
                {
                    var comm = db.Commodity
                        .Where(c => c.Turn == lastTurnNumber)
                        .Where(c => c.ResourceTypeNumber == i)
                        .FirstOrDefault();

                    oldPrice = comm.Price;
                    delta = (rnd.NextDouble() * _spreadBetweenMinAndMax) + _minPriceFluctuation;
                    newPrice = oldPrice * (1 + delta);

                    Commodity newComm = new Commodity()
                    {
                        Delta = delta,
                        Price = newPrice,
                        Turn = QC.TurnNumber,
                        ResourceTypeNumber = i,
                    };

                    db.Commodity.Add(newComm);
                }
            }
        }
    }
}
