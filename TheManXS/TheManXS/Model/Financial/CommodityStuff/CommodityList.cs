using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.ParametersForGame;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
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

        System.Random rnd = new System.Random();

        public CommodityList() { }
        public CommodityList(Game game)
        {
            _game = game;

            SetCommodityConstants();
            InitPricingWithStartValues();
            SetFourTurnMovingAveragePrice();
            WriteCommodityListToDB();
        }
        private void SetCommodityConstants()
        {
            var c = _game.ParameterConstantList;
            _startPrice = c.GetConstant(AllConstantParameters.CommodityConstants,(int)CommodityConstantSecondary.StartPrice);
            _minPrice = c.GetConstant(AllConstantParameters.CommodityConstants, (int)CommodityConstantSecondary.MinPrice);
            _maxPrice = c.GetConstant(AllConstantParameters.CommodityConstants, (int)CommodityConstantSecondary.MaxPrice);
            _minPriceFluctuation = c.GetConstant(AllConstantParameters.CommodityConstants, (int)CommodityConstantSecondary.MinChange);
            _maxPriceFluctuation = c.GetConstant(AllConstantParameters.CommodityConstants, (int)CommodityConstantSecondary.MaxChange);
            _spreadBetweenMinAndMax = _maxPriceFluctuation - _minPriceFluctuation;
            _minPriceFluctuation *= (-1);
        }

        public void InitPricingWithStartValues()
        {
            for (int i = 0; i < (int)RT.Total; i++)
            {
                Add(new Commodity(_startPrice,0,i,_game.TurnNumber)); 
            }  
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

                newCommodityPricing.Add(new Commodity(newPrice, fluctuation, i, _game.TurnNumber));
            }

            setRealEstatePricing();
            UpdateThisCommodityListObjectWithNewCommPricing(newCommodityPricing);
            SetFourTurnMovingAveragePrice();            
            WriteCommodityListToDB();

            void setRealEstatePricing()
            {
                newCommodityPricing.Add(new Commodity(_minPrice, 0, (int)RT.RealEstate,_game.TurnNumber));
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
        }
        private void SetFourTurnMovingAveragePrice()
        {
            if (_game.TurnNumber == 1) { setForFirstTurn(); }
            else { setForAllTurnsAfterFirst(); }

            void setForFirstTurn()
            {
                foreach (Commodity commodity in this)
                {
                    commodity.FourTurnMovingAvgPricing = _minPrice;
                }
            }
            void setForAllTurnsAfterFirst()
            {
                using (DBContext db = new DBContext())
                {
                    int startTurns = _game.TurnNumber < 4 ? 1 : _game.TurnNumber - 4;
                    for (int i = 0; i < this.Count; i++)
                    {
                        List<double> fourTurnsAverage = db.Commodity
                            .Where(c => c.Turn >= startTurns)
                            .Where(c => c.ResourceTypeNumber == i)
                            .Select(c => c.Price)
                            .ToList();

                        fourTurnsAverage.Add(this[i].Price);
                        this[i].FourTurnMovingAvgPricing = fourTurnsAverage.Average();
                    }
                }
            }
        }
        private void UpdateThisCommodityListObjectWithNewCommPricing(CommodityList newCommodityPricing)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i] = newCommodityPricing[i];
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
