using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using ST = TheManXS.Model.Settings.SettingsMaster.StatusTypeE;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.Model.Services.EntityFrameWork;
using AS = TheManXS.Model.Settings.SettingsMaster.AS;
using TheManXS.Model.Settings;
using System.Linq;

namespace TheManXS.Model.Financial
{
    public class NextAction
    {
        private SQ _sq;
        public NextAction() { }
        public NextAction(SQ square)
        {
            _sq = square;
            Key = _sq.Key;
            UpdateNextAction();
        }
        
        public int Key { get; set; }
        public string Text { get; set; }
        public double Cost { get; set; }
        
        public void UpdateNextAction()
        {
            switch (_sq.Status)
            {
                case ST.Nada:
                    Text = "Purchase Property";
                    Cost = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.SquarePrice);
                    break;
                case ST.Unexplored:
                    Text = "Explore for Resources";
                    Cost = Setting.GetRand(AS.ExpTT, (int)_sq.TerrainType);
                    break;
                case ST.Explored:
                    Text = "Develop Property";
                    Cost = Setting.GetRand(AS.DevTT, (int)_sq.TerrainType) * _sq.Production;
                    break;
                case ST.Developing:
                    Text = "Under Development";
                    Cost = Setting.GetRand(AS.ProductionTT, (int)_sq.TerrainType) * _sq.Production;
                    break;
                case ST.Producing:
                    Text = "Suspend Production";
                    Cost = Setting.GetRand(AS.SusTT, (int)_sq.TerrainType) * _sq.Production;
                    break;
                case ST.Suspended:
                    Text = "Reactive Property";
                    Cost = Setting.GetRand(AS.ReactivateSingleP, (int)_sq.TerrainType) * _sq.Production;
                    break;
                default:
                    Text = "Nada";
                    break;
            }
            //UpdateDB();
        }     
        //void UpdateDB()
        //{
        //    using (DBContext db = new DBContext())
        //    {
        //        db.NextAction.Add(this);
                
        //        //if(db.NextAction.Any(n => n.Key == this.Key)) { db.NextAction.Update(this); }
        //        //else { db.NextAction.Add(this); }
        //    }
        //}
    }
}
