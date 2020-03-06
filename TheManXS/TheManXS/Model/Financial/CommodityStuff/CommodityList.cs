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

        public CommodityList() { }
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
            for (int i = 0; i < (int)RT.RealEstate; i++)
            {
                Add(new Commodity()
                {
                    Delta = 0,
                    Price = _startPrice,
                    ResourceTypeNumber = (int)((RT)i),
                    Turn = 1,            
                    FourTurnMovingAvgPricing = _startPrice,
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
            CommodityList oldCommodityPricing = _game.CommodityList;
            CommodityList newCommodityPricing = new CommodityList();
            double fluctuation, newPrice;

            for (int i = 0; i < (int)RT.RealEstate; i++)
            {
                setPriceFluctuation();
                setNewPrice(oldCommodityPricing[i].Price);

                Commodity c = new Commodity()
                {
                    Price = newPrice,
                    Delta = fluctuation,
                    ResourceTypeNumber = i,
                    Turn = QC.TurnNumber
                };
                newCommodityPricing.Add(c);

                //newCommodityPricing.Add(new Commodity()
                //{
                //    Price = newPrice,
                //    Delta = fluctuation,
                //    ResourceTypeNumber = i,
                //    Turn = QC.TurnNumber
                //});
            }

            setRealEstatePricing();
            _game.CommodityList = newCommodityPricing;            
            WriteCommodityListToDB();
            setFourTurnMovingAveragePrice();

            void setRealEstatePricing()
            {
                newCommodityPricing.Add(new Commodity()
                {
                    Delta = 0,
                    Price = _minPrice,
                    Turn = QC.TurnNumber,
                    ResourceTypeNumber = (int)(RT.RealEstate),
                });                
            }

            void setPriceFluctuation()
            {
                fluctuation = (double)((_maxPriceFluctuation - (rnd.NextDouble() * (_maxPriceFluctuation + (-1 * _minPriceFluctuation)))) / 100);
            }
                
            void setNewPrice(double oldPrice)
            {
                //double tempNewPrice = oldPrice * fluctuation;
                double tempNewPrice = (oldPrice + (oldPrice * fluctuation));
                if (tempNewPrice < _minPrice) 
                { 
                    newPrice = _minPrice;
                    fluctuation = (oldPrice - newPrice) / 100;
                }
                else if(tempNewPrice > _maxPrice) 
                { 
                    newPrice = _maxPrice;
                    fluctuation = (oldPrice - newPrice) / 100;
                }
                else { newPrice = tempNewPrice; }
            }

            void setFourTurnMovingAveragePrice()
            {
                List<double> pastPrices = new List<double>();
                using (DBContext db = new DBContext())
                {
                    for(int i = 0; i < this.Count; i++)
                    {
                        int turnNumber = getTurnNumber();
                        pastPrices = db.Commodity
                            .Where(c => c.Turn >= turnNumber)
                            .Where(c => c.ResourceTypeNumber == i)
                            .Where(c => c.SavedGameSlot == QC.CurrentSavedGameSlot)
                            .Select(c => c.Price)
                            .ToList();

                        this[i].FourTurnMovingAvgPricing = pastPrices.Average();
                    }
                }
                int getTurnNumber() => _game.TurnNumber <= 4 ? 1 : (_game.TurnNumber - 4); 
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
