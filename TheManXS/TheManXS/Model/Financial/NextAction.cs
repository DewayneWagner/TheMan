﻿using System;
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
        public NextAction(SQ sq) { UpdateNextAction(sq); }
        
        public string Text { get; set; }
        public double Cost { get; set; }
        
        private void UpdateNextAction(SQ sq)
        {
            switch (sq.Status)
            {
                case ST.Nada:
                    Text = "Purchase Property";
                    Cost = Setting.GetConstant(AS.CashConstant, (int)SettingsMaster.CashConstantParameters.SquarePrice);
                    break;
                case ST.Unexplored:
                    Text = "Explore for Resources";
                    Cost = Setting.GetRand(AS.ExpTT, (int)sq.TerrainType);
                    break;
                case ST.Explored:
                    Text = "Develop Property";
                    Cost = Setting.GetRand(AS.DevTT, (int)sq.TerrainType) * sq.Production;
                    break;
                case ST.Developing:
                    Text = "Under Development";
                    Cost = Setting.GetRand(AS.ProductionTT, (int)sq.TerrainType) * sq.Production;
                    break;
                case ST.Producing:
                    Text = "Suspend Production";
                    Cost = Setting.GetRand(AS.SusTT, (int)sq.TerrainType) * sq.Production;
                    break;
                case ST.Suspended:
                    Text = "Reactive Property";
                    Cost = Setting.GetRand(AS.ReactivateSingleP, (int)sq.TerrainType) * sq.Production;
                    break;
                default:
                    Text = "Nada";
                    break;
            }
        }     
    }
}
