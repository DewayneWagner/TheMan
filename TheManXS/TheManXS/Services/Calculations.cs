using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using Xamarin.Forms;
using RT = TheManXS.Model.ParametersForGame.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Units;

namespace TheManXS.Services
{
    public class Calculations
    {
        Game _game;
        public Cash GetCash(SQ sq)
        {
            SetGame();
            double revenue, opex, transport;
            if (sq.ResourceType != RT.Nada)
            {
                revenue = sq.Production * _game.CommodityList[(int)sq.ResourceType].Price;
                opex = sq.OPEXPerUnit * sq.Production;
                transport = sq.Transport * sq.Production;
            }
            else
            {
                revenue = 0;
                opex = 0;
                transport = 0;
            }
            return new Cash(revenue, opex, transport);
        }
        public Cash GetCash(Player player, RT rt = RT.Nada)
        {
            SetGame();
            List<SQ> sqList = new List<SQ>();

            if(rt == RT.Nada)
            {
                foreach (KeyValuePair<int,SQ> s in _game.SquareDictionary)
                {
                    if(s.Value.OwnerNumber == player.Number) { sqList.Add(s.Value); }
                }
            }
            else
            {
                foreach (KeyValuePair<int,SQ> s in _game.SquareDictionary)
                {
                    if(s.Value.OwnerNumber == player.Number && s.Value.ResourceType == rt)
                    {
                        sqList.Add(s.Value);
                    }
                }
            }

            double revenue = 0, opex = 0, transport = 0, commPrice = 0;

            foreach (SQ sq in sqList)
            {
                commPrice = _game.CommodityList[(int)sq.ResourceType].Price;
                revenue += sq.Production * commPrice;
                opex += sq.Production * sq.OPEXPerUnit;
                transport += sq.Production * sq.Transport;
            }
            return new Cash(revenue, opex, transport);
        }
        public Cash GetCash(Unit unit)
        {
            SetGame();

            Cash unitCash = new Cash();

            foreach (SQ sq in unit)
            {
                Cash sqCash = GetCash(sq);

                unitCash.UnitProduction += sq.Production;                
                unitCash.Revenue += sqCash.Revenue;
                unitCash.OPEX += sqCash.OPEX;
                unitCash.Transport += sqCash.Transport;
                unitCash.UnitNexActionCost += sqCash.UnitNexActionCost;                
            }
            unitCash.ProfitDollar = unitCash.Revenue - unitCash.OPEX - unitCash.Transport;
            unitCash.ProfitPercent = (unitCash.Revenue != 0) ? (unitCash.ProfitDollar / unitCash.Revenue) : 0;

            return unitCash;
        }
        private void SetGame() => _game = (Game)App.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.Game)];
    }
}
