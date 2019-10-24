using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Financial;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using Xamarin.Forms;
using RT = TheManXS.Model.Settings.SettingsMaster.ResourceTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;

namespace TheManXS.Services
{
    public class Calculations
    {
        public Cash GetCash(SQ sq)
        {
            using (DBContext db = new DBContext())
            {
                double revenue, opex, transport;
                if(sq.ResourceType != RT.Nada)
                {
                    var commPrice = db.Commodity.Where(p => p.Turn == QC.TurnNumber)
                    .Where(p => p.ResourceTypeNumber == (int)sq.ResourceType)
                    .FirstOrDefault();

                    revenue = sq.Production * commPrice.Price;
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
        }
        public Cash GetCash(Player player, RT rt = RT.Nada)
        {
            using (DBContext db = new DBContext())
            {
                List<SQ> playerList;
                if(rt == RT.Nada)
                {
                    playerList = db.SQ.Where(p => p.OwnerNumber == player.Number).ToList();
                }
                else 
                { 
                    playerList = db.SQ.Where(s => s.OwnerNumber == player.Number)
                        .Where(s => s.ResourceType == rt)
                        .ToList();
                }

                double revenue = 0;
                double opex = 0;
                double transport = 0;
                double commPrice = 0;

                foreach (SQ sq in playerList)
                {
                    var comm = db.Commodity.Where(p => p.Turn == QC.TurnNumber)
                        .Where(p => p.ResourceTypeNumber == (int)sq.ResourceType)
                        .FirstOrDefault();
                    commPrice = comm.Price;

                    revenue += sq.Production * commPrice;
                    opex += sq.Production * sq.OPEXPerUnit;
                    transport += sq.Production * sq.Transport;
                }
                return new Cash(revenue, opex, transport);                
            }
        }        
    }
}
