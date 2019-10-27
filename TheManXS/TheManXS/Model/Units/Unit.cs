using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using QC = TheManXS.Model.Settings.QuickConstants;
using TheManXS.ViewModel.MapBoardVM.Tiles;
using Xamarin.Forms;
using TheManXS.Model.Company;

namespace TheManXS.Model.Units
{    
    public class Unit : List<SQ>
    {
        public enum UnitStatus { Tentative, Complete, }
        public Unit(SQ sq) 
        {
            Number = QC.UnitCounter + 1;
            Status = UnitStatus.Tentative;

            AddSQToUnit(sq);
        }
        public void AddSQToUnit(SQ sq)
        {
            Player p = (Player)Application.Current.Properties[Convert.ToString(App.ObjectsInPropertyDictionary.ActivePlayer)];
            Color c = new CompanyColors(p.Color).ColorXamarin;

            if (!this.Contains(sq))
            {
                this.Add(sq);
                OPEXDiscount += QC.OPEXDiscountPerSQInUnit;
                DevelopmentDiscount += QC.CAPEXDiscountPerSQInUnit;                
                new StaggeredBorder(c).InitStaggeredBorders(this);
            }
            else
            {
                this.Remove(sq);
                OPEXDiscount -= QC.OPEXDiscountPerSQInUnit;
                DevelopmentDiscount -= QC.CAPEXDiscountPerSQInUnit;
                new StaggeredBorder(c).InitStaggeredBorders(this);
            }
        }

        public int Number { get; set; }
        public UnitStatus Status { get; set; }
        public double OPEXDiscount { get; set; }
        public double DevelopmentDiscount { get; set; }
        
        public void CreateUnit()
        {
            using (DBContext db = new DBContext())
            {
                foreach (SQ sq in this)
                {
                    sq.IsPartOfUnit = true;
                    sq.UnitNumber = Number;
                    sq.OPEXPerUnit *= (1 - OPEXDiscount);                    
                }
                db.SaveChanges();
            }
        }
    }    
}
