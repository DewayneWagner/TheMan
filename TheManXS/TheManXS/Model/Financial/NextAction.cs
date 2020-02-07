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

        public NextAction() { }
        public NextAction(SQ square)
        {
            _sq = square;
            Key = _sq.Key;
            UpdateNextAction();
        }
        public int Key { get; set; }
        public string NetActionText { get; set; }
        public double NextActionCost { get; set; }
        private SQ _sq;
        public void UpdateNextAction()
        {
            switch (_sq.Status)
            {
                case ST.Nada:
                    NetActionText = "Purchase Property";
                    NextActionCost = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.SquarePrice);
                    break;
                case ST.Unexplored:
                    NetActionText = "Explore for Resources";
                    NextActionCost = Setting.GetRand(AS.ExpTT, (int)_sq.TerrainType);
                    break;
                case ST.Explored:
                    NetActionText = "Develop Property";
                    NextActionCost = Setting.GetRand(AS.DevTT, (int)_sq.TerrainType) * _sq.Production;
                    break;
                case ST.Developing:
                    NetActionText = "Under Development";
                    NextActionCost = Setting.GetRand(AS.ProductionTT, (int)_sq.TerrainType) * _sq.Production;
                    break;
                case ST.Producing:
                    NetActionText = "Suspend Production";
                    NextActionCost = Setting.GetRand(AS.SusTT, (int)_sq.TerrainType) * _sq.Production;
                    break;
                case ST.Suspended:
                    NetActionText = "Reactive Property";
                    NextActionCost = Setting.GetRand(AS.ReactivateSingleP, (int)_sq.TerrainType) * _sq.Production;
                    break;
                default:
                    NetActionText = "Nada";
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
