using System;
using System.Collections.Generic;
using System.Text;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using TheManXS.Model.Settings;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using System.Linq;
using TheManXS.Model.Main;

namespace TheManXS.Model.Financial.CommodityStuff
{
    public class CommodityList : List<Commodity>
    {
        Game _game;

        private static double _startPrice;
        private static double _minPrice;
        private static double _maxPrice;
        private static double _minPriceFluctuation;
        private static double _maxPriceFluctuation;
        private static double _spreadBetweenMinAndMax;

        private System.Random rnd = new System.Random();
        public CommodityList(Game game)
        {
            _game = game;
            _startPrice = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.CommStartPrice);
            _minPrice = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MinCommPrice);
            _maxPrice = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MaxCommPrice);

            _minPriceFluctuation = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MinCommodityChange);
            _maxPriceFluctuation = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.MaxCommodityChange);
            _spreadBetweenMinAndMax = _minPriceFluctuation + _maxPriceFluctuation;
            _minPriceFluctuation *= (-1);

            InitPricingWithStartValues();
            WriteCommodityListToDB();
        }

        public void InitPricingWithStartValues()
        {
            for (int i = 0; i < (int)RT.Total; i++)
            {
                Add(new Commodity()
                {
                    Delta = 0,
                    Price = _startPrice,
                    ResourceTypeNumber = (int)((RT)i),
                    Turn = 1,
                });
            }
            
            Add(new Commodity()
            {
                Delta = 0,
                Price = _startPrice,
                ResourceTypeNumber = (int)(RT.RealEstate),
                Turn = 1,
            });            
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
        private void WriteCommodityListToDB()
        {
            using (DBContext db = new DBContext())
            {
                db.Commodity.AddRange(this);
                db.SaveChanges();
            }
        }
    }
}
